module App

open Fable.Core.JsInterop
open Elmish
open Elmish.React

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

importSideEffects "./styles/styles.css"

Program.mkProgram Main.init Main.update Main.view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "app"
|> Program.run