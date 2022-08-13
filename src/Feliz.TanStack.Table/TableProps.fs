namespace Feliz.TanStack.Table

open Feliz
open Fable.Core.JsInterop

[<AutoOpen>]
module TableProps =
    let internal getCoreRowModel: unit -> obj = import "getCoreRowModel" "@tanstack/table-core"
    let private getFilteredRowModel : unit -> obj = import "getFilteredRowModel" "@tanstack/table-core"
    let private getPaginationRowModel : unit -> obj = import "getPaginationRowModel" "@tanstack/table-core"
    let private getExpandedRowModel : unit -> obj = import "getExpandedRowModel" "@tanstack/table-core"
    let private getGroupedRowModel : unit -> obj = import "getGroupedRowModel" "@tanstack/table-core"
    let private getSortedRowModel : unit -> obj = import "getSortedRowModel" "@tanstack/table-core"
    
    let rec private nativeColumnDefs (columnDefs: ColumnDefOptionProp<'T> list list) =
        columnDefs
        |> Seq.map (fun colDef ->
            createObj [
                for option in colDef do
                    match option with
                    | Id s -> "id" ==> s
                    | AccessorKey s -> "accessorKey" ==> s
                    | AccessorFn f -> "accessorFn" ==> f
                    | HeaderStr s -> "header" ==> s
                    | FooterStr s -> "footer" ==> s
                    | HeaderFn f -> "header" ==> f
                    | FooterFn f -> "footer" ==> f
                    | Cell f -> "cell" ==> f
                    | Columns def -> "columns" ==> (nativeColumnDefs def)
                    | AggregationFn f -> "aggregationFn" ==> f
                    | AggregatedCell f -> "aggregatedCell" ==> f
                    | Size i -> "size" ==> i
                    | MinSize i -> "minSize" ==> i
                    | MaxSize i -> "maxSize" ==> i
                    | EnableHiding hiding -> "enableHiding" ==> hiding
            ])
        |> Seq.toArray

    type TableStateProps<'T> =
        static member inline columnOrder (columnOrderState: string[]) =
            prop.custom("columnOrder", columnOrderState)
        static member inline init (props: IReactProperty list) =
            (createObj !!props) :?> 'T
    
    type tableProps =
        static member inline data<'T> (data: 'T[]) =
            prop.custom ("data", data)
        static member columns<'T> (columns: ColumnDefOptionProp<'T> list list) =
            prop.custom ("columns", (nativeColumnDefs columns))
        static member getSubRows<'T, 'T2> (fn : 'T -> 'T2) =
            prop.custom ("getSubRows", fn)
        static member inline onColumnOrderChange (fn: string[] -> string[]) =
            prop.custom ("onColumnOrderChange", fn)
        static member inline onColumnOrderChange (fn: string[] -> unit) =
            prop.custom ("onColumnOrderChange", fn)
        static member inline columnOrder (order: string[]) =
            prop.custom ("columnOrder", order)
        static member inline columnResizeMode (resizeMode : ColumnResizeMode) =
            prop.custom ("columnResizeMode", (ColumnResizeMode.toString resizeMode))
        static member inline enableColumnResizing (enable : bool) =
            prop.custom ("enableColumnResizing", enable)
        static member inline autoResetPageIndex (autoReset : bool) =
            prop.custom ("autoResetPageIndex", autoReset)
        static member inline manualPagination (manual : bool) =
            prop.custom("manualPagination", manual)
        static member inline pageCount (count : int) =
            prop.custom("pageCount", count)
        static member inline size (size : int) =
            prop.custom("size", size)
        static member inline maxSize (maxSize : int) =
            prop.custom("maxSize", maxSize)
        static member inline minSize (minSize : int) =
            prop.custom("minSize", minSize)
        static member onColumnSizingChange (onColumnSizingChange: Record<int> -> unit) =
            prop.custom("onColumnSizingChange", onColumnSizingChange)
        static member onColumnSizingInfoChange (onColumnSizingInfoChange: ColumnSizingInfoState -> unit) =
            prop.custom("onColumnSizingInfoChange", onColumnSizingInfoChange)
        static member onColumnVisibilityChange (onColumnVisibilityChange: Record<bool> -> unit) =
            prop.custom("onColumnVisibilityChange", onColumnVisibilityChange)
        static member enableHiding (hiding : bool) =
            prop.custom("enableHiding", hiding)
        
            
        // Row Models
        static member filteredRowModel() =
            prop.custom ("getFilteredRowModel", getFilteredRowModel())
        static member paginationRowModel() =
            prop.custom ("getPaginationRowModel", getPaginationRowModel())
        static member expandedRowModel() =
            prop.custom ("getExpandedRowModel", getExpandedRowModel())
        static member groupedRowModel() =
            prop.custom ("getGroupedRowModel", getGroupedRowModel())
        static member sortedRowModel() =
            prop.custom ("getSortedRowModel", getSortedRowModel())
            
        // debug
        static member debugAll() =
            prop.custom("debugAll", true)
        static member debugTable() =
            prop.custom ("debugTable", true)
    

