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

let columnDef: ColumnDefOption<Person> list list = [
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
    TableState : Table<Person>
}

type Msg =
    | StateChange of ElmishTable.Msg
    
let init () =
    let tableProps = [
        tableProps.data defaultData
        tableProps.columns columnDef ]
    
    let table = ElmishTable.init<Person> tableProps
    { TableState = table }, Cmd.none

let update (state: State) (msg: Msg) =
    console.log("State Update")
    match msg with
    | StateChange msg ->
        let tableState, tableCmd = ElmishTable.update state.TableState msg
        { state with TableState = tableState }, Cmd.map StateChange tableCmd
    
let view (state: State) (dispatch: Msg -> unit) =
    let table = ElmishTable.render state.TableState (StateChange >> dispatch) (
        let thead =
            Html.thead [
                for headerGroup in state.TableState.getHeaderGroups() do
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
                for row in state.TableState.getRowModel().rows do
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
                for footerGroup in state.TableState.getFooterGroups() do
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
        ])

    Html.div [
        prop.children [
            Html.button [
                prop.text "Click me"
            ]
            table
        ]
    ]
    