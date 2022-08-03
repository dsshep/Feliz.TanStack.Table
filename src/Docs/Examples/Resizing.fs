module Examples.Resizing

open System
open Browser.Types
open Elmish
open Fable.Core.JsInterop
open Feliz
open Feliz.UseElmish
open Feliz.TanStack.Table
open Feliz.style

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
        Firstname =  "Ruthie"
        Lastname = "Hirthe"
        Age = 2
        Visits =217
        Progress =96
        Status ="single"
    }
    {
        Firstname =  "Domenico"
        Lastname = "Swift"
        Age = 7
        Visits =497
        Progress =70
        Status ="relationship"
    }
    {
        Firstname =  "Omer"
        Lastname = "Zboncak"
        Age = 13
        Visits =667
        Progress =12
        Status ="complicated"
    }
|]

let defaultColumns : ColumnDefOptionProp<Person> list list = [
    [ columnDef.header "Name"
      columnDef.footer (fun props -> props.column.id)
      columnDef.columns [
          [ columnDef.accessorKey "Firstname"
            columnDef.cell (fun info -> info.getValue<_>())
            columnDef.footer (fun props -> props.column.id) ]
          [ columnDef.accessorFn (fun p -> p.Lastname)
            columnDef.id "Lastname"
            columnDef.cell (fun info -> info.getValue<_>())
            columnDef.header (fun _ -> Html.span [ prop.text "Last Name" ])
            columnDef.footer (fun props -> props.column.id) ]
      ] ]
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
                  columnDef.header "Status"
                  columnDef.footer (fun props -> props.column.id) ]
                [ columnDef.accessorKey "Progress"
                  columnDef.header "Profile Progress"
                  columnDef.footer (fun props -> props.column.id) ]
            ] ]
      ] ]
]

type ResizeMode =
    | OnChange
    | OnEnd
    static member fromString (s : string) =
        match s with
        | "onChange" -> Some OnChange
        | "onEnd" -> Some OnEnd
        | _ -> None
        
    static member toString = function
        | OnChange -> "onChange"
        | OnEnd -> "onEnd"

type State = {
    Table : Table<Person>
    ResizeMode : ResizeMode
    ResizingHandler : (Event -> Table<Person>) option
}

type Msg =
    | ResizeModeChange of ResizeMode
    | BeginResize of Event * Header<Person>
    | Resize of Event * (Event -> Table<Person>)
    | EndResize of Event * Header<Person>
    
let private rand = Random()    

let private shuffle (arr: 'T[]) =
    let swap x y (a: 'T []) =
        let tmp = a[x]
        a[x] <- a[y]
        a[y] <- tmp
    
    arr
    |> Array.iteri (fun i _ -> arr |> swap i (rand.Next(i, Array.length arr)))
    
    arr
    
let init () =
    let tableProps = [
        tableProps.data defaultData
        tableProps.columns defaultColumns ]
    
    let table = Table.init<Person> tableProps
    
    { Table = table
      ResizeMode = OnEnd
      ResizingHandler = None }, Cmd.none

let update (msg: Msg) (state: State) =
    match msg with
    | ResizeModeChange resizeMode ->
        { state with ResizeMode = resizeMode }, Cmd.none
        
    | BeginResize (event, header) ->
        let handler = Header.resizeHandler header state.Table
        let table = Header.resize event header state.Table
        { state with
            ResizingHandler = Some handler
            Table = table }, Cmd.none
        
    | Resize (event, handler) ->
        let table = handler event
        { state with
            Table = table }, Cmd.none
        
    | EndResize (event, header) ->
        match state.ResizingHandler with
        | Some _ ->
            Fable.Core.JS.console.log "end resize"
            let table = Header.resize event header state.Table
            { state with
                ResizingHandler = None
                Table = table }, Cmd.none
        | None -> state, Cmd.none
        
let view (state: State) (dispatch: Msg -> unit) =
    Fable.Core.JS.console.log "re-render"
    
    let table = 
        let thead =
            Html.thead [
                for headerGroup in Table.getHeaderGroups state.Table do
                     Html.tr [
                         prop.key headerGroup.Id
                         prop.style [
                             style.height (length.px 30)
                             width.fitContent
                         ]
                         prop.children [
                             for header in headerGroup.Headers do
                                 Html.th [
                                     prop.onMouseMove (fun e ->
                                         match state.ResizingHandler with
                                         | Some p -> Resize (e, p) |> dispatch
                                         | None -> ())
                                     prop.onMouseUp (fun e -> EndResize (e, header) |> dispatch)
                                     prop.onMouseLeave (fun e -> EndResize (e, header) |> dispatch)
                                     prop.key header.Id
                                     prop.colSpan header.ColSpan
                                     prop.style [
                                         style.width (Header.getSize header)
                                         position.relative
                                     ]
                                     prop.children [
                                         Html.flexRender (
                                             header.IsPlaceholder,
                                             header.Column.ColumnDef.Header,
                                             Table.getContext header)
                                         Html.div [
                                             //prop.onMouseMove (fun e -> Fable.Core.JS.console.log "mouse move")
                                             //prop.onDrag (fun e -> Fable.Core.JS.console.log "drag...")
                                             prop.onMouseDown (fun e -> BeginResize (e, header) |> dispatch)
                                             prop.onTouchStart (fun e -> ())
                                             prop.onTouchEnd (fun e -> ())
                                             prop.className [
                                                 "resizer"
                                                 if Column.getIsResizing header.Column then "isResizing"
                                             ]
                                             // prop.style [
                                             //     match state.ResizeMode with
                                             //     | OnEnd when state.IsResizing ->
                                             //        transform.translateX (length.px (Table.getState state.Table).ColumnSizingInfo.DeltaOffset)
                                             //     | _ -> ()
                                             // ]
                                         ]
                                     ]
                                 ]
                         ]
                     ]
            ]
            
        let tbody =
            Html.tbody [
                for row in (Table.getRowModel state.Table).Rows do
                    Html.tr [
                        prop.key row.Id
                        prop.children [
                            for cell in Table.getVisibleCells row do
                                Html.td [
                                    Html.flexRender(
                                        cell.Column.ColumnDef.Cell,
                                        Table.getContext cell)
                                ]
                        ]
                    ]
            ]
            
        Html.div [
            prop.className [ Bulma.P2 ]
            prop.children [
                Html.table [
                    prop.style [ style.width (Table.getCenterTotalSize state.Table) ]
                    prop.children [
                        thead
                        tbody
                    ]
                ]
            ]
        ]

    Html.div [
        prop.children [
            Html.p "Work in progress..."
            Html.div [
                prop.className [ Bulma.Field ]
                prop.children [
                    Html.select [
                        prop.className [ Bulma.Select ]
                        prop.onChange (fun (e : Event) ->
                            match ResizeMode.fromString e.target?value, state.ResizeMode with
                            | Some onChange, OnEnd -> ResizeModeChange onChange |> dispatch
                            | Some onEnd, OnChange -> ResizeModeChange onEnd |> dispatch
                            | _ -> ())
                        prop.value (ResizeMode.toString state.ResizeMode)
                        prop.children [
                            Html.option [
                                prop.text "onChange"
                            ]
                            Html.option [
                                prop.text "onEnd"
                            ]
                        ]
                    ]
                ]
            ]
            
            table
        ]
    ]
    
[<ReactComponent>]
let Component () =
    let state, dispatch = React.useElmish (init, update)
    view state dispatch