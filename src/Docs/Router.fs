module Router

open Browser.Types
open Feliz.Router
open Fable.Core.JsInterop

type Page =
    | Home
    | Basic
    | Groups
    | Ordering
    | Pinning
    | Resizing
    | EditableData
    | Expanding
    | Grouping
    | PaginationControlled
    
type Link = {
  Name: string
  Page: Page
}

let links: Link list = [
  { Name = "Home"
    Page = Home }
  { Name = "Basic"
    Page = Basic }
  { Name = "Column Groups"
    Page = Groups }
  { Name = "Column Ordering"
    Page = Ordering }
  { Name = "Column Pinning"
    Page = Pinning }
  { Name = "Column Resizing"
    Page = Resizing }
  { Name = "Editable Data"
    Page = EditableData }
  { Name = "Expanding"
    Page = Expanding }
  { Name = "Grouping"
    Page = Grouping }
  { Name = "Pagination Controlled"
    Page = PaginationControlled }
]

[<RequireQualifiedAccess>]
module Page =
    let defaultPage = Page.Home
    
    let parseUrlSegment = function
        | [ "basic" ] -> Page.Basic
        | [ "groups" ] -> Page.Groups
        | [ "ordering" ] -> Page.Ordering
        | [ "pinning" ] -> Page.Pinning
        | [ "resizing" ] -> Page.Resizing
        | [ "editableData" ] -> Page.EditableData
        | [ "expanding" ] -> Page.Expanding
        | [ "grouping" ] -> Page.Grouping
        | [ "paginationControlled" ] -> Page.PaginationControlled
        | _ -> defaultPage
    
    let noQueryString segments : string list * (string * string) list = segments, []
    
    let toUrlSegments (page: Page) =
        let pageSegments =
            match page with
            | Page.Home -> [ "" ]
            | Page.Basic -> [ "basic" ]
            | Page.Groups -> [ "groups" ]
            | Page.Ordering -> [ "ordering" ]
            | Page.Pinning -> [ "pinning" ]
            | Page.Resizing -> [ "resizing" ]
            | Page.EditableData -> [ "editableData" ]
            | Page.Expanding -> [ "expanding" ]
            | Page.Grouping -> [ "grouping" ]
            | Page.PaginationControlled -> [ "paginationControlled" ]
        
        pageSegments |> noQueryString
    
[<RequireQualifiedAccess>]
module Router =
    let goToUrl (e: MouseEvent) =
        e.preventDefault()
        let href : string = !!e.currentTarget?attributes?href?value
        Router.navigate href
        
    let navigatePage (page: Page) = page |> Page.toUrlSegments |> Router.navigate
    
[<RequireQualifiedAccess>]
module Cmd =
    let navigatePage (page: Page) =
        page
        |> Page.toUrlSegments
        |> Cmd.navigate
    