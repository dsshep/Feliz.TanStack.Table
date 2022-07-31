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
                    Html.p [
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
                                        Html.text ")."
                                    ]
                                    Html.li [
                                        Html.text "Install the nuget package (coming soon...)"
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
                ]
            ]
        ]
    ]

