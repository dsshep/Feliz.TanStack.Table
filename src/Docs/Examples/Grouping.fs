module Examples.Grouping

open Fable.Core.JsInterop
open Elmish
open Browser.Types
open Feliz
open Feliz.UseElmish
open Feliz.TanStack.Table
open MakeData
open MakeData.Types

let defaultColumns : ColumnDefOptionProp<Person> list list = [
    [ columnDef.header "Name"
      columnDef.columns [
          [ columnDef.accessorKey "Firstname"
            columnDef.header "First Name"
            columnDef.cell (fun info -> info.getValue<_>()) ]
          [ columnDef.accessorKey "Lastname"
            columnDef.id "Lastname"
            columnDef.cell (fun info -> info.getValue<_>())
            columnDef.header (fun _ -> Html.span [ prop.text "Last Name" ]) ]
      ] ]
    [ columnDef.header "info"
      columnDef.columns [
          [ columnDef.accessorKey "Age"
            columnDef.header (fun _ -> "Age")
            columnDef.aggregatedCell (fun props ->
                let x = ((props.getValue<float>() * 100.) / 100.)
                Fable.Core.JS.debugger()
                ((props.getValue<float>() * 100.) / 100.) |> round)
            columnDef.aggregationFn Aggregation.Median ]
          [ columnDef.header "More Info"
            columnDef.columns [
                [ columnDef.accessorKey "Visits"
                  columnDef.header (fun _ -> Html.span [ prop.text "Visits" ])
                  columnDef.aggregationFn Aggregation.Sum ]
                [ columnDef.accessorKey "Status"
                  columnDef.header "Status" ]
                [ columnDef.accessorKey "Progress"
                  columnDef.header "Profile Progress"
                  columnDef.cell (fun props ->
                      $"{((props.getValue<float>() * 100.) / 100.) |> round}%%")
                  columnDef.aggregationFn Aggregation.Mean
                  columnDef.aggregatedCell (fun props ->
                      $"{((props.getValue<float>() * 100.) / 100.) |> round}%%") ]
            ] ]
      ] ]
]

type PaginateChange =
    | First
    | Previous
    | Next
    | Last
    | Index of int

type State = {
    Table : Table<Person>
}


type Msg =
    | ColumnClicked of Column<Person>
    | Paginate of PaginateChange
    | PageSizeChange of int
    | ExpandClicked of Row<Person>
    
let init () =
    let data = MakeData.make 10_000 // |> Array.collect (fun p -> [| p; p; p; p; |])
    
    let tableProps = [
        tableProps.data data
        tableProps.columns defaultColumns
        tableProps.expandedRowModel()
        tableProps.filteredRowModel()
        tableProps.groupedRowModel()
        tableProps.paginationRowModel() ]
    
    Fable.Core.JS.debugger()
    let table = Table.init<Person> tableProps
    
    { Table = table }, Cmd.none

let update (msg: Msg) (state: State) =
    match msg with
    | ColumnClicked column ->
        Column.toggleGrouping column
        
        { state with Table = state.Table }, Cmd.none
    
    | Paginate paginateChange ->
        let table = 
            match paginateChange with
            | First -> Table.setPageIndex 0 state.Table
            | Previous -> Table.previousPage state.Table
            | Next -> Table.nextPage state.Table
            | Last -> Table.setPageIndex (Table.getPageCount state.Table - 1) state.Table
            | Index i -> Table.setPageIndex i state.Table
            
        { state with Table = table }, Cmd.none
        
    | PageSizeChange pageSize ->
        { state with Table = Table.setPageSize pageSize state.Table }, Cmd.none
        
    | ExpandClicked row ->
        Row.toggleExpanded row
        { state with Table = state.Table }, Cmd.none
        
    
