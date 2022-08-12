module Home

open Feliz

let view() =
    Html.div [
        prop.children [
            Html.div [
                prop.children [
                    Html.h1 [
                        prop.className [ Bulma.Title ]
                        prop.text "Feliz.TanStack.Table"
                    ]
                    Html.p [
                        prop.className [ Bulma.Pb4 ]
                        prop.children [
                            Html.text "Feliz style bindings for "
                            Html.a [
                                prop.text "TanStack Table"
                                prop.href "https://tanstack.com/table/v8"
                            ]
                            Html.text "."
                        ]
                    ]
                    Html.h2 [
                        prop.className [ Bulma.Subtitle ]
                        prop.text "Getting Started"
                    ]
                    Html.div [
                        prop.className [ Bulma.Content ]
                        prop.children [
                            Html.ol [
                                prop.children [
                                    Html.li [
                                        Html.text """Add the @tanstack/react-table """
                                        Html.a [
                                            prop.text "npm package"
                                            prop.href "https://www.npmjs.com/package/@tanstack/react-table"
                                        ]
                                        Html.text " ("
                                        Html.code "npm i @tanstack/react-table"
                                        Html.text "/"
                                        Html.code "yarn install @tanstack/react-table"
                                        Html.text "/"
                                        Html.code "Femto"
                                        Html.text ")."
                                    ]
                                    Html.li [
                                        Html.a [
                                            prop.text "Install the nuget package."
                                            prop.href "https://www.nuget.org/packages/Feliz.TanStack.Table"
                                        ]
                                        Html.text " ("
                                        Html.code "Install-Package Feliz.TanStack.Table"
                                        Html.text ")."
                                    ]
                                    Html.li [
                                        Html.text "Have a look at the examples, "
                                        Html.a [
                                            prop.text "which closely follow the official React examples"
                                            prop.href "https://tanstack.com/table/v8/docs/"
                                        ]
                                        Html.text "."
                                    ]
                                ]
                            ]
                        ]
                    ]
                    Html.h2 [
                        prop.className [ Bulma.Subtitle ]
                        prop.text "Mutability and Elmish"
                    ]
                    Html.p [
                        Html.text "TanStack.Table relies on internal mutation to maintain the table state. "
                        Html.text "This means that many of the methods that effect the table state return "
                        Html.code "unit"
                        Html.text ", instead of a new"
                        Html.code "Table<'T>"
                        Html.text ". To force re-render in Elmish, the state should be reassigned, e.g.: "
                        Html.code " { state with Table = state.Table }"
                        Html.text "."
                    ]
                ]
            ]
        ]
    ]

