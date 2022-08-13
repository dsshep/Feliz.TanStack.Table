namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Header =

    type Header = 
        static member getLeafHeaders (header : Header<'T>) : Header<'T>[] =
            header?getLeafHeaders()
            
        static member isPlaceholder (header : Header<'T>) : bool =
            header?isPlaceholder
            