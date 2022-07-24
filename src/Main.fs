module Main

open App.Table
open TanStackTable
open Elmish
open Feliz
open Feliz.UseElmish

type State = {
    Count: int
    ShowTable: bool
}

type Msg = 
    | Increment
    | ShowTable

let init() =
    { Count = 0; ShowTable = false }, Cmd.none

let update (msg : Msg) (state : State) = 
    match msg with 
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
    | ShowTable -> { state with ShowTable = not state.ShowTable }, Cmd.none

let view (state : State) (dispatch : Msg -> unit) =
    let tableComponent = createTable(fun table ->
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
            
        let tfoot =
            Html.tfoot [
                for footerGroup in table.getFooterGroups() do
                    Html.tr [
                        prop.key footerGroup.id
                        prop.children [
                            for footer in footerGroup.headers do
                                Html.th [
                                    prop.key footer.id
                                    prop.flexRender(footer.column.columnDef.footer, footer.getContext())
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
            ]
        ]
        )
       
    Html.div [
        prop.children [
            Html.p [
                prop.text $"Count is %i{state.Count}"
            ]
            Html.button [
                prop.text "Increment"
                prop.onClick(fun _ -> dispatch Increment)
            ]
            tableComponent
        ]
    ]
