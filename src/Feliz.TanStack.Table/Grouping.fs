namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Grouping =
    type Column =
        static member getCanGroup (column : Column<'T>) : bool =
            column?getCanGroup()

        static member getIsGrouped (column : Column<'T>) : bool =
            column?getIsGrouped()
            
        static member getGroupedIndex (column : Column<'T>) : int =
            column?getGroupedIndex()
            
        static member toggleGrouping (column : Column<'T>) : unit =
            if Column.getCanGroup column then
                column?toggleGrouping()
            
    type Cell =
        static member getIsGrouped (cell : Cell<'T>) : bool =
            cell?getIsGrouped()
            
        static member getIsAggregated (cell : Cell<'T>) : bool =
            cell?getIsAggregated()
            
        static member getIsPlaceholder (cell : Cell<'T>) : bool =
            cell?getIsPlaceholder()