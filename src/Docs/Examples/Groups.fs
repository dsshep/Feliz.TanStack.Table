module Examples.Groups

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
          [ columnDef.accessorKey "Lastname"
            columnDef.id "Lastname"
            columnDef.cell (fun info -> info.getValue<_>())
            columnDef.header (fun _ -> Html.span [ prop.text "Last Name" ])
            columnDef.footer (fun props -> props.column.id) ]
      ] ]
    [ columnDef.header "info"
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
}

type Msg =
    | Nop
    
let init () =
    let tableProps = [
        tableProps.data defaultData
        tableProps.columns defaultColumns ]
    
    let table = Table.init<Person> tableProps
    
    { Table = table }, Cmd.none

let update (msg: Msg) (state: State) =
    state, Cmd.none
    
let view (state: State) (dispatch: Msg -> unit) =
    let table = 
        let thead =
            Html.thead [
                for headerGroup in Table.getHeaderGroups state.Table do
                     Html.tr [
                         prop.key headerGroup.id
                         prop.children [
                             for header in headerGroup.headers do
                                 Html.th [
                                     prop.key header.id
                                     prop.colSpan header.colSpan
                                     prop.children [
                                         if header.isPlaceholder then
                                             Html.none
                                         else 
                                             Html.flexRender (
                                                 header.column.columnDef.header,
                                                 Table.getContext header)
                                     ]
                                 ]
                         ]
                     ]
            ]
            
        let tbody =
            Html.tbody [
                for row in (Table.getRowModel state.Table).rows do
                    Html.tr [
                        prop.key row.id
                        prop.children [
                            for cell in Table.getVisibleCells row do
                                Html.td [
                                    Html.flexRender(
                                        cell.column.columnDef.cell,
                                        Table.getContext cell)
                                ]
                        ]
                    ]
            ]
            
        let tfoot =
            Html.tfoot [
                for footerGroup in Table.getFooterGroups state.Table do
                    Html.tr [
                        prop.key footerGroup.id
                        prop.children [
                            for footer in footerGroup.headers do
                                Html.th [
                                    prop.key footer.id
                                    prop.colSpan footer.colSpan
                                    prop.children [
                                        if footer.isPlaceholder then
                                             Html.none
                                        else 
                                            Html.flexRender (
                                                footer.column.columnDef.header,
                                                Table.getContext footer)
                                    ]
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

    table
    
[<ReactComponent>]
let Component() =
    let state, dispatch = React.useElmish (init, update)
    view state dispatch