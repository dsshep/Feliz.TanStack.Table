module App

open Elmish
open Elmish.React
open Elmish.HMR

Program.mkProgram Main.init Main.update Main.view
|> Program.withReactSynchronous "app"
|> Program.run
