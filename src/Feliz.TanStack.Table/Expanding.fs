namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Expanding = 
    type Table =
        static member setExpanded (updater : Record<bool> -> Record<bool>) (table : Table<'T>) : Table<'T> =
            table?_obj?setExpanded(updater)
            table
        
        static member toggleAllRowsExpanded (table : Table<'T>) : Table<'T> =
            table?_obj?toggleAllRowsExpanded()
            table
            
        static member resetExpanded (defaultValue : bool) (table : Table<'T>) : Table<'T> =
            table?_obj?resetExpanded(defaultValue)
            table
            
        static member getCanSomeRowsExpand (table : Table<'T>) : bool =
            table?_obj?getCanSomeRowsExpand()
            
        static member getIsSomeRowsExpanded (table : Table<'T>) : bool =
            table?_obj?getIsSomeRowsExpanded()
            
        static member getToggleAllRowsExpandedHandler (table : Table<'T>) : bool =
            table?_obj?getToggleAllRowsExpandedHandler()
            
        static member getIsAllRowsExpanded (table : Table<'T>) : bool =
            table?_obj?getIsAllRowsExpanded()
    
        static member getExpandedDepth (table : Table<'T>) : int =
            table?_obj?getExpandedDepth()
            
        static member getExpandedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getExpandedRowModel()
            
        static member getPreExpandedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getPreExpandedRowModel()
    
    type Row =
        
        static member getCanExpand (row : Row<'T>) : bool =
            row?getCanExpand()
            
        static member getIsExpanded (row : Row<'T>) : bool =
            row?getIsExpanded()
            
        static member toggleExpanded(row : Row<'T>) : unit =
            if Row.getCanExpand row then
                row?toggleExpanded()
            