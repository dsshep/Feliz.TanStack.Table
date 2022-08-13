namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Visibility =
    type Column =
        static member getCanHide (column : Column<'T>) : bool =
            column?getCanHide()
            
        static member getIsVisible (column : Column<'T>) : bool =
            column?getIsVisible()
            
        static member toggleVisibility (column : Column<'T>) : unit =
            column?toggleVisibility()
    
    type Table =
        static member getVisibleFlatColumns (table : Table<'T>) : Column<'T>[] =
            table?_obj?getVisibleFlatColumns()
        
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
            