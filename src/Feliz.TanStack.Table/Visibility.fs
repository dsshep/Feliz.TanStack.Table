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
        
        static member setColumnVisibility (column: string) (isVisible: bool) (table : Table<'T>) : Table<'T> =
            updateRecordStateProperty
                (fun s -> s?columnVisibility)
                (fun s n -> s?columnVisibility <- n)
                column
                isVisible
                table
        
        static member resetColumnVisibility (defaultState : bool) (table : Table<'T>) : Table<'T> =
            updateStateFunctionCall (fun t -> t?resetColumnVisibility(defaultState)) table
            
        static member toggleAllColumnsVisible (value : bool) (table : Table<'T>) =
            updateStateFunctionCall (fun t -> t?toggleAllColumnsVisible(value)) table
            
        static member getIsAllColumnsVisible (table : Table<'T>) : bool =
            table._obj?getIsAllColumnsVisible()
            
        static member getIsSomeColumnsVisible (table : Table<'T>) : bool =
            table._obj?getIsSomeColumnsVisible()
            
        static member getToggleAllColumnsVisibilityHandler (table : Table<'T>) : 'TEvent -> unit =
            table?getToggleAllColumnsVisibilityHandler()