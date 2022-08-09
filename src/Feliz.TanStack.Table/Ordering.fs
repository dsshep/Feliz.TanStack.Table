namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Core

[<AutoOpen>]
module Ordering =
    
    type Table =
        
        static member setColumnOrder (columnOrder: string[]) (table : Table<'T>) : Table<'T> =
            table?_obj?setColumnOrder(columnOrder)
            table

        static member resetColumnOrder (table : Table<'T>) : Table<'T> =
            table?_obj?resetColumnOrder()
            table
            