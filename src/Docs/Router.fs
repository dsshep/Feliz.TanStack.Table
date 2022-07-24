module App.Router

open Browser.Types
open Feliz.Router
open Fable.Core.JsInterop

type Page =
    | Home
    | Basic
    | ColumnGroups
    
[<RequireQualifiedAccess>]
module Page =
    let defaultPage = Page.Home
    
    let parseUrlSegment = function
        | [ "basic" ] -> Page.Basic
        | [ "columnGroups" ] -> Page.ColumnGroups
        | _ -> defaultPage
    
    let noQueryString segments : string list * (string * string) list = segments, []
    
    let toUrlSegments (page: Page) =
        let pageSegments =
            match page with
            | Page.Home -> [ "" ]
            | Page.Basic -> [ "basic" ]
            | Page.ColumnGroups -> [ "columnGroups" ]
        
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
    let navigatePage (page: Page) = page |> Page.toUrlSegments |> Cmd.navigate