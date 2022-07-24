namespace Feliz.TanStack.Table

open Feliz.TanStack.Table

[<AutoOpen>]
module Table = 

    open Feliz
    open Fable.Core
    open Fable.Core.JsInterop

    [<Import("useReactTable", from="@tanstack/react-table")>]
    let private useReactTable (model) = jsNative

    [<Import("flexRender", from="@tanstack/react-table")>]
    let innerFlexRender<'T>(comp: obj, context: Context<'T>) = jsNative
    let private coreRowModel: unit -> obj = import "getCoreRowModel" "@tanstack/react-table"

    let rec private nativeColumnDefs (columnDefs: ColumnDefOption<'T> list list) =
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

    [<ReactComponent>]
    let createTanStackTable<'T> (data: 'T list) (columnDefs: ColumnDefOption<'T> list list) (render: Table<'T> -> ReactElement) : ReactElement =
        let columns = nativeColumnDefs columnDefs
        
        let options = {| data = (data |> Array.ofList)
                         columns = columns
                         getCoreRowModel = coreRowModel() |}
        
        let tanStackTableComponent = useReactTable options
        render tanStackTableComponent
        
    type prop =
        static member inline flexRender<'T>(comp: obj, context: Context<'T>) =
            prop.children [
                innerFlexRender(comp, context)
            ]
        