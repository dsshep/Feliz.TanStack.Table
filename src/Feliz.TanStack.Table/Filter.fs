namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Filter =
    type Column =
        static member getCanFilter (column : Column<'T>) : bool =
            column?getCanFilter()
            
        static member getFilterValue (column : Column<'T>) : 'T2 option =
            let filterValue = column?getFilterValue()
            if nullOrUndefined filterValue then None else Some filterValue
            
        static member setFilterValue (value : 'T2 option -> 'T2) (column : Column<'T>) : Column<'T> =
            column?setFilterValue(fun x ->
                if (isNullOrUndefined x) then value None
                else value (Some x))
            column
            
        static member getCanGlobalFilter (column : Column<'T>) : bool =
            column?getCanGlobalFilter()
            
        static member getFilterIndex (column : Column<'T>) : int =
            column?getFilterIndex()
            
        static member getIsFiltered (column : Column<'T>) : bool =
            column?getIsFiltered()
            
    type Row =
        static member columnFilters (row : Row<'T>) : Record<bool> =
            row?columnFilters
            
        static member columnFiltersMeta (row : Row<'T>) : Record<obj> =
            row?columnFiltersMeta
            
    type Table =
        static member filterFns (table : Table<'T>) : obj =
            table?filterFns
            
        static member filterFromLeafRows (table : Table<'T>) : bool =
            table?filterFromLeafRows