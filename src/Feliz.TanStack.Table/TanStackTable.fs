namespace Feliz.TanStack.Table

open System.Collections.Generic
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<AutoOpen>]
module Table =
    [<Import("createTable", from="@tanstack/table-core")>]
    let private createTable (options) = jsNative    
    [<Import("useReactTable", from="@tanstack/react-table")>]
    let private useReactTable (options) = jsNative

    [<Import("flexRender", from="@tanstack/react-table")>]
    let private innerFlexRender<'T>(comp: obj, context: Context<'T>) = jsNative
    let private getCoreRowModel: unit -> obj = import "getCoreRowModel" "@tanstack/react-table"
    [<Emit("{ ...$0, ...$1, state: { ...$2 }, onStateChange: $3 }")>]
    let private spreadOptions prev options state onStateChange = jsNative
    [<Emit("{ ...$0, ...$1, onStateChange: $2 }")>]
    let setStateChange prev options stateChange = jsNative
    
    let rec internal nativeColumnDefs (columnDefs: ColumnDefOption<'T> list list) =
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
            (createObj !!props) :?> TableState<'T>
    
    type tableProps =
        static member inline data<'T> (data: 'T[]) =
            prop.custom ("data", data)
        static member inline columns<'T> (columns: ColumnDefOption<'T> list list) =
            prop.custom ("columns", (nativeColumnDefs columns))
        static member inline state<'T> (state: TableState<'T>) =
            prop.custom ("state", state)
        static member inline onColumnVisibilityChange (fn: Dictionary<string, bool> -> Dictionary<string, bool>) =
            prop.custom ("onColumnVisibilityChange", fn)
        static member inline onColumnOrderChange (fn: string[] -> string[]) =
            prop.custom ("onColumnOrderChange", fn)
        static member inline onColumnOrderChange (fn: string[] -> unit) =
            prop.custom ("onColumnOrderChange", fn)
        static member inline columnOrder (order: string[]) =
            prop.custom ("columnOrder", order)
    
    module ElmishTable =
        type Msg =
            | TableStateChange of obj
        
        let init<'T> (options: IReactProperty list) =
            let coreProps : IReactProperty list = [
                prop.custom ("getCoreRowModel", getCoreRowModel())
                prop.custom ("state", obj())
                prop.custom ("onStateChange", fun () -> ())
                prop.custom ("renderFallbackValue", null)
            ]
            
            let props = createObj !!(options |> List.append coreProps)
            let table = createTable props |> box :?> TableOptionsResolved<'T>
            let initialState = table.initialState
            
            table?setOptions(fun prev ->
                let options = spreadOptions prev props initialState (fun () -> ())
                options)
            table :?> Table<'T>
            
        let update (state: Table<'T>) (msg: Msg) =
            match msg with
            | TableStateChange tsc ->
                let initialState = state?initialState
                let options = state?options
                state?setOptions(fun prev ->
                    let options = spreadOptions prev options initialState (fun () -> ())
                    options)
                
        let view (state: Table<'T>) (dispatch: Msg -> unit) =
            state?setOptions(fun prev -> setStateChange prev state?options (TableStateChange >> dispatch))
            Html.none
            
        let render (state: Table<'T>) (dispatch: Msg -> unit) (view: ReactElement) =
            state?setOptions(fun prev -> setStateChange prev state?options (TableStateChange >> dispatch))
            view
    
    type Table =
        static member create<'T> (options: TableOptionsResolved<'T>) =
            let table : Table<'T> = createTable options
            table
        
        [<ReactComponent>]
        static member Create<'T> (props: TableOptionsResolved<'T>) : Table<'T> =
            let tanStackTableComponent = useReactTable props
            tanStackTableComponent
        
        [<ReactComponent>]
        static member Create (data: 'T list, columnDefs: ColumnDefOption<'T> list list, render: Table<'T> -> ReactElement) : ReactElement =
            let columns = nativeColumnDefs columnDefs
        
            let options = {| data = (data |> Array.ofList)
                             columns = columns
                             getCoreRowModel = getCoreRowModel() |}
            
            let tanStackTableComponent = useReactTable options
            //Fable.Core.JS.console.log (JS.JSON.stringify (tanStackTableComponent))
            render tanStackTableComponent
        
    type prop =
        static member inline flexRender<'T>(comp: obj, context: Context<'T>) =
            prop.children [
                innerFlexRender(comp, context)
            ]
