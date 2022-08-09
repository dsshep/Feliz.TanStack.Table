﻿namespace Feliz.TanStack.Table

open Fable.Core
open Fable.Core.JsInterop
open Feliz

[<AutoOpen>]
module rec Types =  
        
    type HeaderFnProps<'T, 'State, 'Msg> =
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
        static member header<'T1, 'T2, 'State, 'Msg> (fn: HeaderFnProps<'T1, 'State, 'Msg> -> 'T2) : ColumnDefOptionProp<'T1> =
            (HeaderFn (fun props ->
                props?table?_obj <- props?table
                fn props)) 
        static member footer s = FooterStr s
        static member footer<'T1, 'T2, 'TState, 'Msg> (fn: HeaderFnProps<'T1, 'TState, 'Msg> -> 'T2) = (FooterFn fn) : ColumnDefOptionProp<'T1>
        static member cell<'T, 'State, 'Msg> (fn: CellContextProp<'T, 'State, 'Msg> -> ReactElement) : ColumnDefOptionProp<'T> =
            Cell (fun props ->
                props?table?_obj <- props?table
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
        
    type PaginationState =
        abstract member pageIndex: int
        abstract member pageSize: int
        
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
    
    type TableState<'T> =
        abstract member pagination: PaginationState
        