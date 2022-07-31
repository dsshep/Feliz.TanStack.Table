namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

type Column =
    
    static member getIsVisible (column : Column<'T>) : bool =
        column._obj?getIsVisible()
