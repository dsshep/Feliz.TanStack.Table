[<RequireQualifiedAccess>]
module Faker

open Fable.Core.JsInterop

let private faker : obj = import "faker" "@faker-js/faker"

type Name =
    static member FirstName() : string = faker?name?firstName()
    static member LastName() : string = faker?name?lastName()

type DataType =
    static member Number (max : int) : int = faker?datatype?number(max)

type Helpers =
    static member Shuffle<'T> (seq : seq<'T>) : 'T[] = faker?helpers?shuffle(seq)
