namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Core

module Table =
    let setColumnVisibility (column: string) (isVisible: bool) (table : Table<'T>) : Table<'T> =
        updateRecordStateProperty
            (fun s -> s?columnVisibility)
            (fun s n -> s?columnVisibility <- n)
            column
            isVisible
            table
    