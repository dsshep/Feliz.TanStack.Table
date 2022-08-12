namespace Feliz.TanStack.Table

open Fable.Core
open Fable.Core.JsInterop
open Feliz

[<AutoOpen>]
module rec Types =  
        
    [<Emit("{ ...$0, _obj: $0, Data: $0.options.data }")>]
    let private wrapTable table = jsNative
    
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
        | Cell of obj
        | AggregatedCell of obj
        | Columns of ColumnDefOptionProp<'T> list list
        | AggregationFn of obj
        
    type ColumnHelper =
        static member accessor (accessor: string, columnDefs: ColumnDefOptionProp<_> list) =
            columnDef.accessorKey accessor :: columnDefs
        static member accessor (accessorFn: 'T -> string, columnDefs: ColumnDefOptionProp<_> list) =
            columnDef.accessorFn accessorFn :: columnDefs
        static member createColumnHelper<'T> (columnDefs: ColumnDefOptionProp<'T> list list) =
            columnDefs
        
    type Aggregation =
        | Sum
        | Min
        | Max
        | Extent
        | Mean
        | Median
        | Unique
        | UniqueCount
        | Count
        | Auto
        with member this.asString() =
                match this with
                | Sum -> "sum"
                | Min -> "min"
                | Max -> "max"
                | Extent -> "extent"
                | Mean -> "mean"
                | Median -> "median"
                | Unique -> "unique"
                | UniqueCount -> "uniqueCount"
                | Count -> "count"
                | Auto -> "auto"
        
    type AggregationProps<'T> =
        abstract member getLeafRows: unit -> Row<'T>[]
        abstract member getChildRows: unit -> Row<'T>[]
        
    type columnDef =
        static member id s = Id s
        static member accessorKey s = AccessorKey s
        static member accessorFn fn = AccessorFn fn
        static member header s = HeaderStr s
        static member header<'T1, 'T2, 'State, 'Msg> (fn: HeaderProps<'T1, 'State, 'Msg> -> 'T2) : ColumnDefOptionProp<'T1> =
            (HeaderFn (fun props ->
                let table = wrapTable props?table
                props?table <- table
                fn props))
            
        static member footer s = FooterStr s
        static member footer<'T1, 'T2, 'TState, 'Msg> (fn: HeaderProps<'T1, 'TState, 'Msg> -> 'T2) : ColumnDefOptionProp<'T1> =
            (FooterFn fn) 
        static member cell<'T, 'State, 'Msg, 'T2> (fn: CellContextProp<'T, 'State, 'Msg> -> 'T2) : ColumnDefOptionProp<'T> =
            Cell (fun props ->
                let table = wrapTable props?table
                props?table <- table
                fn props)
            
        static member aggregationFn<'T> (fn : AggregationProps<'T> -> obj) : ColumnDefOptionProp<'T> =
            AggregationFn fn
        static member aggregationFn<'T> (aggregation : Aggregation) : ColumnDefOptionProp<'T> =
            AggregationFn (aggregation.asString())
        static member aggregatedCell<'T, 'State, 'Msg, 'T2> (fn: CellContextProp<'T, 'State, 'Msg> -> 'T2) : ColumnDefOptionProp<'T> =
            AggregatedCell (fun props ->
                let table = wrapTable props?table
                props?table <- table
                fn props)
            
        static member columns columnDef = Columns columnDef

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
        
        