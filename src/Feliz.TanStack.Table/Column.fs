namespace Feliz.TanStack.Table

open Fable.Core.JsInterop
open Core

type ColumnPinningPosition =
    | Left
    | Right
    | Neither

[<AutoOpen>]
module Column = 

    type Column =
        
        static member getIsVisible (column : Column<'T>) : bool =
            column._obj?getIsVisible()

        static member getCanPin (column : Column<'T>) : bool =
            column._obj?getCanPin()
        
        static member getIsPinned (column : Column<'T>) : ColumnPinningPosition =
            let isPinned : string = column._obj?getIsPinned()
            let state =
                match isPinned with
                | "left" -> Left
                | "right" -> Right
                | _ -> Neither
            state
            
        static member getIsResizing (column : Column<'T>) : bool =
            column._obj?getIsResizing()
    
        static member getSize (column : Column<'T>) : int =
            column._obj?getSize()
            
    type Table =
        
        static member pinColumn (position : ColumnPinningPosition) (column : Column<'T>) (table : Table<'T>) : Table<'T> =
            let pinnedColumns : string[] =
                column._obj?getLeafColumns()
                |> Array.map (fun c -> c?id)
                |> Array.filter (fun c -> c <> null)
                
            table._obj?setOptions(fun prev ->
                let existingPinning : {| left: string[]; right: string[] |} =
                    prev?state?columnPinning
                    
                let existingPinning =
                    {| existingPinning with
                        left = if existingPinning.left = null then [||] else existingPinning.left
                        right = if existingPinning.right = null then [||] else existingPinning.right |}
                    
                let columnPinningState =
                    match position with
                    | Left ->
                        {| existingPinning
                            with left = existingPinning.left
                                        |> Array.filter (fun c -> pinnedColumns |> Array.contains c |> not)
                                        |> Array.append pinnedColumns
                                 right = existingPinning.right
                                         |> Array.filter (fun c -> pinnedColumns |> Array.contains c |> not) |}
                    | Right ->
                        {| existingPinning
                            with left = existingPinning.left
                                        |> Array.filter (fun c -> pinnedColumns |> Array.contains c |> not)
                                 right = existingPinning.right
                                        |> Array.filter (fun c -> pinnedColumns |> Array.contains c |> not)
                                        |> Array.append pinnedColumns |}
                    | Neither ->
                        {| existingPinning
                            with left = existingPinning.left
                                        |> Array.filter (fun c -> pinnedColumns |> Array.contains c |> not)
                                 right = existingPinning.right
                                        |> Array.filter (fun c -> pinnedColumns |> Array.contains c |> not)|}
                
                prev?state?columnPinning <- columnPinningState
                setStateChange prev table._obj?options (fun () -> ()))
            
            table
        
