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
    let private spreadInitialOptions prev options state onStateChange = jsNative
    
    [<Emit("{ ...$0, ...$1, onStateChange: $2 }")>]
    let internal setStateChange prev options stateChange = jsNative
    
    [<Emit("{ ...$0, onStateChange: $1 }")>]
    let private spreadSecondaryOptions prev onStateChange = jsNative
    [<Emit("(typeof $0 === 'function')")>]
    let private isJsFunc o = jsNative
    
    [<Emit("{ _obj: $0 }")>]
    let private wrapObj o = jsNative
    
    let private getCoreRowModel: unit -> obj = import "getCoreRowModel" "@tanstack/table-core"
    let private getFilteredRowModel : unit -> obj = import "getFilteredRowModel" "@tanstack/table-core"
    let private getPaginationRowModel : unit -> obj = import "getPaginationRowModel" "@tanstack/table-core"
    let private getExpandedRowModel : unit -> obj = import "getExpandedRowModel" "@tanstack/table-core"
    
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
        static member getSubRows<'T, 'T2> (fn : 'T -> 'T2) =
            prop.custom ("getSubRows", fn)
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
        static member expandedRowModel() =
            prop.custom ("getExpandedRowModel", getExpandedRowModel())
            
        // debug
        static member debugAll() =
            prop.custom("debugAll", true)
        static member debugTable() =
            prop.custom ("debugTable", true)
    
    type Table =
        static member internal onStateChange (table : Table<'T>) : 'T2 -> 'T3 =
            fun updater ->
                let next = (Table.onStateChange table)
                if isJsFunc updater then
                    let state = table?_obj?getState()
                    let updatedState = (!!updater)(state)
                    
                    table?_obj?setOptions(fun prev ->
                        prev?state <- updatedState
                        let options = setStateChange prev table?_obj?options next
                        options)
                else
                    JS.debugger()
                    setStateChange2 (createObj []) next
            
        static member init<'T> (options: IReactProperty list) : Table<'T> =
            let coreProps : IReactProperty list = [
                prop.custom ("getCoreRowModel", getCoreRowModel())
                prop.custom ("state", obj())
                prop.custom ("onStateChange", fun () -> ())
                prop.custom ("renderFallbackValue", null)
            ]
            
            let props = createObj !!(options |> List.append coreProps)
            let table = createTable props
            let state = table?initialState
            
            table?setOptions(fun prev ->
                let options = spreadInitialOptions prev props state (fun () -> ())
                options)
            
            let data = props?data
            let table =
                wrapObj table
                |> merge (createObj [ "Data" ==> data ])
            
            table?_obj?setOptions(fun prev ->
                let options =
                    spreadSecondaryOptions
                        prev
                        (Table.onStateChange table)
                options)
            table
            
        static member setData (table : Table<'T>) (data : 'T[]) : Table<'T> =
            table?_obj?options?data <- data
            let table =
                wrapObj (table?_obj)
                |> merge (createObj [ "Data" ==> data ])
            
            table
        
        static member getContext (header : Header<'T>) : Context<'T> =
            header?getContext()
            
        static member getContext (cell : Cell<'T>) : Context<'T> =
            cell?getContext()
            
        static member getVisibleCells (row : Row<'T>) : Cell<'T>[] =
            row?getVisibleCells()
            
        static member getLeftVisibleCells (row : Row<'T>) : Cell<'T>[] =
            row?getLeftVisibleCells()
        
        static member getCenterVisibleCells (row : Row<'T>) : Cell<'T>[] =
            row?getCenterVisibleCells()
            
        static member getRightVisibleCells (row : Row<'T>) : Cell<'T>[] =
            row?getRightVisibleCells()
            
        static member getHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            table?_obj?getHeaderGroups()
            
        static member getLeftHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            table?_obj?getLeftHeaderGroups()
        
        static member getCenterHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            table?_obj?getCenterHeaderGroups()
            
        static member getRightHeaderGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            table?_obj?getRightHeaderGroups()
            
        static member getFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            table?_obj?getFooterGroups()
            
        static member getLeftFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            table?_obj?getLeftFooterGroups()
        
        static member getCenterFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            table?_obj?getCenterFooterGroups()
            
        static member getRightFooterGroups (table : Table<'T>) : HeaderGroup<'T>[] =
            table?_obj?getRightFooterGroups()
            
        static member getAllLeafColumns (table : Table<'T>) : Column<'T>[] =
            table?_obj?getAllLeafColumns()
            
        static member getRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getRowModel()
            
        static member getPreFilteredRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getPreFilteredRowModel()
            
        static member getCenterTotalSize (table : Table<'T>) : int =
            table?_obj?getCenterTotalSize()
            
    type Html =
        static member flexRender<'T> (comp : obj, context : Context<'T>) : ReactElement =
            innerFlexRender(comp, context)
        
        static member flexRender<'T> (isPlaceholder : bool, comp : obj, context : Context<'T>) : ReactElement =
            if isPlaceholder then Html.none
            else innerFlexRender(comp, context)
            
        static member flexRender<'T, 'TState, 'Msg> (state : 'TState, dispatch : 'Msg -> unit, comp : obj, context : Context<'T>) : ReactElement =
            if isJsFunc comp then 
                let obj = createObj [
                    "state" ==> state
                    "dispatch" ==> dispatch
                ]
                
                let props = merge context obj
                (unbox comp)(props)
            else innerFlexRender(comp, context)
            