module Examples.Expanding

open System
open Browser.Types
open Elmish
open Fable.Core
open Fable.Core.JsInterop
open MakeData.Types
open MakeData
open Feliz
open Feliz.UseElmish
open Feliz.TanStack.Table

[<Emit("typeof $0 === 'number'")>]
let private isNumber x = jsNative

let firstNameHeader (props : HeaderFnProps<_,_,_>) =
    Html.span [
        prop.children [
            Html.input [
                prop.type' "checkbox"
                Interop.mkAttr "indeterminate" "true"
            ]
            Html.button [
                prop.className [ Bulma.Button; Bulma.IsSmall; Bulma.IsGhost ]
                prop.text (if Table.getIsAllRowsExpanded props.table then "👇" else "👉")
            ]
            Html.text "First Name"
        ]
    ]
    
let firstNameCell (props : CellContextProp<_,_,_>) =
    Html.div [
        prop.style [ style.paddingLeft (length.rem (props.row.depth * 2)) ]
        prop.children [
            Html.input [
                prop.type' "checkbox"
            ]
            if Row.getCanExpand props.row then
                Html.button [
                    prop.className [ Bulma.Button; Bulma.IsSmall; Bulma.IsGhost; ]
                    prop.text (if Row.getIsExpanded props.row then "👇" else "👉")
                ]
            else
                Html.text "🔵"
            Html.text (props.getValue<string>())
        ]
    ]

