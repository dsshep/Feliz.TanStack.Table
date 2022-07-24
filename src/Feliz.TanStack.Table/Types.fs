namespace Feliz.TanStack.Table

[<AutoOpen>]
module rec Types =  
        
    open System.Collections.Generic
    open Feliz

    type HeaderFnProps<'T> =
        abstract member table: Table<'T>
        abstract member header: Header<'T>
        abstract member column: Column<'T>

    type Context<'T> = interface end

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
