module Main

open Elmish
open Feliz
open Router

type State = {
    Page: Page
}

type Msg = 
    | UrlChanged of Page

let init () =
    let nextPage = Router.currentUrl() |> Page.parseUrlSegment
    { Page = nextPage }, Cmd.none

let update (msg : Msg) (state : State) = 
    match msg with 
    | UrlChanged page ->
        { state with Page = page }, Cmd.none

let view (state : State) (dispatch : Msg -> unit) =
    let colDiv e =
        Html.div [
            prop.className [ Bulma.Column; Bulma.IsFull ]
            prop.children [ e ]
        ]
    
    let sourceLinkOpt =
        let baseUrl = "https://github.com/dsshep/Feliz.TanStack.Table/tree/main/src/Docs/Examples/"
        match state.Page with
        | Home -> None
        | Basic -> Some $"{baseUrl}Basic.fs"
        | Groups -> Some $"{baseUrl}Groups.fs"
        | Ordering -> Some $"{baseUrl}Ordering.fs"
        | Pinning -> Some $"{baseUrl}Pinning.fs"
        | Resizing -> Some $"{baseUrl}Resizing.fs"
    
    let sourceLink =
        match sourceLinkOpt with
        | None -> Html.none
        | Some l ->
            Html.section [
                prop.className [ Bulma.Container; Bulma.HasBackgroundLight ]
                prop.children [
                    Html.div [
                        prop.className [ Bulma.Px3; Bulma.Py2 ]
                        prop.children [
                            Html.p [
                                Html.text "View the source code for this example "
                                Html.a [
                                    prop.text "here"
                                    prop.href l
                                ]
                                Html.text "."
                            ]
                        ]
                    ]
                ]
            ]
    
    let sidebar =
        Html.div [
            prop.className [ Bulma.Tile; Bulma.Is3 ]
            prop.children [
                Html.aside [
                    prop.className [ Bulma.Menu; Bulma.Pb2 ]
                    prop.children [
                        Html.ul [
                            prop.className Bulma.MenuList
                            prop.children [
                                Html.p [
                                    prop.className [ Bulma.MenuLabel; Bulma.IsUnselectable ]
                                    prop.text "Feliz.TanStack.Table"
                                ]
                                for l in links do
                                    Html.li [
                                        Html.a [
                                            prop.className [
                                                if l.Page = state.Page then Bulma.HasBackgroundPrimary
                                                if l.Page = state.Page then Bulma.IsActive
                                            ]
                                            prop.text l.Name
                                            prop.onClick (fun _ -> Router.navigatePage l.Page)
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
            prop.className [ Bulma.Tile; Bulma.Is9 ]
            prop.children [
                Html.div [
                    prop.className [ Bulma.Columns; Bulma.IsMultiline ]
                    prop.children [
                        colDiv
                            (Html.section [
                                prop.className Bulma.Container
                                prop.children [
                                    match state.Page with
                                    | Page.Home -> Home.view()
                                    | Page.Basic -> Examples.Basic.Component()
                                    | Page.Groups -> Examples.Groups.Component()
                                    | Page.Ordering -> Examples.Ordering.Component()
                                    | Page.Pinning -> Examples.Pinning.Component()
                                    | Page.Resizing -> Examples.Resizing.Component()
                                ]
                            ])
                        colDiv sourceLink
                    ]
                ]
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