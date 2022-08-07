namespace Feliz.TanStack.Table

open Fable.Core
open Feliz

//open Feliz.TanStack.Table

[<AutoOpen>]
module rec Types =  
        
    type HeaderFnProps<'T> =
        abstract member table: TableProps<'T>
        abstract member header: HeaderProp<'T>
        abstract member column: ColumnProp<'T>

    type Context<'T> = interface end

    type ColumnResizeMode =
        | OnChange
        | OnEnd
        static member toString (resizeMode : ColumnResizeMode) =
            match resizeMode with
            | OnChange -> "onChange"
            | OnEnd -> "onEnd"
    
    type InternalContext<'T> = {
        _obj : obj
        Table : Table<'T>
        Header : Header<'T>
        Column : Column<'T>
    }
    
    type ColumnDefOptionProp<'T> =
        internal
        | Id of string
        | AccessorKey of string
        | AccessorFn of ('T -> string)
        | HeaderStr of string
        | HeaderFn of obj
        | FooterStr of string
        | FooterFn of obj
        | Cell of (CellContextProp<'T> -> ReactElement)
        | Columns of ColumnDefOptionProp<'T> list list
        
    type ColumnHelper =
        static member accessor (accessor: string, columnDefs: ColumnDefOptionProp<_> list) =
            columnDef.accessorKey accessor :: columnDefs
        static member accessor (accessorFn: 'T -> string, columnDefs: ColumnDefOptionProp<_> list) =
            columnDef.accessorFn accessorFn :: columnDefs
        static member createColumnHelper<'T> (columnDefs: ColumnDefOptionProp<'T> list list) =
            columnDefs
        
    type columnDef =
        static member id s = Id s
        static member accessorKey s = AccessorKey s
        static member accessorFn fn = AccessorFn fn
        static member header s = HeaderStr s
        static member header<'T1, 'T2> (fn: HeaderFnProps<'T1> -> 'T2) = (HeaderFn fn) : ColumnDefOptionProp<'T1>
        static member footer s = FooterStr s
        static member footer<'T1, 'T2> (fn: HeaderFnProps<'T1> -> 'T2) = (FooterFn fn) : ColumnDefOptionProp<'T1>
        static member cell<'T> (fn: CellContextProp<'T> -> ReactElement) = Cell fn
        static member columns columnDef = Columns columnDef

    type CellContextProp<'T> =
        inherit Context<'T>
        //abstract member table: Table<'T>
        //abstract member column: Column<'T>
        //abstract member row: Row<'T>
        //abstract member cell: Cell<'T>
        abstract member getValue<'TValue> : unit -> 'TValue
        abstract member renderValue<'TValue> : unit -> 'TValue
//
//    type Cell<'T> =
//        abstract member id: string
//        abstract member getValue<'TValue> : unit -> 'TValue
//        abstract member row: Row<'T>
//        abstract member column: Column<'T>
//        abstract member getContext: unit -> CellContext<'T>
//
//    type Row<'T> =
//        abstract member id: string
//        abstract member depth: int
//        abstract member index: int
//        abstract member original: 'T
//        abstract member getValue<'TValue> : string -> 'TValue
//        abstract member subRows: Row<'T> list
//        abstract member getLeafRows: unit -> Row<'T>
//        abstract member  originalSubRows: 'T list
//        abstract member getAllCells: unit -> Cell<'T> list
//        abstract member getVisibleCells: unit -> Cell<'T> list
//
    type CoreColumnProp<'T> =
        abstract member id: string
        abstract member depth: int
        abstract member accessorFn: obj
        //abstract member columnDef: ColumnDef<'T>
        //abstract member columns: ColumnDef<'T> list
        abstract member parent: Column<'T>
        abstract member getFlatColumns: unit -> Column<'T> list
        abstract member getLeafColumns: unit -> Column<'T> list
//    
//    type VisibilityColumn<'T> =
//        abstract member getToggleVisibilityHandler: unit -> event: obj -> unit
//    
    type ColumnProp<'T> =
        inherit CoreColumnProp<'T>
        //inherit VisibilityColumn<'T>
//
//    type CellProps<'T> =
//        abstract member table: Table<'T>
//        abstract member row: Row<'T>
//        abstract member column: Column<'T>
//        abstract member getValue<'TValue> : unit -> 'TValue
//        abstract member renderValue<'TValue> : unit -> 'TValue
//
//    type ColumnDef<'T> =
//        abstract member id: string
//        abstract member accessorKey: string
//        abstract member accessorFn<'TValue> : originalRow: 'T * index: int -> 'TValue
//        abstract member columns: ColumnDef<'T>  list
//        abstract member header: string
//        abstract member footer: string
//        abstract member cell: CellProps<'T> -> obj
//
//    type HeaderContext<'T> =
//        inherit Context<'T>
//        abstract member table: Table<'T>
//        abstract member header: Header<'T>
//        abstract member column: Column<'T>

    type HeaderProp<'T> =
        abstract member id: string
        abstract member index: int
        abstract member depth: int
        //abstract member column: Column<'T>
        //abstract member headerGroup: HeaderGroup<'T>
        abstract member subHeaders: HeaderProp<'T> list
        abstract member colSpan: int
        abstract member rowSpan: int
        abstract member getLeafHeaders: unit -> HeaderProp<'T> list
        abstract member isPlaceholder: bool
        abstract member placeholderId: string
        //abstract member getContext: unit -> HeaderContext<'T>
//
//    type HeaderGroup<'T> =
//        abstract member id: string
//        abstract member depth: int
//        abstract member headers: Header<'T> list
//
//    type RowModel<'T> =
//        abstract member rows: Row<'T> list
//        abstract member flatRows: Row<'T> list
//        abstract member rowsById: Dictionary<string, Row<'T>>
//
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
//   
//    type ReactTable<'T> =
//        inherit Table<'T>
//        inherit ReactElement
//    
//    type FilterFn<'T> = Row<'T> * string * obj -> bool
//    
//    [<Erase>]
//    type FilterFnOption<'T> =
//        | String of string
//        | FilterFn of FilterFn<'T>
//    
//    type FiltersOption<'T> =
//        abstract member enableFilters: bool;
//        abstract member manualFiltering: bool;
//        abstract member filterFromLeafRows: bool;
//        abstract member getFilteredRowModel: table: Table<obj> -> unit -> RowModel<obj>;
//        abstract member onColumnFiltersChange: (ColumnFilter[] -> ColumnFilter[]) -> unit;
//        abstract member enableColumnFilters: bool;
//        abstract member globalFilterFn: FilterFnOption<'T>;
//        abstract member onGlobalFilterChange: (obj -> obj) -> unit;
//        abstract member enableGlobalFilter: bool;
//        abstract member getColumnCanGlobalFilter: column: Column<'T> -> bool;
//        abstract member getFacetedRowModel: table: Table<'T> * columnId: string -> unit -> RowModel<'T>;
//        //abstract member getFacetedUniqueValues: (table: Table<TData>, columnId: string) => () => Map<any, number>;
//        //abstract member getFacetedMinMaxValues: (table: Table<TData>, columnId: string) => () => undefined | [number, number];
//    
//    type TableOptions<'T> =
//        inherit CoreOptions<'T>
//        abstract member state: TableState<'T> with get, set
//    
//    type TableOptionsResolved<'T> =
//        inherit CoreOptions<'T>
//        abstract member onColumnVisibilityChange: (Dictionary<string, bool> -> Dictionary<string, bool>) -> unit
//        abstract member onColumnVisibilityChange: (Dictionary<string, bool> -> unit) -> unit
//        abstract member enableHiding: bool with get, set
//        abstract member onColumnOrderChange: (string[] -> string[]) -> unit
//        abstract member onColumnOrderChange: (string[] -> unit) -> unit
//        abstract member onColumnPinningChange: (obj -> obj)  -> unit
//        abstract member enablePinning: bool with get, set
//    
//    type ColumnPinningState =
//        abstract member left: string[]
//        abstract member right: string[]
//    
//    type ColumnFilter =
//        abstract member id: string
//        abstract member value: string
//    
//    type ColumnSort =
//        abstract member id: string
//        abstract member desc: bool
//    
    type ColumnSizingInfoState = {
        StartOffset: int
        StartSize: int
        DeltaOffset: int
        DeltaPercentage: int
        IsResizingColumn: string
        ColumnSizingStart: (string * int)[]
    }
//    
//    type PaginationState =
//        abstract member pageIndex: int
//        abstract member pageSize: int
//    
//    type RowSelectionTableState =
//        abstract member rowSelection: Dictionary<string, bool>
//    
//    type CoreOptions<'T> =
//        abstract member data: 'T[];
//        abstract member onStateChange: updater: (TableState<'T> -> TableState<'T>) -> unit
//        abstract member onStateChange: updater: (TableState<'T> -> unit) -> unit;
//        abstract member debugAll: bool;
//        abstract member debugTable: bool;
//        abstract member debugHeaders: bool;
//        abstract member debugColumns: bool;
//        abstract member debugRows: bool;
//        abstract member initialState: TableState<'T>;
//        abstract member autoResetAll: bool;
//        abstract member getCoreRowModel: table: Table<'T> -> unit -> obj;
//        abstract member getSubRows: originalRow: 'T * index: int -> 'T[] option;
//        abstract member getRowId: originalRow: 'T * index: int * parent: Row<'T> -> string;
//        abstract member columns: ColumnDef<'T>[];
//        abstract member defaultColumn: ColumnDef<'T>;
//        abstract member renderFallbackValue: obj
    
    type IContext =
        abstract member Instance: obj
    
    [<Erase>]
    type StringOrFunc =
        | String of string
        | Func of obj
    
    type Cell<'T> = {
        _obj: obj
        Id: string
        Row: Row<'T>
        Column: Column<'T>
    } with interface IContext with
            member this.Instance = this._obj
    
    type ColumnDef<'T> = {
        _obj: obj
        Id: string
        AccessorKey: string
        //abstract member accessorFn<'TValue> : originalRow: 'T * index: int -> 'TValue
        //Columns: ColumnDef<'T> []
        Header: string
        Footer: string
        Cell: StringOrFunc
    }
        //abstract member cell: CellProps<'T> -> obj
    
    type Column<'T> = {
        _obj: obj
        Id: string
        Depth: int
        //abstract member accessorFn: obj
        ColumnDef: ColumnDef<'T>
        Columns: Column<'T> []
        Parent: Column<'T> option
    }
    
    type Header<'T> = {
        _obj: obj
        Id: string
        Index: int
        Depth: int
        SubHeaders: Header<'T>[]
        Column: Column<'T>
        ColSpan: int
        RowSpan: int
        IsPlaceholder: bool
        PlaceholderId: string
    } with interface IContext with
            member this.Instance = this._obj
    
    type HeaderGroup<'T> = {
        _obj: obj
        Id: string
        Depth: int
        Headers: Header<'T>[]
    }
    
    type Row<'T> = {
        _obj: obj
        Id: string
        Depth: int
        Index: int
        Original: 'T
        //GetValue: string -> obj
        SubRows: Row<'T> []
        //GetLeafRows: unit -> Row<'T>
        //OriginalSubRows: 'T []
        //GetAllCells: unit -> Cell<'T> list
        //GetVisibleCells: unit -> Cell<'T> list
    }
    
    type RowModel<'T> = {
        _obj: obj
        Rows: Row<'T> []
        FlatRows: Row<'T>[]
    }
    
    type Table<'T> = {
        _obj: obj
        Data: 'T []
    }
    
    type TableState<'T> = {
        _obj: obj
        ColumnSizingInfo: ColumnSizingInfoState
    }
        
