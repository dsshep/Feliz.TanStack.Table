namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Core

[<AutoOpen>]
module Ordering =
    
    type Table =
        
        static member setColumnOrder (columnOrder: string[]) (table : Table<'T>) : Table<'T> =
            table._obj?setOptions(fun prev ->
                prev?state?columnOrder <- columnOrder
                setStateChange prev table._obj?options (fun () -> ()))
            
            table

        static member resetColumnOrder (defaultState: bool) (table : Table<'T>) : Table<'T> =
            table._obj?resetColumnOrder(defaultState)
            table