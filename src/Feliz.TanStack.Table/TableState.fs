namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
//
//StartOffset: int
//StartSize: int
//DeltaOffset: int
//DeltaPercentage: int
//IsResizingColumn: string
//ColumnSizingStart: (string * int)[]

[<AutoOpen>]
module TableState = 
    type Table = 
        static member getState (table : Table<'T>) : TableState<'T> =
            let getColumnSizingInfo (o : obj) : ColumnSizingInfoState =
                { StartOffset = o?startOffset
                  StartSize = o?startSize
                  DeltaOffset = o?deltaOffset
                  DeltaPercentage = o?deltaPercentage
                  IsResizingColumn = o?isResizingColumn
                  ColumnSizingStart = o?columnSizingStart }
            
            let dynamicState = table._obj?getState()
            
            { _obj = dynamicState
              ColumnSizingInfo = getColumnSizingInfo (dynamicState?columnSizingInfo) }
            