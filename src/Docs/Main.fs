module Main

open Elmish
open Feliz
open Router
open Zanaptak.TypedCssClasses

type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.4/css/bulma.min.css", Naming.PascalCase>

type State = {
    Page: Page
    BasicState: Examples.Basic.State
    OrderingState: Examples.Ordering.State
}

type Msg = 
    | UrlChanged of Page
    | BasicMsg of Examples.Basic.Msg
    | OrderingMsg of Examples.Ordering.Msg

let init () =
    let nextPage = Router.currentUrl() |> Page.parseUrlSegment
    let basicState, basicCmd = Examples.Basic.init()
    let orderingState, orderingCmd = Examples.Ordering.init()
    { Page = nextPage
      BasicState = basicState
      OrderingState = orderingState },
    Cmd.batch [ Cmd.map BasicMsg basicCmd
                Cmd.map OrderingMsg orderingCmd ]

let update (msg : Msg) (state : State) = 
    match msg with 
    | UrlChanged page -> { state with Page = page }, Cmd.none
    | BasicMsg elmMsg ->
        let update, cmd = Examples.Basic.update state.BasicState elmMsg
        { state with BasicState = update }, Cmd.map BasicMsg cmd
    | OrderingMsg msg ->
        let update, cmd = Examples.Ordering.update state.OrderingState msg
        { state with OrderingState = update }, Cmd.map OrderingMsg cmd

let view (state : State) (dispatch : Msg -> unit) =
    let pages: {| displayName: string; isSelected: bool |} list = [
        {| displayName = "Basic"; isSelected = true |}
        {| displayName = "Grouping"; isSelected = false |}
        {| displayName = "Another example"; isSelected = false |}
        {| displayName = "Something silly"; isSelected = false |}
        {| displayName = "Reeeee"; isSelected = false |}
    ]
    
    let sidebar =
        Html.div [
            prop.className [ Bulma.Tile; Bulma.Is2 ]
            prop.children [
                Html.aside [
                    prop.className Bulma.Menu
                    prop.children [
                        Html.ul [
                            prop.className Bulma.MenuList
                            prop.children [
                                Html.p [
                                    prop.className [ Bulma.MenuLabel; Bulma.IsUnselectable ]
                                    prop.text "Feliz.TanStack.Table"
                                ]
                                for p in pages do 
                                    Html.li [
                                        Html.a [
                                            prop.className [
                                                if p.isSelected then Bulma.HasBackgroundPrimary
                                                if p.isSelected then Bulma.IsActive
                                            ]
                                            prop.text p.displayName
                                        ]
                                    ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
        
    let main =
        Html.main [
            prop.className [ Bulma.Tile; Bulma.Is10 ]
            prop.children [
                Html.div [ prop.children (Examples.Basic.view state.BasicState (BasicMsg >> dispatch)) ]
                Html.div [ prop.children (Examples.Ordering.view state.OrderingState (OrderingMsg >> dispatch)) ]
            ]
        ]
        
    React.router [
        router.hashMode
        router.onUrlChanged (Page.parseUrlSegment >> UrlChanged >> dispatch)
        router.children [
            Html.div [
                prop.className [ Bulma.Tile; Bulma.IsAncestor; Bulma.P6 ]
                prop.children [
                    sidebar
                    main
                ]
            ]
        ]
    ]