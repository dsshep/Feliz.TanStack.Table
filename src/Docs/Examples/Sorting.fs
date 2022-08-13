module Examples.Sorting

open Fable.Core.JsInterop
open Elmish
open Browser.Types
open Feliz
open Feliz.UseElmish
open Feliz.TanStack.Table
open MakeData
open MakeData.Types

let columnDef =
    ColumnHelper.createColumnHelper<PersonCreatedAt> [
        ColumnHelper.accessor (
          "firstname",
          [ columnDef.accessorKey "Firstname"
            columnDef.cell (fun info -> info.getValue())
            columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
           (fun (p : PersonCreatedAt) -> p.Lastname),
           [ columnDef.id "lastName"
             columnDef.cell (fun info -> Html.i [ prop.text (info.getValue<int>()) ])
             columnDef.header (fun _ -> Html.span [ prop.text "Last Name" ])
             columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
          "Age",
          [ columnDef.header (fun _ -> "Age")
            columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
          "Visits",
          [ columnDef.header (fun _ -> Html.span [ prop.text "Visits" ])
            columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
          "Status",
          [ columnDef.header "Status"
            columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
          "Progress",
          [ columnDef.header "Profile Progress"
            columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
            "CreatedAt",
            [ columnDef.header "Created At" ])
    ]

type PaginateChange =
    | First
    | Previous
    | Next
    | Last
    | Index of int

type PageState = {
    PageIndex: int
    PageSize: int
}

type Msg =
    | Paginate of PaginateChange
    | PageSizeChange of int
    | SortColumn of Column<PersonCreatedAt>
    
type State = {
    Table : Table<PersonCreatedAt>
    PageState : PageState
}

let init () =
    let defaultData = MakeData.make 1000
    
    let tableProps = [
        tableProps.data defaultData
        tableProps.columns columnDef
        tableProps.sortedRowModel()
        tableProps.paginationRowModel() ]
    
    let table = Table.init<PersonCreatedAt> tableProps
    
    { Table = table
      PageState = { PageIndex = 0; PageSize = 10 } }, Cmd.none

let update (msg: Msg) (state: State) =
    match msg with
    | Paginate paginateChange ->
        let table = 
            match paginateChange with
            | First -> Table.setPageIndex 0 state.Table
            | Previous -> Table.previousPage state.Table
            | Next -> Table.nextPage state.Table
            | Last -> Table.setPageIndex (Table.getPageCount state.Table - 1) state.Table
            | Index i -> Table.setPageIndex (i - 1) state.Table
            
        { state with Table = table }, Cmd.none
        
    | PageSizeChange pageSize ->
        { state with Table = Table.setPageSize pageSize state.Table }, Cmd.none
        
    | SortColumn column ->
        Column.toggleSorting column
        { state with Table = state.Table }, Cmd.none
        
let view (state: State) (dispatch: Msg -> unit) =
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
                                     prop.children [
                                         if Header.isPlaceholder header then Html.none
                                         else
                                             Html.div [
                                                 prop.style (
                                                     if Column.getCanSort header.column then
                                                         [ style.cursor.pointer; style.userSelect.none ]
                                                     else [])
                                                 prop.onClick (fun _ -> SortColumn header.column |> dispatch)
                                                 prop.children [
                                                     Html.flexRender (
                                                         header.column.columnDef.header,
                                                         Table.getContext header)
                                                     Html.text (
                                                         match Column.getIsSorted header.column with
                                                         | Asc -> "🔼"
                                                         | Desc -> "🔽"
                                                         | NotSorted -> "")
                                                 ]
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
                                    Html.flexRender(
                                        cell.column.columnDef.cell,
                                        Table.getContext cell)
                                ]
                        ]
                    ]
            ]
            
        let tfoot =
            Html.tfoot [
                for footerGroup in Table.getFooterGroups state.Table do
                    Html.tr [
                        prop.key footerGroup.id
                        prop.children [
                            for footer in footerGroup.headers do
                                Html.th [
                                    prop.key footer.id
                                    prop.children [
                                        Html.flexRender(
                                            footer.column.columnDef.footer,
                                            Table.getContext footer)
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
                    pageControl "<" (Table.hasPreviousPage state.Table |> not) (fun _ -> Paginate Previous |> dispatch)
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
                Html.table [
                    prop.children [
                        thead
                        tbody
                        tfoot
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

    