namespace Feliz.TanStack.Table

open Fable.Core
open Fable.Core.JsInterop

[<AutoOpen>]
module ColumnDef =
    [<Emit("{ ...$0, _obj: $0, Data: $0.options.data }")>]
    let private wrapTable table = jsNative
    
    let rec internal nativeColumnDefs (columnDefs: ColumnDefOptionProp<'T> list list) =
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
                    | AggregationFnOption f -> "aggregationFn" ==> f
                    | EnableGrouping b -> "enableGrouping" ==> b
                    | AggregatedCell f -> "aggregatedCell" ==> f
                    | Size i -> "size" ==> i
                    | MinSize i -> "minSize" ==> i
                    | MaxSize i -> "maxSize" ==> i
                    | EnableHiding hiding -> "enableHiding" ==> hiding
                    | SortingFn o -> "sortingFn" ==> o
                    | SortDescFirst b -> "sortDescFirst" ==> b
                    | EnableSorting b -> "enableSorting" ==> b
                    | EnableMultiSort b -> "enableMultiSort" ==> b
                    | InvertSorting b -> "invertSorting" ==> b
                    | SortUndefined o -> "sortUndefined" ==> o
            ])
        |> Seq.toArray
        
    type columnDef =
        static member id s = Id s
        static member accessorKey s = AccessorKey s
        static member accessorFn fn = AccessorFn fn
        static member header s = HeaderStr s
        static member header<'T1, 'T2, 'State, 'Msg> (fn: HeaderProps<'T1, 'State, 'Msg> -> 'T2) : ColumnDefOptionProp<'T1> =
            (HeaderFn (fun props ->
                let table = wrapTable props?table
                props?table <- table
                fn props))
            
        static member footer s = FooterStr s
        static member footer<'T1, 'T2, 'TState, 'Msg> (fn: HeaderProps<'T1, 'TState, 'Msg> -> 'T2) : ColumnDefOptionProp<'T1> =
            (FooterFn fn) 
        static member cell<'T, 'State, 'Msg, 'T2> (fn: CellContextProp<'T, 'State, 'Msg> -> 'T2) : ColumnDefOptionProp<'T> =
            Cell (fun props ->
                let table = wrapTable props?table
                props?table <- table
                fn props)
            
        static member aggregationFn<'T> (fn : AggregationFn<'T>) : ColumnDefOptionProp<'T> =
            AggregationFnOption fn
        static member aggregationFn<'T> (aggregation : Aggregation) : ColumnDefOptionProp<'T> =
            AggregationFnOption (aggregation.asString())
        static member aggregatedCell<'T, 'State, 'Msg, 'T2> (fn: CellContextProp<'T, 'State, 'Msg> -> 'T2) : ColumnDefOptionProp<'T> =
            AggregatedCell (fun props ->
                let table = wrapTable props?table
                props?table <- table
                fn props)
        static member enableGrouping (enableGrouping : bool) = EnableGrouping enableGrouping
        static member columns columnDef = Columns columnDef
        static member size size = Size size
        static member minSize minSize = MinSize minSize
        static member maxSize maxSize = MaxSize maxSize
        static member enableHiding hiding = EnableHiding hiding
        static member sortingFn<'T> (fn : SortingFn<'T>) : ColumnDefOptionProp<'T> = SortingFn fn
        static member sortingFn<'T> (sortingFnKey : string) : ColumnDefOptionProp<'T> = SortingFn sortingFnKey
        static member sortDescFirst (sortDescFirst : bool) = SortDescFirst sortDescFirst
        static member enableSorting (enableSorting : bool) = EnableSorting enableSorting
        static member enableMultiSort (enableMultiSort : bool) = EnableMultiSort enableMultiSort
        static member invertSorting (invertSorting : bool) = InvertSorting invertSorting
        static member sortUndefined (sortUndefined : int) =
            match sortUndefined with
            | 1 | -1 -> ()
            | _ -> failwithf "sortUndefined must be either -1 or 1"
            SortUndefined sortUndefined
    
    type ColumnHelper =
        static member accessor (accessor: string, columnDefs: ColumnDefOptionProp<_> list) =
            columnDef.accessorKey accessor :: columnDefs
        static member accessor (accessorFn: 'T -> string, columnDefs: ColumnDefOptionProp<_> list) =
            columnDef.accessorFn accessorFn :: columnDefs
        static member createColumnHelper<'T> (columnDefs: ColumnDefOptionProp<'T> list list) =
            columnDefs
        