namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Grouping =
    let private objToAggregation (agg : obj) =
        if isJsFunc agg then AggregationFn (unbox agg) |> Some
        else
            match (string agg) with
            | "sum" -> Some Sum
            | "min" -> Some Min
            | "max" -> Some Max
            | "extent" -> Some Extent
            | "mean" -> Some Mean
            | "median" -> Some Median
            | "unique" -> Some Unique
            | "uniqueCount" -> Some UniqueCount
            | "count" -> Some Count
            | "auto" -> Some Auto
            | _ -> None
            |> Option.map Aggregation
            
    type Column =
        static member aggregationFn (column : Column<'T>) : AggregationFnOption<'T> option =
            let agg = column?aggregationFn
            objToAggregation agg
        
        static member getCanGroup (column : Column<'T>) : bool =
            column?getCanGroup()

        static member getIsGrouped (column : Column<'T>) : bool =
            column?getIsGrouped()
            
        static member getGroupedIndex (column : Column<'T>) : int =
            column?getGroupedIndex()
            
        static member toggleGrouping (column : Column<'T>) : unit =
            if Column.getCanGroup column then
                column?toggleGrouping()
        
        static member getAutoAggregationFn (column : Column<'T>) : AggregationFnOption<'T> option =
            column?getAutoAggregationFn() |> objToAggregation
        
        static member getAggregationFn (column : Column<'T>) : AggregationFnOption<'T> option =
            column?getAggregationFn() |> objToAggregation
            
    type Row =
        static member getIsGrouped (row : Row<'T>) =
            row?getIsGrouped()
            
    type Table =
        static member setGrouping (grouping : string[] -> string[]) (table : Table<'T>) : Table<'T> =
            table?_obj?setGrouping(grouping)
            table
            
        static member resetGrouping (defaultValue : bool) (table : Table<'T>) : Table<'T> =
            table?_obj?resetGrouping(defaultValue)
            table
            
        static member getPreGroupedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getPreGroupedRowModel()
            
        static member getGroupedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getGroupedRowModel()
            
    type Cell =
        static member getIsGrouped (cell : Cell<'T>) : bool =
            cell?getIsGrouped()
            
        static member getIsAggregated (cell : Cell<'T>) : bool =
            cell?getIsAggregated()
            
        static member getIsPlaceholder (cell : Cell<'T>) : bool =
            cell?getIsPlaceholder()