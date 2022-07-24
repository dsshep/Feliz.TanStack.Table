module App.Examples.Basic

open Feliz.TanStack.Table
open Feliz

type Person = {
  Firstname: string
  Lastname: string
  Age: int
  Visits: int
  Status: string
  Progress: int
}

let defaultData: Person list = [
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
]

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

let rec createTable () =
  createTanStackTable defaultData columnDef (fun table ->
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
