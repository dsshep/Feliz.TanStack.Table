namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Header =

    type Header = 
        static member getSize (header : Header<'T>) : int = 
            header?getSize()

        static member getLeafHeaders (header : Header<'T>) : Header<'T>[] =
            header?getLeafHeaders()
            