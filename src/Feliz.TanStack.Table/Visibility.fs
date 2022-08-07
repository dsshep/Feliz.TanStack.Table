namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Core

[<AutoOpen>]
module Visibility = 
    type Table =
        static member getVisibleLeafColumns (table : Table<'T>) : Column<'T>[] =
            let dynamicColumns = table._obj?getVisibleLeafColumns()
            dynamicColumns |> Array.choose Table.getColumn
            
        static member getLeftVisibleLeafColumns (table : Table<'T>) : Column<'T>[] =
            let dynamicColumns = table._obj?getLeftVisibleLeafColumns()
            dynamicColumns |> Array.choose Table.getColumn
            
        static member getRightVisibleLeafColumns (table : Table<'T>) : Column<'T>[] =
            let dynamicColumns = table._obj?getRightVisibleLeafColumns()
            dynamicColumns |> Array.choose Table.getColumn
            
        static member getCenterVisibleLeafColumns (table : Table<'T>) : Column<'T>[] =
            let dynamicColumns = table._obj?getCenterVisibleLeafColumns()
            dynamicColumns |> Array.choose Table.getColumn
        
        static member setColumnVisibility (column : Column<'T>) (isVisible : bool) (table : Table<'T>) : Table<'T> =
            let r = createObj [ column.Id, isVisible ]
            table._obj?setColumnVisibility(r)
            table
        
        static member resetColumnVisibility (defaultState : bool) (table : Table<'T>) : Table<'T> =
            table._obj?resetColumnVisibility(defaultState)
            table
            
        static member toggleAllColumnsVisible (value : bool) (table : Table<'T>) =
            table._obj?toggleAllColumnsVisible(value)
            table
            
        static member getIsAllColumnsVisible (table : Table<'T>) : bool =
            table._obj?getIsAllColumnsVisible()
            
        static member getIsSomeColumnsVisible (table : Table<'T>) : bool =
            table._obj?getIsSomeColumnsVisible()
            