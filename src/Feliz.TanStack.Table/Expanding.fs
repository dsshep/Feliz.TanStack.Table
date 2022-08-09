namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Expanding = 
    type Table =
        static member getToggleAllRowsExpandedHandler (table : Table<'T>) : bool =
            table?_obj?getToggleAllRowsExpandedHandler()
            
        static member getIsAllRowsExpanded (table : Table<'T>) : bool =
            table?_obj?getIsAllRowsExpanded()
    
        static member getIsAllRowsSelected (table : Table<'T>) : bool =
            table?_obj?getIsAllRowsSelected()
            
        static member getIsSomeRowsSelected (table : Table<'T>) : bool =
            table?_obj?getIsSomeRowsSelected()
            
        static member getToggleAllRowsSelectedHandler (table : Table<'T>) =
            table?_obj?getToggleAllRowsSelectedHandler()