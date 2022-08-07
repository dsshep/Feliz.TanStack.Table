[<RequireQualifiedAccess>]
module MakeData

type Person = {
  Firstname: string
  Lastname: string
  Age: int
  Visits: int
  Status: string
  Progress: int
}

let make (count : int) : Person[] =
    let statuses = [| "relationship"; "complicated"; "single" |]
    [| for _ in 1..count do
         { Firstname = Faker.Name.FirstName()
           Lastname = Faker.Name.LastName()
           Age = Faker.DataType.Number(40)
           Visits = Faker.DataType.Number(1000)
           Progress = Faker.DataType.Number(100)
           Status = (Faker.Helpers.Shuffle statuses)[0] } |]