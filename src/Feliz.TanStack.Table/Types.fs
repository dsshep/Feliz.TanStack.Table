namespace Feliz.TanStack.Table

open Fable.Core

[<AutoOpen>]
module rec Types =  
    type HeaderProps<'T, 'State, 'Msg> =
        abstract member table: Table<'T>
        abstract member header: Header<'T>
        abstract member column: Column<'T>
        abstract member state: 'State
        abstract member dispatch: 'Msg -> unit

    type Context<'T> = interface end

    type ColumnResizeMode =
        | OnChange
        | OnEnd
        static member toString (resizeMode : ColumnResizeMode) =
            match resizeMode with
            | OnChange -> "onChange"
            | OnEnd -> "onEnd"
    
    type ColumnDefOptionProp<'T> =
        internal
        | Id of string
        | AccessorKey of string
        | AccessorFn of ('T -> string)
        | HeaderStr of string
        | HeaderFn of obj
        | FooterStr of string
        | FooterFn of obj
        | Cell of obj
        | AggregatedCell of obj
        | Columns of ColumnDefOptionProp<'T> list list
        | AggregationFn of obj
        | Size of int
        | MinSize of int
        | MaxSize of int
        | EnableHiding of bool
        | SortingFn of obj
        | SortDescFirst of bool
        | EnableSorting of bool
        | EnableMultiSort of bool
        | InvertSorting of bool
        | SortUndefined of obj
        
    type CellContextProp<'T, 'State, 'Msg> =
        inherit Context<'T>
        abstract member table: Table<'T>
        abstract member header: Header<'T>
        abstract member column: Column<'T>
        abstract member row: Row<'T>
        abstract member state: 'State
        abstract member dispatch: 'Msg -> unit
        abstract member getValue<'TValue> : unit -> 'TValue
        abstract member renderValue<'TValue> : unit -> 'TValue
        
    type TableProps<'T> =
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
        
    type SortingFn<'T> = Row<'T> -> Row<'T> -> string -> int
        
    type SortDirection =
        | Asc
        | Desc
        | NotSorted
        
    [<Erase>]
    type StringOrFunc =
        | String of string
        | Func of obj
    
    type Cell<'T> =
        abstract member id: string
        abstract member row: Row<'T>
        abstract member column: Column<'T>
    
    type ColumnDef<'T> = 
        abstract member id: string
        abstract member accessorKey: string
        abstract member header: string
        abstract member footer: string
        abstract member cell: StringOrFunc
        abstract member aggregatedCell: obj
    
    type Column<'T> =
        abstract member id: string
        abstract member depth: int
        abstract member columnDef: ColumnDef<'T>
        abstract member columns: Column<'T> []
        abstract member parent: Column<'T> option 
    
    type Header<'T> =
        abstract member id: string
        abstract member index: int
        abstract member depth: int
        abstract member subHeaders: Header<'T>[]
        abstract member column: Column<'T>
        abstract member colSpan: int
        abstract member rowSpan: int
        abstract member isPlaceholder: bool
        abstract member placeholderId: string
    
    type HeaderGroup<'T> =
        abstract member id: string
        abstract member depth: int
        abstract member headers: Header<'T>[]
    
    type Row<'T> = 
        abstract member id: string
        abstract member depth: int
        abstract member index: int
        abstract member original: 'T
        abstract member subRows: Row<'T> []
    
    type RowModel<'T> =
        abstract member rows: Row<'T> []
        abstract member flatRows: Row<'T>[] 
    
    type Table<'T> = 
        abstract member Data: 'T []
    
    type Record<'T> =
        abstract member Item: string -> 'T with get
    
    type VisibilityState =
        inherit Record<bool>
    
    type PaginationState =
        abstract member pageIndex: int
        abstract member pageSize: int
    
    type ColumnPinningState =
        abstract member left: string[]
        abstract member right: string[]
        
    type ColumnFilter =
        abstract member id: string
        abstract member value: obj
    
    type ColumnSort =
        abstract member id: string
        abstract member desc: bool
        
    type RowSort =
        abstract member id: string
        abstract member desc: bool
    
    [<Erase>]
    type ExpandedState =
        | Bool of bool
        | State of Record<bool>
    
    type ColumnSizingInfoState =
        abstract member startOffset: int option
        abstract member startSize: int option
        abstract member deltaOffset: int option
        abstract member isResizingColumn: U2<bool, string>
        abstract member columnSizingStart: (string * int)[]
    
    type TableState<'T> =
        abstract member columnVisibility: VisibilityState
        abstract member columnOrder: string[]
        abstract member columnPinning: ColumnPinningState
        abstract member columnFilters: ColumnFilter[]
        abstract member globalFilter: obj
        abstract member sorting: ColumnSort[]
        abstract member expanded: ExpandedState
        abstract member grouping: string[]
        abstract member columnSizing: Record<int>
        abstract member columnSizingInfo: ColumnSizingInfoState
        abstract member pagination: PaginationState
        abstract member rowSelection: Record<bool>
        
        