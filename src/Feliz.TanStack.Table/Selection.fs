namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Selection = 

    type Table =
        static member setRowSelection (updater : Record<bool> -> Record<bool>) (table : Table<'T>) : Table<'T> =
            table?_obj?setRowSelection(updater)
            table
        
        static member resetRowSelection (defaultValue : bool) (table : Table<'T>) : bool =
            table?_obj?resetRowSelection(defaultValue)
        
        static member getIsAllRowsSelected (table : Table<'T>) : bool =
            table?_obj?getIsAllRowsSelected()
            
        static member getIsAllPageRowsSelected (table : Table<'T>) : bool =
            table?_obj?getIsAllPageRowsSelected()   
            
        static member getIsSomeRowsSelected (table : Table<'T>) : bool =
            table?_obj?getIsSomeRowsSelected()
            
        static member getIsSomePageRowsSelected (table : Table<'T>) : bool =
            table?_obj?getIsSomePageRowsSelected()
            
        static member getToggleAllRowsSelectedHandler (table : Table<'T>) : Table<'T> =
            table?_obj?getToggleAllRowsSelectedHandler()
            table
        
        static member toggleAllRowsSelected (table : Table<'T>) : Table<'T> =
            let selected = Table.getIsAllRowsSelected table
            table?_obj?toggleAllRowsSelected(not selected)
            table
            
        static member getPreSelectedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getPreSelectedRowModel()
            
        static member getSelectedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getSelectedRowModel()
            
        static member getFilteredSelectedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getFilteredSelectedRowModel()
            
        static member getGroupedSelectedRowModel (table : Table<'T>) : RowModel<'T> =
            table?_obj?getGroupedSelectedRowModel()
    
    type Row =
        static member getCanSelect (row : Row<'T>) : bool =
            row?getCanSelect()
        
        static member getCanMultiSelect (row : Row<'T>) : bool =
            row?getCanMultiSelect()
        
        static member getCanSelectSubRows (row : Row<'T>) : bool =
            row?getCanSelectSubRows()
        
        static member getIsSelected (row : Row<'T>) : bool =
            row?getIsSelected()
        
        static member getIsSomeSelected (row : Row<'T>) : bool =
            row?getIsSomeSelected()
        
        static member toggleSelected (row : Row<'T>) : unit =
            if Row.getCanSelect row then 
                let isSelected = Row.getIsSelected row
                row?toggleSelected(not isSelected)
                
            