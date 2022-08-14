namespace Feliz.TanStack.Table

open Browser.Types
open Fable.Core
open Fable.Core.JsInterop

[<AutoOpen>]
module Sorting =

    type Column =
        static member getAutoSortingFn (column : Column<'T>) : SortingFn<'T> =
            column?getAutoSortingFn()
        
        static member getAutoSortDir (column : Column<'T>) : SortDirection =
            let dir = column?getAutoSortDir()
            match dir with
            | "asc" -> SortDirection.Asc
            | "desc" -> SortDirection.Desc
            | _ -> SortDirection.NotSorted
        
        static member getSortingFn (column : Column<'T>) : SortingFn<'T> =
            column?getSortingFn()
        
        static member getNextSortingOrder (column : Column<'T>) : SortDirection =
            match column?getNextSortingOrder() with
            | "asc" -> SortDirection.Asc
            | "desc" -> SortDirection.Desc
            | _ -> SortDirection.NotSorted
        
        static member getCanSort (column : Column<'T>) : bool =
            column?getCanSort()
        
        static member getSortIndex (column : Column<'T>) : int =
            column?getSortIndex()
        
        static member getIsSorted (column : Column<'T>) : SortDirection =
            let isSorted = column?getIsSorted()
            match isSorted with
            | "asc" -> Asc
            | "desc" -> Desc
            | _ -> NotSorted
            
        static member clearSorting (column : Column<'T>) : unit =
            column?clearSorting()
            
        static member getCanMultiSort (column : Column<'T>) : bool =
            column?getCanMultiSort()
            
        static member toggleSorting (column : Column<'T>) : unit =
            if Column.getCanSort column then
                column?toggleSorting(null, false)
                
        static member toggleSorting (column : Column<'T>, event : Event) =
            if Column.getCanSort column then
                let canMultiSort = Column.getCanMultiSort column
                let isMultiSortEvent = event?shiftKey
                column?toggleSorting(null, (canMultiSort && isMultiSortEvent))
                
    type Table =
        static member setSorting (sortingState : RowSort[] -> RowSort[]) (table : Table<'T>) : Table<'T> =
            table?_obj?setSorting(sortingState)
        
        static member resetSorting (defaultState: bool) (table : Table<'T>) : Table<'T> =
            table?_obj?resetSorting(defaultState)
            table
            
        static member getPreSortedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getPreSortedRowModel()
            
        static member getSortedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getSortedRowModel()