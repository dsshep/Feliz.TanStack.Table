namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Expanding = 
    type Table =
        static member getToggleAllRowsExpandedHandler (table : Table<'T>) : bool =
            table?_obj?getToggleAllRowsExpandedHandler()
            
        static member getIsAllRowsExpanded (table : Table<'T>) : bool =
            table?_obj?getIsAllRowsExpanded()
    
        static member toggleAllRowsExpanded (table : Table<'T>) : Table<'T> =
            table?_obj?toggleAllRowsExpanded()
            table
    
    type Row =
        static member getCanExpand (row : Row<'T>) : bool =
            row?getCanExpand()
            
        static member getIsExpanded (row : Row<'T>) : bool =
            row?getIsExpanded()
            
        static member toggleExpanded(row : Row<'T>) : unit =
            if Row.getCanExpand row then
                row?toggleExpanded()
            