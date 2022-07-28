module Table

open Feliz.TanStack.Table
open Feliz

type Link = {
  Name: string
  Url: string
  Description: string
}

let links: Link list = [
  { Name = "Home"
    Url = "#/"
    Description = "Home." }
  { Name = "Basic"
    Url = "#/basic"
    Description = "Basic example from the TanStack docs." }
  { Name = "Basic"
    Url = "#/columnGroups"
    Description = "Column group example from the TanStack docs." }
]

let columnDef: ColumnDefOptionProp<Link> list list = [
    [ columnDef.id "Name"
      columnDef.accessorKey "Name"
      columnDef.cell (fun info -> info.getValue()) ]
    [ columnDef.id "Url"
      columnDef.accessorKey "Url"
      columnDef.cell (fun info -> Html.a [
        prop.href (info.getValue<string>())
        prop.text (info.getValue<string>())
      ]) ]
    [ columnDef.id "Description"
      columnDef.accessorKey "Description"
      columnDef.cell (fun info -> info.getValue()) ]
]

//let rec createTable render =
//  Table.Create(links, columnDef, render)
