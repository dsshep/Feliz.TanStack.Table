module Examples.Basic

open Elmish
open Feliz
open Feliz.UseElmish
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

let columnDef =
    ColumnHelper.createColumnHelper<Person> [
        ColumnHelper.accessor (
          "firstname",
          [ columnDef.accessorKey "Firstname"
            columnDef.cell (fun info -> info.getValue()) ])
        ColumnHelper.accessor (
           (fun p -> p.Lastname),
           [ columnDef.id "lastName"
             columnDef.cell (fun info -> Html.i [ prop.text (info.getValue<int>()) ])
             columnDef.header (fun _ -> Html.span [ prop.text "Last Name" ])
             columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
          "Age",
          [ columnDef.header (fun _ -> "Age")
            columnDef.cell (fun info -> info.renderValue())
            columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
          "Visits",
          [ columnDef.header (fun _ -> Html.span [ prop.text "Visits" ])
            columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
          "Status",
          [ columnDef.header "Status"
            columnDef.footer (fun info -> info.column.id) ])
        ColumnHelper.accessor (
          "Progress",
          [ columnDef.header "Profile Progress"
            columnDef.footer (fun info -> info.column.id) ])
    ]

type State = {
    Table : Table<Person>
}

type Msg =
    | Nop
    
let init () =
    let tableProps = [
        tableProps.data defaultData
        tableProps.columns columnDef ]
    
    let table = Table.init<Person> tableProps
    { Table = table }, Cmd.none

let update (msg: Msg) (state: State) =
    state, Cmd.none
    
let view (state: State) (dispatch: Msg -> unit) =
    let table = 
        let thead =
            Html.thead [
                for headerGroup in Table.getHeaderGroups state.Table do
                     Html.tr [
                         prop.key headerGroup.id
                         prop.children [
                             for header in headerGroup.headers do
                                 Html.th [
                                     prop.key header.id
                                     prop.children [
                                         Html.flexRender (
                                             header.column.columnDef.header,
                                             Table.getContext header)
                                     ]
                                 ]
                         ]
                     ]
            ]
            
        let tbody =
            Html.tbody [
                for row in (Table.getRowModel state.Table).rows do
                    Html.tr [
                        prop.key row.id
                        prop.children [
                            for cell in Table.getVisibleCells row do
                                Html.td [
                                    Html.flexRender(
                                        cell.column.columnDef.cell,
                                        Table.getContext cell)
                                ]
                        ]
                    ]
            ]
            
        let tfoot =
            Html.tfoot [
                for footerGroup in Table.getFooterGroups state.Table do
                    Html.tr [
                        prop.key footerGroup.id
                        prop.children [
                            for footer in footerGroup.headers do
                                Html.th [
                                    prop.key footer.id
                                    prop.children [
                                        Html.flexRender(
                                            footer.column.columnDef.footer,
                                            Table.getContext footer)
                                    ]
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

    table
    
[<ReactComponent>]
let Component() =
    let state, dispatch = React.useElmish (init, update)
    view state dispatch

    