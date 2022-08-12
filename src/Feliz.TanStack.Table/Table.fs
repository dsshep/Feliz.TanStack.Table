namespace Feliz.TanStack.Table

open Feliz
open Fable.Core
open Fable.Core.JsInterop
open Feliz.TanStack.Table

[<AutoOpen>]
module rec Table =
    [<Import("createTable", from="@tanstack/table-core")>]
    let private createTable (options) = jsNative
    
    [<Emit("{ ...$0, ...$1, state: { ...$2 }, onStateChange: $3 }")>]
    let private spreadInitialOptions prev options state onStateChange = jsNative
    
    [<Emit("{ ...$0, ...$1, onStateChange: $2 }")>]
    let internal setInitialState prev options stateChange = jsNative
    
    [<Emit("{ ...$0, onStateChange: $1 }")>]
    let private spreadSecondaryOptions prev onStateChange = jsNative
    
    [<Emit("{ _obj: $0 }")>]
    let internal wrapObj o = jsNative
    
    type Table =
        static member internal onStateChange (table : Table<'T>) : 'T2 -> 'T3 =
            fun updater ->
                let next = (Table.onStateChange table)
                let state = table?_obj?getState()
                let updatedState =
                    if isJsFunc updater then (!!updater)(state)
                    else merge state updater
                
                table?_obj?setOptions(fun prev ->
                    prev?state <- updatedState
                    let options = setInitialState prev table?_obj?options next
                    options)
                
            
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
            if isJsFunc comp then (unbox comp) context
            else !!comp
        
        static member flexRender<'T, 'TState, 'Msg> (state : 'TState, dispatch : 'Msg -> unit, comp : obj, context : Context<'T>) : ReactElement =
            if isJsFunc comp then 
                let obj = createObj [
                    "state" ==> state
                    "dispatch" ==> dispatch
                ]
                
                let props = merge context obj
                (unbox comp)(props)
            else !!comp
            