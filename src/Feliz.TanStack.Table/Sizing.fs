namespace Feliz.TanStack.Table

open Browser.Dom
open Fable.Core.JsInterop
open Fable.Core
open Browser.Types
open Feliz.TanStack.Table.Core

[<AutoOpen>]
module Sizing =
    [<Emit("typeof $0 === 'number'")>]
    let private isNumberType x = jsNative
    
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
           
    type Header =
        static member resizeHandler (header : Header<'T>) (table : Table<'T>) : Event -> Table<'T> =
            fun (event : Event) ->
                Header.resize event header table
        
        static member resizeHandler2 (event : Event) (header : Header<'T>) (table : Table<'T>) : Event -> Table<'T> =
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
            
            let table =
                updateRecordStateProperty
                    (fun s -> s?columnSizing)
                    (fun s n -> s?columnSizing <- n)
                    column._obj?id
                    startSize
                    table
                    
            table._obj?setOptions(fun prev ->
                let options = createObj [
                    "startOffset" ==> clientX
                    "startSize" ==> startSize
                    "deltaOffset" ==> 0
                    "deltaPercentage" ==> 0
                    "columnSizingStart" ==> columnSizingStart
                    "isResizingColumn" ==> column.Id
                ]
                
                let updateState =
                    createObj [ "columSizingInfo" ==> options ]
                    |> merge prev
                    
                setStateChange updateState table._obj?options (fun _ -> ()))

            fun event ->
                if event.cancelable then
                    event.preventDefault()
                    event.stopPropagation()
                
                let newColumnSizing = ResizeArray<string * obj>()
                
                table._obj?setOptions(fun prev ->
                    let clientXPos = event?clientX
                    let old = prev?state?columnSizingInfo
                    
                    let oldOffset: float = if old?startOffset = null then 0 else old?startOffset
                    let deltaOffset: float = clientXPos - oldOffset
                    let oldSize: float = if old?startSize = null then 0 else old?startSize
                    let deltaPercentage = max (deltaOffset / oldSize) -0.999999
                    
                    let delta = createObj [
                        "deltaOffset" ==> deltaOffset
                        "deltaPercentage" ==> deltaPercentage
                    ]
                    
                    old?columnSizingStart
                    |> Array.iter (fun (arr : obj[]) ->
                        let c = unbox arr[0]
                        let h = unbox arr[1]
                        newColumnSizing.Add (c, round ((max (h + h * deltaPercentage) 0.) * 100.) / 100. |> box))
                    
                    let newState = merge (prev?state?columnSizingInfo) delta
                    prev?state?columnSizingInfo <- newState
                    prev?state?columnSizing <- merge prev?state?columnSizing (createObj newColumnSizing)
                    setStateChange prev table._obj?options (fun () -> ()))
                
                table
                    
        
        static member resize (event : Event) (header : Header<'T>) (table : Table<'T>) : Table<'T> =
            let isTouchStartEvent e =
                !!e?``type`` = "touchstart"
            
            let isMultiTouch e =
                isTouchStartEvent e && e?touches?length > 1
            
            let column = header.Column
            
            if Column.getCanResize column |> not then table
            elif isMultiTouch event then table
            else 
            
            if event?persist <> null then
                event?persist()
            
            let startSize = Header.getSize header
            let leafHeaders = Header.getLeafHeaders header
            
            let columnSizingStart =
                if leafHeaders.Length <> 0 then
                    leafHeaders
                    |> Array.map (fun h -> h.Column.Id, Column.getSize h.Column |> box)
                else [| header.Column.Id, Column.getSize header.Column |]
            
            let clientX =
                if isTouchStartEvent event then
                    let x: float = (event?touches |> Array.head)?clientX
                    round x
                else event?clientX
                
            let updateOffset (eventType : string) (clientXPos : float) =
                if isNumberType clientXPos |> not then ()
                else
                    let newColumnSizing = ResizeArray<string * obj>()
                    table._obj?setColumnSizingInfo(fun old ->
                        let oldOffset: float = if old?startOffset = null then 0 else old?startOffset
                        let deltaOffset: float = clientXPos - oldOffset
                        let oldSize: float = if old?startSize = null then 0 else old?startSize
                        let deltaPercentage = max (deltaOffset / oldSize) -0.999999
                        
                        old?columnSizingStart
                        |> Array.iter (fun (arr : obj[]) ->
                            let c = unbox arr[0]
                            let h = unbox arr[1]
                            newColumnSizing.Add (c, round ((max (h + h * deltaPercentage) 0.) * 100.) / 100. |> box))
                        
                        let delta = createObj [
                            "deltaOffset" ==> deltaOffset
                            "deltaPercentage" ==> deltaPercentage
                        ]
                        JS.debugger()
                        merge old delta)
                    if table._obj?options?columnResizeMode = "onChange" || eventType = "onEnd" then
                        table._obj?setColumnSizing(fun old ->
                            JS.debugger()
                            let newSizing = createObj newColumnSizing 
                            merge old newSizing)
            
            let onMove clientXPos =
                updateOffset "move" clientXPos
                
            let onEnd clientXPos =
                updateOffset "end" clientXPos
                
                table._obj?setColumnSizingInfo(fun old ->
                    let options = createObj [
                        "isResizingColumn" ==> false
                        "startOffset" ==> null
                        "startSize" ==> null
                        "deltaOffset" ==> null
                        "deltaPercentage" ==> null
                        "columnSizingStart" ==> [||]
                    ]
                    merge old options)
                
            let mutable mouseEvents : {| moveHandler: MouseEvent -> unit
                                         upHandler: MouseEvent -> unit |} = Unchecked.defaultof<_>
            
            mouseEvents <-
                {| moveHandler = fun (e : MouseEvent) -> onMove(e.clientX)
                   upHandler = fun (e : MouseEvent) ->
                       onEnd(e.clientX) |}
            
            let mutable touchEvents : {| moveHandler: TouchEvent -> bool
                                         upHandler: TouchEvent -> unit |} = Unchecked.defaultof<_> 
            
            touchEvents <-
                {| moveHandler = fun (e : TouchEvent) ->
                       if e.cancelable then
                           e.preventDefault()
                           e.stopPropagation()
                       onMove(e.touches[0].clientX)
                       false
                   upHandler = fun (e : TouchEvent) ->
                       if e.cancelable then
                           e.preventDefault()
                           e.stopPropagation()
                       onEnd(e.touches[0].clientX) |}
            
            if isTouchStartEvent event |> not then
                mouseEvents.moveHandler (event :?> _)
                
            table._obj?setColumnSizingInfo(fun old ->
                let sizingStart = createObj columnSizingStart
                JS.debugger()
                let options = createObj [
                    "startOffset" ==> clientX
                    "startSize" ==> startSize
                    "deltaOffset" ==> 0
                    "deltaPercentage" ==> 0
                    "columnSizingStart" ==> sizingStart
                    "isResizingColumn" ==> column.Id
                ]
                merge old options)
                
            table
            
        