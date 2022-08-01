module Examples.Pinning

open System
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
    {
        Firstname =  "Esmeralda"
        Lastname = "Kihn"
        Age = 9
        Visits =262
        Progress =6
        Status ="complicated"
    }
    {
        Firstname =  "Amara"
        Lastname = "Bergnaum"
        Age = 31
        Visits =386
        Progress =94
        Status ="single"
    }
    {
        Firstname =  "Meredith"
        Lastname = "Weber"
        Age = 32
        Visits =509
        Progress =76
        Status ="complicated"
    }
    {
        Firstname =  "Kareem"
        Lastname = "Bashirian"
        Age = 33
        Visits =802
        Progress =83
        Status ="relationship"
    }
    {
        Firstname =  "Rosalyn"
        Lastname = "Hoppe"
        Age = 6
        Visits =464
        Progress =57
        Status ="single"
    }
    {
        Firstname =  "Marcella"
        Lastname = "Powlowski"
        Age = 21
        Visits =758
        Progress =10
        Status ="single"
    }
    {
        Firstname =  "Jesus"
        Lastname = "Rowe"
        Age = 39
        Visits =738
        Progress =44
        Status ="complicated"
    }
    {
        Firstname =  "Astrid"
        Lastname = "Pfeffer"
        Age = 9
        Visits =644
        Progress =76
        Status ="complicated"
    }
    {
        Firstname =  "Trey"
        Lastname = "Keebler"
        Age = 0
        Visits =487
        Progress =5
        Status ="single"
    }
    {
        Firstname =  "Verdie"
        Lastname = "Mraz"
        Age = 25
        Visits =610
        Progress =83
        Status ="complicated"
    }
    {
        Firstname =  "Paige"
        Lastname = "Torphy"
        Age = 22
        Visits =63
        Progress =26
        Status ="single"
    }
    {
        Firstname =  "Cassandre"
        Lastname = "Koepp"
        Age = 6
        Visits =827
        Progress =11
        Status ="relationship"
    }
    {
        Firstname =  "Walker"
        Lastname = "Farrell"
        Age = 34
        Visits =836
        Progress =63
        Status ="complicated"
    }
    {
        Firstname =  "Brenna"
        Lastname = "Koss"
        Age = 25
        Visits =150
        Progress =44
        Status ="complicated"
    }
    {
        Firstname =  "Carlos"
        Lastname = "Buckridge"
        Age = 10
        Visits =766
        Progress =20
        Status ="complicated"
    }
    {
        Firstname =  "Francesca"
        Lastname = "Tremblay"
        Age = 9
        Visits =245
        Progress =20
        Status ="relationship"
    }
    {
        Firstname =  "Nat"
        Lastname = "Ryan"
        Age = 24
        Visits =561
        Progress =32
        Status ="single"
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

type State = {
    Table : Table<Person>
    IsSplitMode : bool
}

type Msg =
    | AllCheckedChanged
    | ColumnChecked of Column<Person>
    | Reset
    | Randomise
    | SplitModeChanged
    | ToggleColumnPinning of Column<Person> * ColumnPinningPosition
    
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
    
    { Table = table; IsSplitMode = false }, Cmd.none

let update (msg: Msg) (state: State) =
    match msg with
    | AllCheckedChanged ->
        let anyVisible = Table.getIsSomeColumnsVisible state.Table
        let table = Table.toggleAllColumnsVisible (not anyVisible) state.Table
        { state with Table = table }, Cmd.none
    | ColumnChecked c ->
        let isVisible = Column.getIsVisible c
        let table = Table.setColumnVisibility c (not isVisible) state.Table
        { state with Table = table }, Cmd.none
    | Reset ->
        let table = Table.resetColumnOrder state.Table
        { state with Table = table }, Cmd.none
    | Randomise ->
        let shuffled =
            Table.getAllLeafColumns state.Table
            |> Array.map (fun c -> c.Id)
            |> shuffle
            
        let table = Table.setColumnOrder shuffled state.Table
        { state with Table = table }, Cmd.none
    | SplitModeChanged ->
        { state with IsSplitMode = (not state.IsSplitMode) }, Cmd.none
    | ToggleColumnPinning (column, columnPinningPosition) ->
        let table = Table.pinColumn columnPinningPosition column state.Table
        { state with Table = table }, Cmd.none
        
let view (state: State) (dispatch: Msg -> unit) =
    Fable.Core.JS.console.log "Re-render"
    let renderPinButtons (column : Column<Person>) =
        Html.div [
            prop.children [
                match Column.getIsPinned column with
                | Neither | Right ->
                   Html.button [
                       prop.className [ Bulma.Button; Bulma.IsSmall ]
                       prop.text "<="
                       prop.onClick (fun _ -> ToggleColumnPinning (column, Left) |> dispatch)
                   ]
                | _ -> Html.none
                match Column.getIsPinned column with
                | Neither | Left ->
                    Html.button [
                       prop.className [ Bulma.Button; Bulma.IsSmall ]
                       prop.text "=>"
                       prop.onClick (fun _ -> ToggleColumnPinning (column, Right) |> dispatch)
                   ]
                | _ -> Html.none
                match Column.getIsPinned column with
                | Left | Right ->
                    Html.button [
                       prop.className [ Bulma.Button; Bulma.IsSmall ]
                       prop.text "x"
                       prop.onClick (fun _ -> ToggleColumnPinning (column, Neither) |> dispatch)
                   ]
                | _ -> Html.none
            ]
        ]
    
    let table = 
        let thead =
            Html.thead [
                for headerGroup in Table.getHeaderGroups state.Table do
                    Html.tr [
                        prop.key headerGroup.Id
                        prop.children [
                            for header in headerGroup.Headers do
                                Html.th [
                                    prop.key header.Id
                                    prop.colSpan header.ColSpan
                                    prop.children [
                                        Html.div [
                                            prop.flexRender (
                                                header.IsPlaceholder,
                                                header.Column.ColumnDef.Header,
                                                Table.getContext header)
                                        ]
                                        if not header.IsPlaceholder && Column.getCanPin header.Column then
                                            renderPinButtons header.Column
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
                                    prop.flexRender(
                                        cell.Column.ColumnDef.Cell,
                                        Table.getContext cell)
                                ]
                        ]
                    ]
            ]
            
        let tfoot =
            Html.tfoot [
                for footerGroup in Table.getFooterGroups state.Table do
                    Html.tr [
                        prop.key footerGroup.Id
                        prop.children [
                            for footer in footerGroup.Headers do
                                Html.th [
                                    prop.key footer.Id
                                    prop.colSpan footer.ColSpan
                                    prop.flexRender(
                                        footer.IsPlaceholder,
                                        footer.Column.ColumnDef.Footer,
                                        Table.getContext footer)
                                ]
                        ]
                    ]
            ]
        
        Html.div [
            prop.className [ Bulma.P2 ]
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

    let inputCheck (props : {| id: string; text: string; isChecked: bool; onChange: unit -> unit |}) =
        Html.div [
            prop.className [ Bulma.Field ]
            prop.children [
                Html.label [
                    prop.className Bulma.Checkbox
                    prop.children [
                        Html.input [
                            prop.className [ Bulma.Pl2 ]
                            prop.type' "checkbox"
                            prop.isChecked props.isChecked
                            prop.onCheckedChange (fun _ -> props.onChange())
                        ]
                        Html.text $" {props.text}"
                    ]
                ]
            ]
        ]
    
    Html.div [
        prop.children [
            Html.div [
                prop.className [ Bulma.Field ]
                prop.children [
                    inputCheck {| id = "all"
                                  text = "Toggle All"
                                  isChecked = Table.getIsSomeColumnsVisible state.Table
                                  onChange = fun () -> dispatch AllCheckedChanged |}
                    
                    Html.div [
                        prop.children [
                            for column in Table.getAllLeafColumns state.Table do
                                inputCheck {| id = column.Id
                                              text = column.Id
                                              isChecked = Column.getIsVisible column
                                              onChange = fun () -> ColumnChecked column |> dispatch |}
                        ]
                    ]
                    Html.div [
                        prop.className [ Bulma.Pt2 ]
                        prop.children [
                            Html.button [
                                prop.className [ Bulma.Button ]
                                prop.text "Reset"
                                prop.onClick (fun _ -> dispatch Reset)
                            ]
                            Html.button [
                                prop.className [ Bulma.Button ]
                                prop.text "Randomise"
                                prop.onClick (fun _ -> dispatch Randomise)
                            ]
                        ]
                    ]
                    inputCheck {| id = "split"
                                  text = "Split Mode"
                                  isChecked = state.IsSplitMode
                                  onChange = fun () -> dispatch SplitModeChanged |}
                ]
            ]
            
            table
        ]
    ]
    
[<ReactComponent>]
let Component () =
    let state, dispatch = React.useElmish (init, update)
    view state dispatch