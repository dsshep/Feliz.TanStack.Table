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

let columnDef: ColumnDefOption<Link> list list = [
  [ Id "Name"
    AccessorKey "Name"
    Cell (fun info -> info.getValue()) ]
  [ Id "Url"
    AccessorKey "Url"
    Cell (fun info -> Html.a [
      prop.href (info.getValue<string>())
      prop.text (info.getValue<string>())
    ]) ]
  [ Id "Description"
    AccessorKey "Description"
    Cell (fun info -> info.getValue()) ]
]

let rec createTable(render) =
  createTanStackTable links columnDef render
