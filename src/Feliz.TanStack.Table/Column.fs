namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

type ColumnPinningPosition =
    | Left
    | Right
    | Neither
    with
    member this.asString() =
        match this with
        | Left -> "left"
        | Right -> "right"
        | _ -> "false"

[<AutoOpen>]
module Column = 

    type Column =
        static member getCanPin (column : Column<'T>) : bool =
            column?getCanPin()
        
        static member getIsPinned (column : Column<'T>) : ColumnPinningPosition =
            let isPinned : string = column?getIsPinned()
            let state =
                match isPinned with
                | "left" -> Left
                | "right" -> Right
                | _ -> Neither
            state
            
    type Table =
        static member setColumnFilter (value : 'TValue) (column : Column<'T>) : Column<'T> =
            column?setFilterValue(value)
            column
        
        static member pinColumn (position : ColumnPinningPosition) (column : Column<'T>) (table : Table<'T>) : Table<'T> =
            let positionStr = position.asString()
            column?pin(positionStr)
            
            table
        