namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Core

type ColumnPinningPosition =
    | Left
    | Right
    | Neither

[<AutoOpen>]
module Column = 

    type Column =
        
        static member getIsVisible (column : Column<'T>) : bool =
            column?getIsVisible()

        static member getCanPin (column : Column<'T>) : bool =
            column?getCanPin()
        
        static member getIsPinned (column : Column<'T>) : ColumnPinningPosition =
            let isPinned : string = column?getIsPinned()
            let state =
                match isPinned with
                | "left" -> Left
                | "right" -> Right
                | _ -> Neither
            state
            
        static member getIsResizing (column : Column<'T>) : bool =
            column?getIsResizing()
    
        static member getSize (column : Column<'T>) : int =
            column?getSize()
            
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
            
    type Table =
        static member setColumnFilter (value : 'TValue) (column : Column<'T>) : Column<'T> =
            column?setFilterValue(value)
            column
        
        static member pinColumn (position : ColumnPinningPosition) (column : Column<'T>) (table : Table<'T>) : Table<'T> =
            let positionStr =
                match position with
                | Left -> "left"
                | Right -> "right"
                | _ -> "false"
            column?pin(positionStr)
            
            table
        