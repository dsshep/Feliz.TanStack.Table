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
    [ Id "firstname"
      AccessorKey "Firstname"
      Cell (fun info -> info.getValue()) ]
    [ Id "lastName"
      AccessorKey (nameof Unchecked.defaultof<Person>.Lastname)
      Cell (fun info -> Html.i [ prop.text (info.getValue<int>()) ])
      HeaderFn (fun _ -> Html.span [ prop.text "Last Name" ])
      FooterFn (fun info -> info.column.id) ]
    [ AccessorKey "Age"
      HeaderFn (fun _ -> "Age")
      Cell (fun info -> info.renderValue())
      FooterFn (fun info -> info.column.id) ]
    [ AccessorKey "Visits"
      HeaderFn (fun _ -> Html.span [ prop.text "Visits" ])
      FooterFn (fun info -> info.column.id) ]
    [ AccessorKey "Status"
      Header "Status"
      FooterFn (fun info -> info.column.id) ]
    [ AccessorKey "Progress"
      Header "Profile Progress"
      FooterFn (fun info -> info.column.id) ]
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
