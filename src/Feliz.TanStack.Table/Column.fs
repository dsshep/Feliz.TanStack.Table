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
            column._obj?getIsVisible()

        static member getCanPin (column : Column<'T>) : bool =
            column._obj?getCanPin()
        
        static member getIsPinned (column : Column<'T>) : ColumnPinningPosition =
            let isPinned : string = column._obj?getIsPinned()
            let state =
                match isPinned with
                | "left" -> Left
                | "right" -> Right
                | _ -> Neither
            state
            
        static member getIsResizing (column : Column<'T>) : bool =
            column._obj?getIsResizing()
    
        static member getSize (column : Column<'T>) : int =
            column._obj?getSize()
            
        static member getCanFilter (column : Column<'T>) : bool =
            column._obj?getCanFilter()
            
        static member getFilterValue (column : Column<'T>) : 'T2 option =
            let filterValue = column._obj?getFilterValue()
            if nullOrUndefined filterValue then None else Some filterValue
            
        static member setFilterValue (value : 'T2 option -> 'T2) (column : Column<'T>) : Column<'T> =
            column._obj?setFilterValue(fun x ->
                if (isNullOrUndefined x) then value None
                else value (Some x))
            column
            
    type Table =
        static member setColumnFilter (value : 'TValue) (column : Column<'T>) : Column<'T> =
            column._obj?setFilterValue(value)
            column
        
        static member pinColumn (position : ColumnPinningPosition) (column : Column<'T>) (table : Table<'T>) : Table<'T> =
            let positionStr =
                match position with
                | Left -> "left"
                | Right -> "right"
                | _ -> "false"
            column._obj?pin(positionStr)
            
            table
        