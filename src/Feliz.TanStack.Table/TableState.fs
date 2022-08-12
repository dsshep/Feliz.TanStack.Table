namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module TableState =
    let private setState (name : string) (s : obj) (table : Table<'T>) : Table<'T> =
        let o = createObj [ "pagination" ==> s ]
        table?_obj?setState(o)
        table
    
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
        
        static member setColumnVisibilityState (visibility : VisibilityState) (table : Table<'T>) : Table<'T> =
            setState "columnVisibility" visibility table
        
        static member setColumnOrderState (order : string[]) (table : Table<'T>) : Table<'T> =
            setState "columnOrder" order table
        
        static member setColumnPinningState (pinning : ColumnPinningState) (table : Table<'T>) : Table<'T> =
            setState "columnPinning" pinning table
        
        static member setColumnFiltersState (filters : ColumnFilter[]) (table : Table<'T>) : Table<'T> =
            setState "columnFilters" filters table
        
        static member setColumnSortingState (sorting : ColumnSort[]) (table : Table<'T>) : Table<'T> =
            setState "columnSorting" sorting table
        
        static member setExpandedState (expanded : ExpandedState) (table : Table<'T>) : Table<'T> =
            setState "expanded" expanded table
        
        static member setColumnGroupingState (grouping : string[]) (table : Table<'T>) : Table<'T> =
            setState "grouping" grouping table
        
        static member setColumnSizingState (sizing : (string * int) seq) (table : Table<'T>) : Table<'T> =
            setState "columnSizing" (createObj (sizing |> Seq.map (fun (f, s) -> (f, box s)))) table
        
        static member setColumnSizingInfoState (sizingInfo : ColumnSizingInfoState) (table : Table<'T>) : Table<'T> =
            setState "columnSizingInfo" sizingInfo table
        
        static member setPaginationState (paginationState : PaginationState) (table : Table<'T>) : Table<'T> =
            setState "pagination" paginationState table
            
        static member setRowSelectionState (rowSelection : (string * bool) seq) (table : Table<'T>) : Table<'T> =
            setState "rowSelection" (createObj (rowSelection |> Seq.map (fun (f, s) -> (f, box s)))) table
            