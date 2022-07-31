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
                        prop.children [
                            Html.text "Feliz style bindings for "
                            Html.a [
                                prop.text "TanStack Table"
                                prop.href "https://tanstack.com/table/v8"
                            ]
                            Html.text "."
                        ]
                    ]
                ]
            ]
        ]
    ]

