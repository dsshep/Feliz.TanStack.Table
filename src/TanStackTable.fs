module rec TanStackTable

open System.Collections.Generic
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Import("useReactTable", from="@tanstack/react-table")>]
let private useReactTable (model) = jsNative//: obj = import "useReactTable" "@tanstack/react-table"

[<Import("flexRender", from="@tanstack/react-table")>]
let private innerFlexRender<'T>(comp: obj, context: Context<'T>) = jsNative
let private coreRowModel: unit -> obj = import "getCoreRowModel" "@tanstack/react-table"


type HeaderFnProps<'T> =
    abstract member table: Table<'T>
    abstract member header: Header<'T>
    abstract member column: Column<'T>

[<Erase>]
type HeaderChoice<'T> =
    | String of string
    | Fn of (HeaderFnProps<'T> -> obj)

type Context<'T> = interface end

/// https://stackoverflow.com/questions/45465887/how-to-write-fable-bindings-for-react-component-library
type ColumnDefOption<'T> =
    | Id of string
    | AccessorKey of string
    | AccessorFn of ('T -> string)
    | Header of string
    | HeaderFn of (HeaderFnProps<'T> -> obj)
    | Footer of string
    | FooterFn of (HeaderFnProps<'T> -> obj)
    | Cell of (CellContext<'T> -> ReactElement)
    | Columns of ColumnDefOption<'T> list list

type CellContext<'T> =
    inherit Context<'T>
    abstract member table: Table<'T>
    abstract member column: Column<'T>
    abstract member row: Row<'T>
    abstract member cell: Cell<'T>
    abstract member getValue<'TValue> : unit -> 'TValue
    abstract member renderValue<'TValue> : unit -> 'TValue

type Cell<'T> =
    abstract member id: string
    abstract member getValue<'TValue> : unit -> 'TValue
    abstract member row: Row<'T>
    abstract member column: Column<'T>
    abstract member getContext: unit -> CellContext<'T>

type Row<'T> =
    abstract member id: string
    abstract member depth: int
    abstract member index: int
    abstract member original: 'T
    abstract member getValue<'TValue> : string -> 'TValue
    abstract member subRows: Row<'T> list
    abstract member getLeafRows: unit -> Row<'T>
    abstract member  originalSubRows: 'T list
    abstract member getAllCells: unit -> Cell<'T> list
    abstract member getVisibleCells: unit -> Cell<'T> list

type Column<'T> =
    abstract member id: string
    abstract member depth: int
    abstract member columnDef: ColumnDef<'T>
    abstract member columns: ColumnDef<'T> list
    abstract member parent: Column<'T>
    abstract member getFlatColumns: unit -> Column<'T> list
    abstract member getLeafColumns: unit -> Column<'T> list

type CellProps<'T> =
    abstract member table: Table<'T>
    abstract member row: Row<'T>
    abstract member column: Column<'T>
    abstract member getValue<'TValue> : unit -> 'TValue
    abstract member renderValue<'TValue> : unit -> 'TValue

type ColumnDef<'T> =
    abstract member id: string
    abstract member accessorKey: string
    abstract member accessorFn<'TValue> : originalRow: 'T * index: int -> 'TValue
    abstract member columns: ColumnDef<'T>  list
    abstract member header: string
    abstract member footer: string
    abstract member cell: CellProps<'T> -> obj

type HeaderContext<'T> =
    inherit Context<'T>
    abstract member table: Table<'T>
    abstract member header: Header<'T>
    abstract member column: Column<'T>

type Header<'T> =
    abstract member id: string
    abstract member index: int
    abstract member depth: int
    abstract member column: Column<'T>
    abstract member headerGroup: HeaderGroup<'T>
    abstract member subHeaders: Header<'T> list
    abstract member colSpan: int
    abstract member rowSpan: int
    abstract member getLeafHeaders: unit -> Header<'T> list
    abstract member isPlaceholder: bool
    abstract member placeholderId: string
    abstract member getContext: unit -> HeaderContext<'T>

type HeaderGroup<'T> =
    abstract member id: string
    abstract member depth: int
    abstract member headers: Header<'T> list

type RowModel<'T> =
    abstract member rows: Row<'T> list
    abstract member flatRows: Row<'T> list
    abstract member rowsById: Dictionary<string, Row<'T>>

type Table<'T> =
    abstract member data: 'T  list
    abstract member columns: ColumnDef<'T>  list
    abstract member defaultColumn: ColumnDef<'T> list
    abstract member reset: unit -> unit
    abstract member getState: unit -> obj
    abstract member setState: obj -> unit
    abstract member getCoreRowModel: unit -> RowModel<'T> 
    abstract member getRowModel: unit -> RowModel<'T>
    abstract member getAllColumns: unit -> Column<'T> list
    abstract member getAllFlatColumns: unit -> Column<'T> list
    abstract member getAllLeafColumns: unit -> Column<'T> list
    abstract member getColumn: id: string -> Column<'T>
    abstract member getHeaderGroups: unit -> HeaderGroup<'T> list
    abstract member getFooterGroups: unit -> HeaderGroup<'T> list
    abstract member getFlatHeaders: unit -> Header<'T> list
    abstract member getLeafHeaders: unit -> Header<'T> list

(*
| Header of string
    | HeaderFn of (HeaderFnProps<'T> -> obj)
    | Footer of string
    | FooterFn of (HeaderFnProps<'T> -> obj)
    | Cell of (CellContext<'T> -> ReactElement)
*)

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
let createTableComponent (data: 'T list) (columnDefs: ColumnDefOption<'T> list list) : Table<'T> =
    let columns = nativeColumnDefs columnDefs
    
    let tanStackTableComponent =
        useReactTable({| data = data; columns = columns; getCoreRowModel = coreRowModel |})    
    
    tanStackTableComponent

[<ReactComponent>]
let createElmishTable<'T> (data: 'T list) (columnDefs: ColumnDefOption<'T> list list) (render: Table<'T> -> ReactElement) : ReactElement =
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
    