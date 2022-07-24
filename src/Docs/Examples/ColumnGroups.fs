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
  [ Header "Name"
    FooterFn (fun props -> props.column.id)
    Columns [
      [ AccessorKey "Firstname"
        Cell (fun info -> info.getValue())
        FooterFn (fun props -> props.column.id) ]
      [ AccessorFn (fun row -> row.Lastname)
        Id "Lastname"
        Cell (fun info -> info.getValue())
        HeaderFn (fun _ -> Html.span [ prop.text "Last Name" ])
        FooterFn (fun props -> props.column.id) ]
      ]
    ]
  [ Header "Info"
    FooterFn (fun props -> props.column.id)
    Columns [
      [ AccessorKey "Age"
        HeaderFn (fun _ -> "Age")
        FooterFn (fun props -> props.column.id) ]
      [ Header "More Info"
        Columns [
          [ AccessorKey "Visits"
            HeaderFn (fun _ -> Html.span [ prop.text "Visits" ])
            FooterFn (fun props -> props.column.id) ]
          [ AccessorKey "Status"
            HeaderFn (fun _ -> Html.span [ prop.text "Status" ])
            FooterFn (fun props -> props.column.id) ]
          [ AccessorKey "Progress"
            HeaderFn (fun _ -> Html.span [ prop.text "Profile Progress" ])
            FooterFn (fun props -> props.column.id) ]
        ] ]
      
    ] ]
]

let rec createTable () =
  createTanStackTable defaultData columns (fun table ->
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
        )
)