let view (state: State) (dispatch: Msg -> unit) =
    Fable.Core.JS.debugger()
    let table = 
        let thead =
            Html.thead [
                for headerGroup in Table.getHeaderGroups state.Table do
                     Html.tr [
                         prop.key headerGroup.id
                         prop.children [
                             for header in headerGroup.headers do
                                 Html.th [
                                     prop.key header.id
                                     prop.colSpan header.colSpan
                                     prop.children [
                                         if header.isPlaceholder then Html.none
                                         else
                                         Html.div [
                                             if Column.getCanGroup header.column then
                                                 Html.button [
                                                     prop.className [ Bulma.Button; Bulma.IsSmall; Bulma.IsGhost ]
                                                     prop.style [ style.cursor.pointer ]
                                                     prop.onClick (fun _ -> (ColumnClicked header.column) |> dispatch)
                                                     prop.text (if Column.getIsGrouped header.column then
                                                                    $"🛑{Column.getGroupedIndex header.column}"
                                                                else "👊")
                                                 ]
                                             Html.flexRender (
                                                 header.column.columnDef.header,
                                                 Table.getContext header)
                                         ]
                                     ]
                                 ]
                         ]
                     ]
            ]
            
        let tbody =
            Html.tbody [
                for row in (Table.getRowModel state.Table).rows do
                    Html.tr [
                        prop.key row.id
                        prop.children [
                            for cell in Table.getVisibleCells row do
                                Html.td [
                                    prop.style [
                                        style.backgroundColor
                                            (if Cell.getIsGrouped cell then "#0aff0082"
                                             elif Cell.getIsAggregated cell then "#ffa50078"
                                             elif Cell.getIsPlaceholder cell then "#ff000042"
                                             else "white")
                                    ]
                                    prop.children [
                                        if Cell.getIsGrouped cell then
                                            Html.span [
                                                Html.button [
                                                    prop.className [ Bulma.Button; Bulma.IsSmall; Bulma.IsGhost ]
                                                    prop.onClick (fun _ -> (ExpandClicked row) |> dispatch)
                                                    prop.children [
                                                        Html.flexRender(
                                                            cell.column.columnDef.cell,
                                                            Table.getContext cell)
                                                        Html.text (if Row.getIsExpanded row then "👇" else "👉")
                                                        Html.text $" {row.subRows.Length}"
                                                    ]
                                                ]
                                            ]
                                        elif Cell.getIsAggregated cell then
                                            Html.flexRender(
                                                (if cell.column.columnDef.aggregatedCell <> null then
                                                    cell.column.columnDef.aggregatedCell
                                                 else cell.column.columnDef.cell), 
                                                Table.getContext cell)
                                        elif Cell.getIsPlaceholder cell then Html.none
                                        else
                                            Html.flexRender(
                                                cell.column.columnDef.cell,
                                                Table.getContext cell)
                                    ]
                                ]
                        ]
                    ]
            ]
            
        let paginationControls =
            let selectOptions = Map.ofList [
                for i in 10..10..50 do
                    string i, ($"Show {i}", fun _ -> PageSizeChange i |> dispatch)
            ]
            
            let pageControl (text : string) isDisabled onClick =
                Html.button [
                    prop.className [ Bulma.Button ]
                    prop.text text
                    prop.disabled isDisabled
                    prop.onClick onClick
                ]
            
            Html.div [
                prop.className [ Bulma.IsInlineBlock ]
                prop.children [
                    pageControl "<<" (Table.hasPreviousPage state.Table |> not) (fun _ -> Paginate First |> dispatch)
                    pageControl "<" (Table.hasPreviousPage state.Table |> not) (fun _ -> Paginate First |> dispatch)
                    pageControl ">" (Table.hasNextPage state.Table |> not) (fun _ -> Paginate Next |> dispatch)
                    pageControl ">>" (Table.hasNextPage state.Table |> not) (fun _ -> Paginate Last |> dispatch)
                    Html.p [
                        Html.text "Page "
                        Html.text ((Table.getState state.Table).pagination.pageIndex + 1)
                        Html.text " of "
                        Html.text (Table.getPageCount state.Table)
                        Html.text " | Go to page:"
                    ]
                    Html.input [
                        prop.className [ Bulma.Input ]
                        prop.type' "number"
                        prop.style [ style.width (length.rem 4) ]
                        prop.defaultValue ((Table.getState state.Table).pagination.pageIndex + 1)
                        prop.onChange (Index >> Paginate >> dispatch)
                    ]
                    Html.div [
                        prop.className [ Bulma.Select ]
                        prop.children [
                            Html.select [
                                prop.onChange (fun (event : Event) -> snd selectOptions[event.target?value]())
                                prop.value $"{(Table.getState state.Table).pagination.pageSize}"
                                prop.children [
                                    for option in selectOptions do
                                        Html.option [
                                            prop.key option.Key
                                            prop.value option.Key
                                            prop.text (fst option.Value)
                                        ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
            
        Html.div [
            prop.classes [
                "p-2"
            ]
            prop.children [
                Html.p [
                    prop.className [ Bulma.HasBackgroundWarning; Bulma.Mb2; Bulma.P2 ]
                    prop.text "Work in progress..."
                ]
                Html.table [
                    prop.children [
                        thead
                        tbody
                    ]
                ]
                paginationControls
            ]
        ]

    table
    
[<ReactComponent>]
let Component() =
    let state, dispatch = React.useElmish (init, update)
    view state dispatch