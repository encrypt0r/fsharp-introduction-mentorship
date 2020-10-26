// Learn more about F# at http://fsharp.org


// Version 1
//open System

//exception InfraErrror of string

//type Transaction = 
//   { Command : string; Amount: decimal; Label: string }

//type Account = 
//   { IsOpen : bool; Balance: decimal }

//let OpenAccount account =
//    { account with IsOpen = true }

//let IncreaseBalance account transaction =
//    { account with Balance = account.Balance + transaction.Amount }

//let DecreaseBalance account transaction =
//    { account with Balance = account.Balance - transaction.Amount }

//let CloseBankAccount account =
//    { account with IsOpen = false }

//// [<EntryPoint>]
//let main argv =
//    let path = "test.json";

//    let json = System.IO.File.ReadAllText path

//    let transactions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Transaction>>(json)

//    let mutable account = { IsOpen = false; Balance = 0M; }

//    for transaction in transactions do
//        account <- match transaction.Command.ToLowerInvariant() with
//            | "open bank account" -> OpenAccount account
//            | "deposit cash" -> IncreaseBalance account transaction
//            | "emit wire transfer" -> DecreaseBalance account transaction
//            | "withdraw cash" -> DecreaseBalance account transaction
//            | "receive wire transfer" -> IncreaseBalance account transaction
//            | "pay with bank card" -> DecreaseBalance account transaction
//            | "close bank account" -> CloseBankAccount account
//            | _ -> raise (InfraErrror("Command out of whack!"))

//    printfn "%A" account

//    0



// Version 2: More functional 🕺

open System

exception InfraErrror of string

type Transaction = 
   { Command : string; Amount: decimal; Label: string }

type AccountState =
    | Open
    | Closed
    | None

type Account = 
   { IsOpen : bool; Balance: decimal }

let OpenAccount account =
    match account.IsOpen with
    | false -> Ok { account with IsOpen = true }
    | true -> Error "Account is already open!"

let IncreaseBalance account transaction =
    Ok { account with Balance = account.Balance + transaction.Amount }

let DecreaseBalance account transaction =
    // Question: How can this be converted to a match expression?
    if account.Balance < transaction.Amount then
        Error "Account balance can't be negative!"
    else
        Ok { account with Balance = account.Balance - transaction.Amount }

let CloseBankAccount account =
    match account.IsOpen with
    | true -> Ok { account with IsOpen = false }
    | false -> Error "Account is already closed!"

let Transform state transaction =
    match state with
    | Ok account ->
        match transaction.Command.ToLowerInvariant() with
        | "open bank account" -> OpenAccount account
        | "deposit cash" -> IncreaseBalance account transaction
        | "emit wire transfer" -> DecreaseBalance account transaction
        | "withdraw cash" -> DecreaseBalance account transaction
        | "receive wire transfer" -> IncreaseBalance account transaction
        | "pay with bank card" -> DecreaseBalance account transaction
        | "close bank account" -> CloseBankAccount account
        | _ -> raise (InfraErrror("Command format is out of whack!"))
    | Error e -> Error e

[<EntryPoint>]
let main argv =
    let path = "test.json";

    let json = System.IO.File.ReadAllText path

    let transactions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Transaction>>(json)

    let initial = Ok { IsOpen = false; Balance = 0M; }
    let transformer = fun state transaction -> Transform state transaction

    try
        let result = transactions |> List.fold transformer initial

        match result with
        | Ok account -> printfn "Cool, here is your account state: %A" account
        | Error message -> printfn "Domain Error: %A" message
    with
    | InfraErrror e -> printfn "Infra Error: %A" e

    0