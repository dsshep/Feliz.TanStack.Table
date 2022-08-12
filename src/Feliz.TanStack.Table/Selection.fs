namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Selection = 

    type Table =
        static member getIsAllRowsSelected (table : Table<'T>) : bool =
            table?_obj?getIsAllRowsSelected()
            
        static member getIsSomeRowsSelected (table : Table<'T>) : bool =
            table?_obj?getIsSomeRowsSelected()
            
        static member getToggleAllRowsSelectedHandler (table : Table<'T>) : Table<'T> =
            table?_obj?getToggleAllRowsSelectedHandler()
            table
        
        static member toggleAllRowsSelected (table : Table<'T>) : Table<'T> =
            let selected = Table.getIsAllRowsSelected table
            table?_obj?toggleAllRowsSelected(not selected)
            table
    
    type Row =
        static member getCanSelect (row : Row<'T>) : bool =
            row?getCanSelect()
        
        static member getIsSelected (row : Row<'T>) : bool =
            row?getIsSelected()
        
        static member getIsSomeSelected (row : Row<'T>) : bool =
            row?getIsSomeSelected()
        
        static member toggleSelected (row : Row<'T>) : unit =
            if Row.getCanSelect row then 
                let isSelected = Row.getIsSelected row
                row?toggleSelected(not isSelected)
                
            