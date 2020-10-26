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

type Account = 
   { IsOpen : bool; Balance: decimal }

let OpenAccount account =
    { account with IsOpen = true }

let IncreaseBalance account transaction =
    { account with Balance = account.Balance + transaction.Amount }

let DecreaseBalance account transaction =
    { account with Balance = account.Balance - transaction.Amount }

let CloseBankAccount account =
    { account with IsOpen = false }

let Transform account transaction =
    match transaction.Command.ToLowerInvariant() with
    | "open bank account" -> OpenAccount account
    | "deposit cash" -> IncreaseBalance account transaction
    | "emit wire transfer" -> DecreaseBalance account transaction
    | "withdraw cash" -> DecreaseBalance account transaction
    | "receive wire transfer" -> IncreaseBalance account transaction
    | "pay with bank card" -> DecreaseBalance account transaction
    | "close bank account" -> CloseBankAccount account
    | _ -> raise (InfraErrror("Command format is out of whack!"))

[<EntryPoint>]
let main argv =
    let path = "test.json";

    let json = System.IO.File.ReadAllText path

    let transactions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Transaction>>(json)

    let transformer = fun state transaction -> Transform state transaction
    let account = transactions |> List.fold transformer { IsOpen = false; Balance = 0M; } 

    printfn "%A" account

    0
