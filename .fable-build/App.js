import { ProgramModule_run } from "./fable_modules/Fable.Elmish.3.1.0/program.fs.js";
import { Program_withReactSynchronous } from "./fable_modules/Fable.Elmish.React.3.0.1/react.fs.js";
import { ProgramModule_mkProgram } from "./fable_modules/Fable.Elmish.3.1.0/program.fs.js";
import { view, update, init } from "./Main.js";

ProgramModule_run(Program_withReactSynchronous("app-bootstrap", ProgramModule_mkProgram(init, update, view)));

