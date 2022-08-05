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
    [<Emit("{ ...$0, ...$1, state: { ...$2 }, onStateChange: $3 }")>]
    let private spreadOptions prev options state onStateChange = jsNative
    [<Emit("(typeof $0 === 'function')")>]
    let private isJsFunc o = jsNative
    
    let private getCoreRowModel: unit -> obj = import "getCoreRowModel" "@tanstack/react-table"
    let private getFilteredRowModel : unit -> obj = import "getFilteredRowModel" "@tanstack/react-table"
    let private getPaginationRowModel : unit -> obj = import "getPaginationRowModel" "@tanstack/react-table"
    
    let rec internal nativeColumnDefs (columnDefs: ColumnDefOptionProp<'T> list list) =
        columnDefs
        |> Seq.map (fun colDef ->
            createObj [
                for option in colDef do
                    match option with
                    | Id s -> "id" ==> s
                    | AccessorKey s -> "accessorKey" ==> s
                    | AccessorFn f -> "accessorFn" ==> f
                    | HeaderStr s -> "header" ==> s
                    | FooterStr s -> "footer" ==> s
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
        static member columns<'T> (columns: ColumnDefOptionProp<'T> list list) =
            prop.custom ("columns", (nativeColumnDefs columns))
        static member inline onColumnVisibilityChange (fn: Dictionary<string, bool> -> Dictionary<string, bool>) =
            prop.custom ("onColumnVisibilityChange", fn)
        static member inline onColumnOrderChange (fn: string[] -> string[]) =
            prop.custom ("onColumnOrderChange", fn)
        static member inline onColumnOrderChange (fn: string[] -> unit) =
            prop.custom ("onColumnOrderChange", fn)
        static member inline columnOrder (order: string[]) =
            prop.custom ("columnOrder", order)
        static member inline columnResizeMode (resizeMode : ColumnResizeMode) =
            prop.custom ("columnResizeMode", (ColumnResizeMode.toString resizeMode))
        static member inline enableColumnResizing (enable : bool) =
            prop.custom ("enableColumnResizing", enable)
        static member inline autoResetPageIndex (autoReset : bool) =
            prop.custom ("autoResetPageIndex", autoReset)
            
        // Row Models
        static member filteredRowModel() =
            prop.custom ("getFilteredRowModel", getFilteredRowModel())
        static member paginationRowModel() =
            prop.custom ("getPaginationRowModel", getPaginationRowModel())
    
    type Table =
        static member private convertTable (dynamic : obj) (data : 'T []) : Table<'T> =
            { _obj = dynamic
              Data = data }
            
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
            
        static member setData (table : Table<'T>) (data : 'T[]) : Table<'T> =
            table._obj?options?data <- data
            table
            
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
            
        static member internal getColumn (o: obj, ?parent : Column<'T>) : Column<'T> option =
            if o = null then None
            // prevent a stack overflow from parsing columns
            // that then reference back to the same column
            else
                let parent = 
                    match parent with
                    | Some p -> p |> Some
                    | None -> (Table.getColumn (o?parent))
                
                let temp = 
                    { _obj = o
                      Id = o?id
                      Depth = o?depth
                      ColumnDef = Table.getColumnDef o?columnDef
                      Columns = [||]
                      Parent = parent }
                    
                let columns =
                    o?columns
                    |> Array.map (fun c -> Table.getColumn(c, temp))
                    |> Array.choose id
                    
                { temp with Columns = columns } |> Some
                
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
        
        static member private convertCells (dynamicCells : obj[]) : Cell<'T>[] =
            let cells =
                dynamicCells
                |> Array.map (fun c -> {
                    _obj = c
                    Id = c?id
                    Row = Table.getRow c?row
                    Column = (Table.getColumn c?column).Value
                })
            cells
            
        static member getVisibleCells (row : Row<'T>) : Cell<'T>[] =
            Table.convertCells (row._obj?getVisibleCells())
            
        static member getLeftVisibleCells (row : Row<'T>) : Cell<'T>[] =
            Table.convertCells (row._obj?getLeftVisibleCells())
        
        static member getCenterVisibleCells (row : Row<'T>) : Cell<'T>[] =
            Table.convertCells (row._obj?getCenterVisibleCells())
            
        static member getRightVisibleCells (row : Row<'T>) : Cell<'T>[] =
            Table.convertCells (row._obj?getRightVisibleCells())
            
        static member internal convertHeaders (o : seq<_>) : Header<'T>[] =
            let rec convertToHeader (o : seq<_>) : Header<'T>[] = [|
                for h in o do
                    { _obj = h
                      Id = h?id
                      Index = h?index
                      Depth = h?depth
                      Column = (Table.getColumn h?column).Value
                      ColSpan = h?colSpan
                      RowSpan = h?rowSpan
                      IsPlaceholder = h?isPlaceholder
                      PlaceholderId = h?placeholderId
                      SubHeaders = convertToHeader h?subHeaders }
            |]
            convertToHeader o
            
        static member private convertHeaderFooterGroups (groups : obj[]) : HeaderGroup<'T>[] =
            groups |> Array.map(fun g ->
                { _obj = g
                  Id = g?id
                  Depth = g?depth
                  Headers = Table.convertHeaders (g?headers) })
            
        static member getHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.convertHeaderFooterGroups (table._obj?getHeaderGroups())
            
        static member getLeftHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.convertHeaderFooterGroups (table._obj?getLeftHeaderGroups())
        
        static member getCenterHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.convertHeaderFooterGroups (table._obj?getCenterHeaderGroups())
            
        static member getRightHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.convertHeaderFooterGroups (table._obj?getRightHeaderGroups())
            
        static member getFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.convertHeaderFooterGroups (table._obj?getFooterGroups())
            
        static member getLeftFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.convertHeaderFooterGroups (table._obj?getLeftFooterGroups())
        
        static member getCenterFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.convertHeaderFooterGroups (table._obj?getCenterFooterGroups())
            
        static member getRightFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            Table.convertHeaderFooterGroups (table._obj?getRightFooterGroups())
            
        static member getAllLeafColumns (table : Table<'T>) : Column<'T>[] =
            table._obj?getAllLeafColumns() |> Array.choose Table.getColumn
            
        static member getRowModel (table : Table<'T>) : RowModel<'T> =
            let rowModel = table._obj?getRowModel()
            let rows = Table.convertToRows rowModel?rows
            { _obj = rowModel 
              Rows = rows }
            
        static member getCenterTotalSize (table : Table<'T>) : int =
            table._obj?getCenterTotalSize()
            
    type Html =
        static member flexRender<'T> (comp : obj, context : Context<'T>) =
            innerFlexRender(comp, context)
        
        static member flexRender<'T> (isPlaceholder : bool, comp : obj, context : Context<'T>) =
            if isPlaceholder then Html.none
            else innerFlexRender(comp, context)