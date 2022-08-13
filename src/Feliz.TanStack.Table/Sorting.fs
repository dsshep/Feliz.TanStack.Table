namespace Feliz.TanStack.Table

open Browser.Types
open Fable.Core.JsInterop

[<AutoOpen>]
module Sorting =

    type Column =
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
                