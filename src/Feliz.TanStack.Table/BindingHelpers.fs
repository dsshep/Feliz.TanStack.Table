namespace Feliz.TanStack.Table

open Fable.Core
open Fable.Core.JsInterop

[<AutoOpen>]
module internal BindingHelpers =
    
    [<Emit("{ ...$0, ...$1 }")>]
    let merge a b = jsNative
    
    [<Emit("{ ...$0, onStateChange: $1 }")>]
    let setStateChange x stateChange = jsNative
    
    [<Emit("$0 == null")>]
    let nullOrUndefined x : bool = jsNative
    
    [<Emit("(typeof $0 === 'function')")>]
    let isJsFunc o = jsNative
    
    