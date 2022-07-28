module App.Examples.Elmish

open Elmish
open Fable.Core.JS
open Feliz
open Feliz.TanStack.Table

type Person = {
  Firstname: string
  Lastname: string
  Age: int
  Visits: int
  Status: string
  Progress: int
}

let defaultData: Person[] = [|
    {
      Firstname = "tanner"
      Lastname = "linsley"
      Age = 24
      Visits = 100
      Status = "In Relationship"
      Progress = 50
    }
    {
      Firstname= "tandy"
      Lastname= "miller"
      Age= 40
      Visits= 40
      Status= "Single"
      Progress= 80
    }
    {
      Firstname= "joe"
      Lastname= "dirte"
      Age= 45
      Visits= 20
      Status= "Complicated"
      Progress= 10
    }
|]

let columnDef: ColumnDefOptionProp<Person> list list = [
    [ columnDef.id "firstname"
      columnDef.accessorKey "Firstname"
      columnDef.cell (fun info -> info.getValue()) ]
    [ columnDef.id "lastName"
      columnDef.accessorKey (nameof Unchecked.defaultof<Person>.Lastname)
      columnDef.cell (fun info -> Html.i [ prop.text (info.getValue<int>()) ])
      columnDef.header (fun _ -> Html.span [ prop.text "Last Name" ])
      columnDef.footer (fun info -> info.column.id) ]
    [ columnDef.accessorKey "Age"
      columnDef.header (fun _ -> "Age")
      columnDef.cell (fun info -> info.renderValue())
      columnDef.footer (fun info -> info.column.id) ]
    [ columnDef.accessorKey "Visits"
      columnDef.header (fun _ -> Html.span [ prop.text "Visits" ])
      columnDef.footer (fun info -> info.column.id) ]
    [ columnDef.accessorKey "Status"
      columnDef.header "Status"
      columnDef.footer (fun info -> info.column.id) ]
    [ columnDef.accessorKey "Progress"
      columnDef.header "Profile Progress"
      columnDef.footer (fun info -> info.column.id) ]
]

type State = {
    Table : Table<Person>
    HideColumn: bool
}

type Msg =
    | ButtonClicked
    
let init () =
    let tableProps = [
        tableProps.data defaultData
        tableProps.columns columnDef ]
    
    let table = Table.init<Person> tableProps
    { Table = table; HideColumn = false }, Cmd.none

let update (state: State) (msg: Msg) =
    match msg with
    | ButtonClicked ->
        debugger()
        { state with
            Table = Table.setColumnVisibility "firstname" (not state.HideColumn) state.Table
            HideColumn = not state.HideColumn }, Cmd.none
    
let view (state: State) (dispatch: Msg -> unit) =
    let table = 
        let thead =
            Html.thead [
                for headerGroup in Table.getHeaderGroups state.Table do
                     Html.tr [
                         prop.key headerGroup.Id
                         prop.children [
                             for header in headerGroup.Headers do
                                 Html.th [
                                     prop.key header.Id
                                     prop.flexRender (
                                         header.Column.ColumnDef.Header,
                                         Table.getContext header)
                                 ]
                         ]
                     ]
            ]
            
        let tbody =
            Html.tbody [
                for row in (Table.getRowModel state.Table).Rows do
                    Html.tr [
                        prop.key row.Id
                        prop.children [
                            for cell in Table.getVisibleCells row do
                                Html.td [
                                    prop.flexRender(
                                        cell.Column.ColumnDef.Cell,
                                        Table.getContext cell)
                                ]
                        ]
                    ]
            ]
            
        let tfoot =
            Html.tfoot [
                for footerGroup in Table.getFooterGroups state.Table do
                    Html.tr [
                        prop.key footerGroup.Id
                        prop.children [
                            for footer in footerGroup.Headers do
                                Html.th [
                                    prop.key footer.Id
                                    prop.flexRender(
                                        footer.Column.ColumnDef.Footer,
                                        Table.getContext footer)
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

    Html.div [
        prop.children [
            Html.button [
                prop.text "Click me"
                prop.onClick (fun _ -> dispatch ButtonClicked)
            ]
            table
        ]
    ]
    