namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Core

[<AutoOpen>]
module Visibility = 
    type Table =
        static member getVisibleLeafColumns (table : Table<'T>) : Column<'T>[] =
            table?_obj?getVisibleLeafColumns()
            
        static member getLeftVisibleLeafColumns (table : Table<'T>) : Column<'T>[] =
            table?_obj?getLeftVisibleLeafColumns()
            
        static member getRightVisibleLeafColumns (table : Table<'T>) : Column<'T>[] =
            table?_obj?getRightVisibleLeafColumns()
            
        static member getCenterVisibleLeafColumns (table : Table<'T>) : Column<'T>[] =
            table?_obj?getCenterVisibleLeafColumns()
        
        static member setColumnVisibility (column : Column<'T>) (isVisible : bool) (table : Table<'T>) : Table<'T> =
            let r = createObj [ column.id, isVisible ]
            table?_obj?setColumnVisibility(r)
            table
        
        static member resetColumnVisibility (defaultState : bool) (table : Table<'T>) : Table<'T> =
            table?_obj?resetColumnVisibility(defaultState)
            table
            
        static member toggleAllColumnsVisible (value : bool) (table : Table<'T>) =
            table?_obj?toggleAllColumnsVisible(value)
            table
            
        static member getIsAllColumnsVisible (table : Table<'T>) : bool =
            table?_obj?getIsAllColumnsVisible()
            
        static member getIsSomeColumnsVisible (table : Table<'T>) : bool =
            table?_obj?getIsSomeColumnsVisible()
            