namespace Feliz.TanStack.Table

open System.Collections.Generic
open Feliz
open Fable.Core
open Fable.Core.JsInterop
open Feliz.TanStack.Table
open Core

[<AutoOpen>]
module rec Table =
    [<Import("createTable", from="@tanstack/table-core")>]
    let private createTable (options) = jsNative    
    [<Import("flexRender", from="@tanstack/react-table")>]
    let private innerFlexRender<'T>(comp: obj, context: Context<'T>) = jsNative
    let private getCoreRowModel: unit -> obj = import "getCoreRowModel" "@tanstack/react-table"
    [<Emit("{ ...$0, ...$1, state: { ...$2 }, onStateChange: $3 }")>]
    let private spreadOptions prev options state onStateChange = jsNative
    [<Emit("(typeof $0 === 'function')")>]
    let private isJsFunc o = jsNative
    
    let rec internal nativeColumnDefs (columnDefs: ColumnDefOptionProp<'T> list list) =
        columnDefs
        |> Seq.map (fun colDef ->
            createObj [
                for option in colDef do
                    match option with
                    | Id s -> "id" ==> s
                    | AccessorKey s -> "accessorKey" ==> s
                    | AccessorFn f -> "accessorFn" ==> f
                    | Header s -> "header" ==> s
                    | Footer s -> "footer" ==> s
                    | HeaderFn f -> "header" ==> f
                    | FooterFn f -> "footer" ==> f
                    | Cell f -> "cell" ==> f
                    | Columns def -> "columns" ==> (nativeColumnDefs def)
            ])
        |> Seq.toArray

    type TableStateProps<'T> =
        static member inline columnVisibility (visibilityState: Dictionary<string, bool>) =
            prop.custom("columnVisibility", visibilityState)
        static member inline columnOrder (columnOrderState: string[]) =
            prop.custom("columnOrder", columnOrderState)
        static member inline init (props: IReactProperty list) =
            (createObj !!props) :?> 'T
    
    type tableProps =
        static member inline data<'T> (data: 'T[]) =
            prop.custom ("data", data)
        static member inline columns<'T> (columns: ColumnDefOptionProp<'T> list list) =
            prop.custom ("columns", (nativeColumnDefs columns))
        static member inline onColumnVisibilityChange (fn: Dictionary<string, bool> -> Dictionary<string, bool>) =
            prop.custom ("onColumnVisibilityChange", fn)
        static member inline onColumnOrderChange (fn: string[] -> string[]) =
            prop.custom ("onColumnOrderChange", fn)
        static member inline onColumnOrderChange (fn: string[] -> unit) =
            prop.custom ("onColumnOrderChange", fn)
        static member inline columnOrder (order: string[]) =
            prop.custom ("columnOrder", order)
    
    type Table =
        static member private convertTable (dynamic : obj) (data : 'T []) : Table<'T> =
            let allColumns = dynamic?getAllColumns()
            
            { _obj = dynamic
              Data = data
              ColumnVisibility = allColumns |> Array.map (fun c -> (c, true)) |> Map.ofArray }
            
        static member init<'T> (options: IReactProperty list) : Table<'T> =
            let coreProps : IReactProperty list = [
                prop.custom ("getCoreRowModel", getCoreRowModel())
                prop.custom ("state", obj())
                prop.custom ("onStateChange", fun () -> ())
                prop.custom ("renderFallbackValue", null)
            ]
            
            let props = createObj !!(options |> List.append coreProps)
            let table = createTable props
            let initialState = table?initialState
            
            table?setOptions(fun prev ->
                let options = spreadOptions prev props initialState (fun () -> ())
                options)
            
            let data = props?data
            Table.convertTable table data
            
        static member getContext (context : #IContext) =
            context.Instance?getContext() :> Context<'T>
            
        static member private getColumnDef (o : obj) : ColumnDef<'T> =
            if o = null then Unchecked.defaultof<_>
            else 
                { _obj = o
                  Id = o?id
                  AccessorKey = o?accessorKey
                  Header = o?header
                  Footer = o?footer
                  Cell = if isJsFunc o?cell then Func o?cell else String o?cell }
            
        static member getCell (o : obj) : Cell<'T> =
            { _obj = o
              Id = o?id
              Row = Table.getRow o
              Column = (Table.getColumn o).Value }
            
        static member private getColumn (o: obj) : Column<'T> option =
            if o = null then None
            else
                let columns =
                    if o?columns = null then [||]
                    else o?columns |> Array.choose Table.getColumn
                
                { _obj = o
                  Id = o?id
                  Depth = o?depth
                  ColumnDef = Table.getColumnDef o?columnDef
                  Columns = columns
                  Parent = Table.getColumn o?parent } |> Some
            
        static member convertToRows (o : seq<_>) =
            if o = null then [||]
            else [| for r in o do
                    { _obj = r
                      Id = r?id
                      Depth = r?depth
                      Index = r?index
                      Original = r?original
                      SubRows = Table.convertToRows r?subRows } |]
            
        static member private getRow (o : obj) : Row<'T> =
            { _obj = o
              Id = o?id
              Depth = o?depth
              Index = o?index
              Original = o?original
              SubRows = Table.convertToRows o?subRows }
        
        static member getVisibleCells (row : Row<'T>) : Cell<'T>[] =
            let dynamicCells = row._obj?getVisibleCells()
            let cells =
                dynamicCells
                |> Array.map (fun c -> {
                    _obj = c
                    Id = c?id
                    Row = Table.getRow c
                    Column = (Table.getColumn c?column).Value
                })
            cells
            
        static member private getHeaderFooterGroups (header : bool) (table : Table<'T>) : HeaderGroup<'T>[] =
            let rec convertToHeader (o : seq<_>) : Header<'T>[] = [|
                for h in o do
                    { _obj = h
                      Id = h?id
                      Index = h?index
                      Depth = h?depth
                      Column = (Table.getColumn h?column).Value
                      ColSpan = h?colSpan
                      RowSpan = h?rowSpan
                      IsPlaceHolder = h?isPlaceHolder
                      PlaceHolderId = h?placeHolderId
                      SubHeaders = convertToHeader h?subHeaders }
            |]
            
            let dynamicHeaderGroups =
                if header then table._obj?getHeaderGroups()
                else table._obj?getFooterGroups()
            
            dynamicHeaderGroups |> Array.map(fun g ->
                { _obj = g
                  Id = g?id
                  Depth = g?depth
                  Headers = convertToHeader (g?headers) })
            
        static member getHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.getHeaderFooterGroups true table
            
        static member getFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.getHeaderFooterGroups false table
            
        static member getRowModel (table : Table<'T>) : RowModel<'T> =
            let rowModel = table._obj?getRowModel()
            let rows = Table.convertToRows rowModel?rows
            { _obj = rowModel 
              Rows = rows }
            
    type prop =
        static member inline flexRender<'T>(comp: obj, context: Context<'T>) =
            prop.children [
                innerFlexRender(comp, context)
            ]
