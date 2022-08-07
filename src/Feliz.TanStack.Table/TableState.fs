namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module TableState = 
    type Table = 
        static member getState (table : Table<'T>) : TableState<'T> =
            let dynamicState = table._obj?getState()
            dynamicState
            