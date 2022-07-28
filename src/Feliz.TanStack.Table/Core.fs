namespace Feliz.TanStack.Table

open Fable.Core
open Fable.Core.JsInterop

module internal Core =
    
    [<Emit("{ ...$0, ...$1 }")>]
    let internal merge a b = jsNative
    
    [<Emit("{ ...$0, ...$1, onStateChange: $2 }")>]
    let internal setStateChange prev options stateChange = jsNative
    
    let internal updateRecordStateProperty
        (getFn: obj -> obj)
        (setFn: obj -> obj -> unit)
        (key : string)
        (newVal : obj)
        (table : Table<'T>) : Table<'T> =
            let r = createObj [ key, newVal ]
            
            table._obj?setOptions(fun prev ->
                let newState = merge (getFn prev?state) r
                setFn prev?state newState
                setStateChange prev table._obj?options (fun () -> ()))
            
            table
        