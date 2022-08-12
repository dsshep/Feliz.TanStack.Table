namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module TableState = 
    type Table = 
        static member getState (table : Table<'T>) : TableState<'T> =
            let dynamicState = table?_obj?getState()
            dynamicState
        
        static member setData (data : 'T[]) (table : Table<'T>) : Table<'T> =
            table?_obj?options?data <- data
            let table =
                wrapObj (table?_obj)
                |> merge (createObj [ "Data" ==> data ])
            
            table
        
        static member setPaginationState (paginationState : PaginationState) (table : Table<'T>) : Table<'T> =
            let o = createObj [ "paginationState" ==> paginationState ]
            table?_obj?setState(o)
            table
            