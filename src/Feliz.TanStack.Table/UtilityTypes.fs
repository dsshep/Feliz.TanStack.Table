namespace Feliz.TanStack.Table

[<AutoOpen>]
module UtilityTypes =
    
    type SortingFns<'T> =
        abstract member alphanumeric : rowA: Row<'T> * rowB: Row<'T> * columnId: string -> int with get
        abstract member alphanumericCaseSensitive: Row<'T> * rowB: Row<'T> * columnId: string -> int with get
        abstract member text: Row<'T> * rowB: Row<'T> * columnId: string -> int with get
        abstract member textCaseSensitive: Row<'T> * rowB: Row<'T> * columnId: string -> int with get
        abstract member datetime: Row<'T> * rowB: Row<'T> * columnId: string -> int with get
        abstract member basic: Row<'T> * rowB: Row<'T> * columnId: string -> int with get
    