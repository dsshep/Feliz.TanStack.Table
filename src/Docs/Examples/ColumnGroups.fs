module App.Examples.ColumnGroups

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
]

let columns: ColumnDefOption<Person> list list = [
  [ columnDef.header "Name"
    columnDef.footer (fun props -> props.column.id)
    columnDef.columns [
      [ columnDef.accessorKey "Firstname"
        columnDef.cell (fun info -> info.getValue())
        columnDef.footer (fun props -> props.column.id) ]
      [ columnDef.accessorFn (fun row -> row.Lastname)
        columnDef.id "Lastname"
        columnDef.cell (fun info -> info.getValue())
        columnDef.header (fun _ -> Html.span [ prop.text "Last Name" ])
        columnDef.footer (fun props -> props.column.id) ]
      ]
    ]
  [ columnDef.header "Info"
    columnDef.footer (fun props -> props.column.id)
    columnDef.columns [
      [ columnDef.accessorKey "Age"
        columnDef.header (fun _ -> "Age")
        columnDef.footer (fun props -> props.column.id) ]
      [ columnDef.header "More Info"
        columnDef.columns [
          [ columnDef.accessorKey "Visits"
            columnDef.header (fun _ -> Html.span [ prop.text "Visits" ])
            columnDef.footer (fun props -> props.column.id) ]
          [ columnDef.accessorKey "Status"
            columnDef.header (fun _ -> Html.span [ prop.text "Status" ])
            columnDef.footer (fun props -> props.column.id) ]
          [ columnDef.accessorKey "Progress"
            columnDef.header (fun _ -> Html.span [ prop.text "Profile Progress" ])
            columnDef.footer (fun props -> props.column.id) ]
        ] ]
    ] ]
]

let rec createTable () =
    Table.Create(defaultData, columns, (fun table ->
        let thead =
            Html.thead [
                for headerGroup in table.getHeaderGroups() do
                     Html.tr [
                         prop.key headerGroup.id
                         prop.children [
                             for header in headerGroup.headers do
                                 Html.th [
                                     prop.colSpan header.colSpan
                                     prop.key header.id
                                     (if header.isPlaceholder then prop.children [] else
                                         prop.flexRender (header.column.columnDef.header, header.getContext()))
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
                                    prop.colSpan footer.colSpan
                                    prop.key footer.id
                                    (if footer.isPlaceholder then prop.children [] else
                                         prop.flexRender (footer.column.columnDef.header, footer.getContext()))
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
        ))

