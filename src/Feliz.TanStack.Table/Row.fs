namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Feliz.TanStack.Table.Types

[<AutoOpen>]
module Row = 
    type Row = 
        static member getValue (column : Column<'T>) (row : Row<'T>) : obj =
            row._obj?getValue(column.Id)
            
        static member getCanExpand (row : Row<'T>) : bool =
            row._obj?getCanExpand()
            
        static member getIsExpanded (row: Row<'T>) : bool =
            row._obj?getIsExpanded()
