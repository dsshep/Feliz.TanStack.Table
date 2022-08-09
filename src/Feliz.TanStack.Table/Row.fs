namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Feliz.TanStack.Table.Types

[<AutoOpen>]
module Row = 
    type Row = 
        static member getValue (column : Column<'T>) (row : Row<'T>) : obj =
            row?getValue(column.id)
            
        static member getCanExpand (row : Row<'T>) : bool =
            row?getCanExpand()
            
        static member getIsExpanded (row: Row<'T>) : bool =
            row?getIsExpanded()