let defaultColumns : ColumnDefOptionProp<PersonSub> list list = [
    [ columnDef.header "Name"
      columnDef.footer (fun props -> props.column.id)
      columnDef.columns [
          [ columnDef.accessorKey "Firstname"
            columnDef.header firstNameHeader
            columnDef.cell firstNameCell
            columnDef.footer (fun props -> props.column.id) ]
          [ columnDef.accessorFn (fun p -> p.Lastname)
            columnDef.id "Lastname"
            columnDef.cell (fun info -> info.getValue<_>())
            columnDef.header (fun _ -> Html.p "Hi")
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

type FilterChangeType =
    | Min
    | Max
    | Search

type FilterChange = {
    Column : Column<Person>
    Type : FilterChangeType
    Value : string
}

type PaginateChange =
    | First
    | Previous
    | Next
    | Last
    | Index of int

type State = {
    Table : Table<Person>
    EditingData : EditingData option
}

type Msg =
    | CellClicked of rowIndex: int * columnId: string
    | FilterChanged of FilterChange
    | Paginate of PaginateChange
    | PageSizeChange of int

let init () =
    let tableProps = [
        tableProps.data (MakeData.subMake 100 [ 10; 10 ])
        tableProps.columns defaultColumns
        tableProps.getSubRows (fun p -> p.SubRows)
        tableProps.filteredRowModel()
        tableProps.paginationRowModel()
        tableProps.expandedRowModel()
        tableProps.autoResetPageIndex true ]
    
    let table = Table.init<Person> tableProps
    
    { Table = table
      EditingData = None }, Cmd.none

let update (msg: Msg) (state: State) =
    match msg with
    | CellClicked (rowId, columnId) ->
        { state with
            EditingData = Some { RowIndex = rowId; ColumnId = columnId; Value = "" } }, Cmd.none
        
    | FilterChanged change ->
        match change.Type with
        | Search ->
            Column.setFilterValue (fun _ -> change.Value) change.Column |> ignore
        | minmax ->
            match Int32.TryParse change.Value, minmax with
            | (true, n), Max ->
                Column.setFilterValue
                    (fun prevOpt ->
                        match prevOpt with
                        | Some (Some min, _) -> (Some min, Some n)
                        | _ -> (None, Some n))
                    change.Column |> ignore
            | (true, n), Min ->
                Column.setFilterValue
                    (fun prevOpt ->
                        match prevOpt with
                        | Some (_, Some max) -> (Some n, Some max)
                        | _ -> (Some n, None))
                    change.Column |> ignore
            | _ -> ()
            
        { state with Table = state.Table }, Cmd.none
        
    | Paginate paginateChange ->
        let table = 
            match paginateChange with
            | First -> Table.setPageIndex 0 state.Table
            | Previous -> Table.previousPage state.Table
            | Next -> Table.nextPage state.Table
            | Last -> Table.setPageIndex (Table.getPageCount state.Table - 1) state.Table
            | Index i -> Table.setPageIndex i state.Table
            
        { state with Table = table }, Cmd.none
        
    | PageSizeChange pageSize ->
        { state with Table = Table.setPageSize pageSize state.Table }, Cmd.none
        
let view (state: State) (dispatch: Msg -> unit) =
    let filter (column : Column<Person>) (table : Table<Person>) =
        let firstValue =
            (Table.getPreFilteredRowModel table).flatRows
            |> Array.head
            |> Row.getValue column
        
        let columnFilterValue = Column.getFilterValue column
        
        Html.div [
            prop.className [ Bulma.IsFlex ]
            prop.children [
                if isNumber firstValue then
                    Html.input [
                        prop.style [ style.width (length.rem 6) ]
                        prop.placeholder "Min..."
                        prop.value (match columnFilterValue with Some (Some v, _) -> fst v | _ -> "")
                        prop.onChange (fun s ->
                            FilterChanged { Type = Min; Value = s; Column = column } |> dispatch)
                    ]
                    Html.input [
                        prop.style [ style.width (length.rem 6) ]
                        prop.placeholder "Max..."
                        prop.value (match columnFilterValue with Some (_, Some v) -> snd v | _ -> "")
                        prop.onChange (fun s ->
                            FilterChanged { Type = Max; Value = s; Column = column } |> dispatch)
                    ]
                else
                    Html.input [
                        prop.style [ style.width (length.rem 8) ]
                        prop.placeholder "Search..."
                        prop.value (match columnFilterValue with Some v -> string v | None -> "")
                        prop.onChange (fun s ->
                            FilterChanged { Type = Search; Value = s; Column = column } |> dispatch)
                    ]
            ]
        ]
        
    let paginationControls =
        let selectOptions = Map.ofList [
            for i in 10..10..50 do
                i, ($"Show {i}", fun _ -> PageSizeChange i |> dispatch)
        ]
        
        let pageControl (text : string) isDisabled onClick =
            Html.button [
                prop.className [ Bulma.Button ]
                prop.text text
                prop.disabled isDisabled
                prop.onClick onClick
            ]
        
        Html.div [
            prop.className [ Bulma.IsInlineBlock ]
            prop.children [
                pageControl "<<" (Table.hasPreviousPage state.Table |> not) (fun _ -> Paginate First |> dispatch)
                pageControl "<" (Table.hasPreviousPage state.Table |> not) (fun _ -> Paginate First |> dispatch)
                pageControl ">" (Table.hasNextPage state.Table |> not) (fun _ -> Paginate Next |> dispatch)
                pageControl ">>" (Table.hasNextPage state.Table |> not) (fun _ -> Paginate Last |> dispatch)
                Html.p [
                    Html.text "Page "
                    Html.text ((Table.getState state.Table).pagination.pageIndex + 1)
                    Html.text " of "
                    Html.text (Table.getPageCount state.Table)
                    Html.text " | Go to page:"
                ]
                Html.input [
                    prop.className [ Bulma.Input ]
                    prop.type' "number"
                    prop.style [ style.width (length.rem 4) ]
                    prop.defaultValue ((Table.getState state.Table).pagination.pageIndex + 1)
                    prop.onChange (Index >> Paginate >> dispatch)
                ]
                Html.div [
                    prop.className [ Bulma.Select ]
                    prop.children [
                        Html.select [
                            prop.onChange (fun (event : Event) ->
                                let option = int event.target?value
                                snd selectOptions[option]())
                            prop.value $"{(Table.getState state.Table).pagination.pageSize}"
                            prop.children [
                                for option in selectOptions do
                                    Html.option [
                                        prop.key option.Key
                                        prop.value option.Key
                                        prop.text (fst option.Value)
                                    ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    
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
                                         if header.isPlaceholder then Html.none else
                                             Html.flexRender (
                                                 state,
                                                 dispatch,
                                                 header.column.columnDef.header,
                                                 Table.getContext header)
                                         if Column.getCanFilter header.column then 
                                            filter header.column state.Table
                                         else Html.none
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
                                    prop.onClick (fun _ -> CellClicked (cell.row.index, cell.column.id) |> dispatch)
                                    prop.children [
                                        Html.flexRender(
                                            state,
                                            dispatch,
                                            cell.column.columnDef.cell,
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
                paginationControls
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