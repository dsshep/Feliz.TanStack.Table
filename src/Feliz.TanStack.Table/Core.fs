namespace Feliz.TanStack.Table

open Fable.Core
open Fable.Core.JsInterop

module internal Core =
    
    [<Emit("{ ...$0, ...$1 }")>]
    let internal merge a b = jsNative
    
    [<Emit("{ ...$0, onStateChange: $1 }")>]
    let internal setStateChange2 x stateChange = jsNative
    
    [<Emit("$0 == null")>]
    let internal nullOrUndefined x : bool = jsNative
    