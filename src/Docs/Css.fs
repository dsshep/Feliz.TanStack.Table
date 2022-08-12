[<AutoOpen>]
module Css

open Zanaptak.TypedCssClasses

type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.4/css/bulma.min.css", Naming.PascalCase>
type Local = CssClasses<"./styles/styles.css", Naming.PascalCase>
