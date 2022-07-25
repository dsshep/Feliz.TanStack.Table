module App.Main

open Feliz.TanStack.Table
open Elmish
open Feliz

open Table
open Router

type State = {
    Page: Page
}

type Msg = 
    | UrlChanged of Page

let init() =
    let nextPage = Router.currentUrl() |> Page.parseUrlSegment
    { Page = nextPage }, Cmd.none

let update (msg : Msg) (state : State) = 
    match msg with 
    | UrlChanged page -> { state with Page = page }, Cmd.none

let view (state : State) (dispatch : Msg -> unit) =
    let renderTable = fun (table: Table<Link>) ->
        let thead =
            Html.thead [
                for headerGroup in table.getHeaderGroups() do
                     Html.tr [
                         prop.key headerGroup.id
                         prop.children [
                             for header in headerGroup.headers do
                                 Html.th [
                                     prop.key header.id
                                     prop.flexRender (header.column.columnDef.header, header.getContext())
                                 ]
                         ]
                     ]
            ]
        
        let tbody =
            Html.tbody [
                for row in table.getRowModel().rows do
                    Html.tr [
                        prop.key row.id
                        prop.children [
                            for cell in row.getVisibleCells() do
                                Html.td [
                                    prop.flexRender(cell.column.columnDef.cell, cell.getContext())
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
                    ]
                ]
            ]
        ]
    
    let tableElement = createTable(renderTable)
       
    let subTable =
        match state.Page with
        | Home -> Html.p [ prop.text "Home" ]
        | Basic -> Examples.Basic.createTable()
        | ColumnGroups -> Examples.ColumnGroups.createTable()
       
    React.router [
        router.hashMode
        router.onUrlChanged (Page.parseUrlSegment >> UrlChanged >> dispatch)
        router.children [
            tableElement
            subTable
        ]
    ]