namespace MakeData

module Types =
    
    type Person = {
        Firstname: string
        Lastname: string
        Age: int
        Visits: int
        Status: string
        Progress: int
    }

    type PersonSub = {
        Firstname: string
        Lastname: string
        Age: int
        Visits: int  
        Status: string
        Progress: int
        SubRows: PersonSub[]
    }


[<RequireQualifiedAccess>]
module MakeData =
    open Types
    
    let private statuses = [| "relationship"; "complicated"; "single" |]

    let make (count : int) : Person[] =
        [| for _ in 1..count do
             { Firstname = Faker.Name.FirstName()
               Lastname = Faker.Name.LastName()
               Age = Faker.DataType.Number(40)
               Visits = Faker.DataType.Number(1000)
               Progress = Faker.DataType.Number(100)
               Status = (Faker.Helpers.Shuffle statuses)[0] } |]
        
    let rec subMake (count : int) (subCounts : int list) = 
        match subCounts with
        | head::tail ->
            [| for _ in 1..count do
                 { Firstname = Faker.Name.FirstName()
                   Lastname = Faker.Name.LastName()
                   Age = Faker.DataType.Number(40)
                   Visits = Faker.DataType.Number(1000)
                   Progress = Faker.DataType.Number(100)
                   Status = (Faker.Helpers.Shuffle statuses)[0]
                   SubRows = subMake head tail } |]
        | [] ->
            [| for _ in 1..count do
                 { Firstname = Faker.Name.FirstName()
                   Lastname = Faker.Name.LastName()
                   Age = Faker.DataType.Number(40)
                   Visits = Faker.DataType.Number(1000)
                   Progress = Faker.DataType.Number(100)
                   Status = (Faker.Helpers.Shuffle statuses)[0]
                   SubRows = [||] } |]
        
    
          
      
      
      
      