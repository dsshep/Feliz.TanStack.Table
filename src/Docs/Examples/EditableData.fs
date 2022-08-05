module Examples.EditableData

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

let makeData (count : int) =
    let statuses = [| "relationship"; "complicated"; "single" |]
    [| for _ in 0..count do
           { Firstname = Faker.Name.FirstName()
             Lastname = Faker.Name.LastName()
             Age = Faker.DataType.Number(40)
             Visits = Faker.DataType.Number(1000)
             Progress = Faker.DataType.Number(100)
             Status = (Faker.Helpers.Shuffle statuses)[0] } |]

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

type EditingData = {
    RowIndex : int
    ColumnId : string
    Value : string
}

type PropertyChange = {
    EditingData : EditingData
    NewValue : string
}

type State = {
    Table : Table<Person>
    EditingData : EditingData option
}

type Msg =
    | CellClicked of rowIndex: int * columnId: string
    | PropertyChanged of PropertyChange
    | FinishedEditing
    
let private rand = Random()    

let init () =
    let tableProps = [
        tableProps.data (makeData 1000)
        tableProps.columns defaultColumns
        tableProps.filteredRowModel()
        tableProps.paginationRowModel() ]
    
    let table = Table.init<Person> tableProps
    
    { Table = table
      EditingData = None }, Cmd.none

let update (msg: Msg) (state: State) =
    match msg with
    | CellClicked (rowId, columnId) ->
        { state with
            EditingData = Some { RowIndex = rowId; ColumnId = columnId; Value = "" } }, Cmd.none
        
    | PropertyChanged propertyChange ->
        let editingPerson = state.Table.Data[propertyChange.EditingData.RowIndex]
        let newValue = propertyChange.NewValue
        
        let updated, inputValue = 
            match propertyChange.EditingData.ColumnId with
            | x when x = nameof(editingPerson.Firstname) -> { editingPerson with Firstname = newValue }, newValue
            | x when x = nameof(editingPerson.Lastname) -> { editingPerson with Lastname = newValue }, newValue
            | x when x = nameof(editingPerson.Status) -> { editingPerson with Status = newValue }, newValue
            | x when x = nameof(editingPerson.Age) ->
                match Int32.TryParse(newValue) with
                | true, i -> { editingPerson with Age = i }, newValue
                | _ -> editingPerson, propertyChange.EditingData.Value
            | x when x = nameof(editingPerson.Visits) ->
                match Int32.TryParse(newValue) with
                | true, i -> { editingPerson with Visits = i }, newValue
                | _ -> editingPerson, propertyChange.EditingData.Value
            | x when x = nameof(editingPerson.Progress) ->
                match Int32.TryParse(newValue) with
                | true, i -> { editingPerson with Progress = i }, newValue
                | _ -> editingPerson, propertyChange.EditingData.Value
            | _ -> editingPerson, propertyChange.EditingData.Value
        
        let table =
            state.Table.Data
            |> Array.mapi (fun i d -> if i = propertyChange.EditingData.RowIndex then updated else d)
            |> Table.setData state.Table
        
        { state with
            Table = table
            EditingData = Some { propertyChange.EditingData with Value = inputValue } }, Cmd.none
        
    | FinishedEditing ->
        { state with EditingData = None }, Cmd.none
        
let view (state: State) (dispatch: Msg -> unit) =
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
                                     prop.style [
                                         style.width (Header.getSize header)
                                         position.relative
                                     ]
                                     prop.children [
                                         Html.flexRender (
                                             header.IsPlaceholder,
                                             header.Column.ColumnDef.Header,
                                             Table.getContext header)
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
                                    prop.onClick (fun _ -> CellClicked (cell.Row.Index, cell.Column.Id) |> dispatch)
                                    prop.children [
                                        match state.EditingData with
                                        | Some editingData when cell.Column.Id = editingData.ColumnId
                                                                && cell.Row.Index = editingData.RowIndex ->
                                            Html.input [
                                                prop.onChange (fun t ->
                                                    PropertyChanged { EditingData = editingData
                                                                      NewValue = t } |> dispatch)
                                                prop.onBlur (fun _ -> FinishedEditing |> dispatch)
                                                prop.autoFocus true
                                                prop.value editingData.Value
                                            ]
                                        | _ ->
                                            Html.flexRender(
                                                cell.Column.ColumnDef.Cell,
                                                Table.getContext cell)
                                    ]
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
            table
        ]
    ]
    
[<ReactComponent>]
let Component () =
    let state, dispatch = React.useElmish (init, update)
    view state dispatch