namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Fable.Core
open Browser.Types
open Feliz.TanStack.Table.Core

[<AutoOpen>]
module Sizing =
    [<Emit("typeof $0 === 'number'")>]
    let private isNumberType x = jsNative
    
    let private calculateColumnSizing (deltaPercentage : float) (old : obj) : (string * obj)[] =
        old?columnSizingStart
        |> Array.map (fun (arr : obj[]) ->
            let c = unbox arr[0]
            let h = unbox arr[1]
            c, round ((max (h + h * deltaPercentage) 0.) * 100.) / 100. |> box)
    
    let private calculateChange (applyChange : bool) (event : Event option) (table : Table<'T>) : Table<'T> =
        table._obj?setOptions(fun prev ->
            let old = prev?state?columnSizingInfo
            
            let processEvent() : (string * obj)[] =
                match event with
                | Some e ->
                    let clientXPos = e?clientX
                    
                    let oldOffset: float = if old?startOffset = null then 0 else old?startOffset
                    let deltaOffset: float = clientXPos - oldOffset
                    let oldSize: float = if old?startSize = null then 0 else old?startSize
                    let deltaPercentage = max (deltaOffset / oldSize) -0.999999
                    
                    let delta = createObj [
                        "deltaOffset" ==> deltaOffset
                        "deltaPercentage" ==> deltaPercentage
                    ]
                    
                    let columnSizing = calculateColumnSizing deltaPercentage old
                    
                    let newState = merge old delta
                    prev?state?columnSizingInfo <- newState
                    columnSizing
                | None ->
                    let deltaPercentage : float = old?deltaPercentage
                    calculateColumnSizing deltaPercentage old
            
            let newColumnSizing = processEvent()
            
            prev?state?columnSizing <-
                if applyChange then
                    merge prev?state?columnSizing (createObj newColumnSizing)
                else prev?state?columnSizing
            setStateChange prev table._obj?options (fun () -> ()) )
        table
    
    type Column =
        static member getCanResize (column : Column<'T>) : bool =
            column._obj?getCanResize()
            
    type Table = 
        static member setColumnSizing (size : int) (column : Column<'T>) (table : Table<'T>) : Table<'T> =
            updateRecordStateProperty
                (fun s -> s?columnVisibility)
                (fun s n -> s?columnVisibility <- n)
                column._obj?id
                size
                table
                
        static member getDeltaOffset (table : Table<'T>) : int =
            table._obj?getState()?columnSizingInfo?deltaOffset
           
        static member setColumnSizingMode (sizingMode : ColumnResizeMode) (table : Table<'T>) : Table<'T> =
            table._obj?setOptions(fun prev ->
                prev?columnResizeMode <- (ColumnResizeMode.toString sizingMode)
                setStateChange prev table._obj?options (fun () -> ()))
            table
           
    type Header =
        static member endResize (table : Table<'T>) : Table<'T> =
            let table = 
                calculateChange
                    (table._obj?options?columnResizeMode = (ColumnResizeMode.toString OnEnd))
                    None
                    table
            
            table._obj?setOptions(fun prev ->
                let options = createObj []
                
                prev?state?columnSizingInfo <- options
                setStateChange prev table._obj?options (fun () -> ()))
            
            table
        
        static member resizeHandler (event : Event) (header : Header<'T>) (table : Table<'T>) : Event -> Table<'T> =
            let column = header.Column
            let startSize = Header.getSize header
            let leafHeaders = Header.getLeafHeaders header
            
            let columnSizingStart =
                if leafHeaders.Length <> 0 then
                    leafHeaders
                    |> Array.map (fun h -> h.Column.Id, Column.getSize h.Column |> box)
                else [| header.Column.Id, Column.getSize header.Column |]
            
            let isTouchStartEvent e =
                !!e?``type`` = "touchstart"
            
            let clientX =
                if isTouchStartEvent event then
                    let x: float = (event?touches |> Array.head)?clientX
                    round x
                else event?clientX
            
            table._obj?setOptions(fun prev ->
                let options = createObj [
                    "startOffset" ==> clientX
                    "startSize" ==> startSize
                    "deltaOffset" ==> 0
                    "deltaPercentage" ==> 0
                    "columnSizingStart" ==> columnSizingStart
                    "isResizingColumn" ==> column.Id
                ]
                
                prev?state?columnSizingInfo <- options
                setStateChange prev table._obj?options (fun () -> ()))
            
            let table =
                updateRecordStateProperty
                    (fun s -> s?columnSizingInfo)
                    (fun s n -> s?columnSizingInfo <- n)
                    column._obj?id
                    startSize
                    table
            
            fun event ->
                if event.cancelable then
                    event.preventDefault()
                    event.stopPropagation()
                
                calculateChange
                    (table._obj?options?columnResizeMode = (ColumnResizeMode.toString OnChange))
                    (Some event)
                    table
        