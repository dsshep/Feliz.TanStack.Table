module App.Main

open Feliz.TanStack.Table
open Elmish
open Feliz

open Table
open Router

type State = {
    Page: Page
    ElmishState: Examples.Elmish.State
    OrderingState: Examples.Ordering.State
}

type Msg = 
    | UrlChanged of Page
    | ElmishMsg of Examples.Elmish.Msg
    | OrderingMsg of Examples.Ordering.Msg

let init() =
    let nextPage = Router.currentUrl() |> Page.parseUrlSegment
    let elmishState, elmishCmd = Examples.Elmish.init()
    let orderingState, orderingCmd = Examples.Ordering.init()
    { Page = nextPage
      ElmishState = elmishState
      OrderingState = orderingState },
    Cmd.batch [ Cmd.map ElmishMsg elmishCmd
                Cmd.map OrderingMsg orderingCmd ]

let update (msg : Msg) (state : State) = 
    match msg with 
    | UrlChanged page -> { state with Page = page }, Cmd.none
    | ElmishMsg elmMsg ->
        let update, cmd = Examples.Elmish.update state.ElmishState elmMsg
        { state with ElmishState = update }, Cmd.map ElmishMsg cmd
    | OrderingMsg msg ->
        let update, cmd = Examples.Ordering.update state.OrderingState msg
        { state with OrderingState = update }, Cmd.map OrderingMsg cmd

let view (state : State) (dispatch : Msg -> unit) =
//    let renderTable = fun (table: Table<Link>) ->
//        let thead =
//            Html.thead [
//                for headerGroup in table.getHeaderGroups() do
//                     Html.tr [
//                         prop.key headerGroup.id
//                         prop.children [
//                             for header in headerGroup.headers do
//                                 Html.th [
//                                     prop.key header.id
//                                     prop.flexRender (header.column.columnDef.header, header.getContext())
//                                 ]
//                         ]
//                     ]
//            ]
//        
//        let tbody =
//            Html.tbody [
//                for row in table.getRowModel().rows do
//                    Html.tr [
//                        prop.key row.id
//                        prop.children [
//                            for cell in row.getVisibleCells() do
//                                Html.td [
//                                    prop.flexRender(cell.column.columnDef.cell, cell.getContext())
//                                ]
//                        ]
//                    ]
//            ]
//        
//        Html.div [
//            prop.classes [
//                "p-2"
//            ]
//            prop.children [
//                Html.table [
//                    prop.children [
//                        thead
//                        tbody
//                    ]
//                ]
//            ]
//        ]
    
    //let tableElement = createTable(renderTable)
       
    let subTable =
        match state.Page with
        | Home -> Html.p [ prop.text "Home" ]
        //| Basic -> Examples.Basic.createTable()
        //| ColumnGroups -> Examples.ColumnGroups.createTable()
       
    React.router [
        router.hashMode
        router.onUrlChanged (Page.parseUrlSegment >> UrlChanged >> dispatch)
        router.children [
            //tableElement
            subTable
            Html.div [ prop.children (Examples.Elmish.view state.ElmishState (ElmishMsg >> dispatch)) ]
            Html.div [ prop.children (Examples.Ordering.view state.OrderingState (OrderingMsg >> dispatch)) ]
        ]
    ]