module Tests

open Fable.Mocha

let myFirstTest =
    testList "Easy tests" [
        testCase "I can write and run a test..." <| fun () ->
            Expect.equal (1 + 1) 2 "Simple test"
            
        testCase "Another test" <| fun () ->
            
            ()
    ]
    
Mocha.runTests myFirstTest
