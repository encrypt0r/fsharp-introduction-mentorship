module FSharp.Introduction.Mentorship.Session1

// Most common issues of consuming C# code in F#:
// - no protected access modifier(s)
// - intertwined generic constraints
// - first call site resolution involving an early parameter types inference

[<Literal>]
let a = 42
let b: int32 = 42
let c: int64 = 42L
let d = "sdfd"

[<Struct>]
type MyRecord =
    { A: int32
      B: string }

type MyDu =
    | A of string
    | B of int32
    | C of MyDu
    | D of MyRecord

let processMyDu myDu =
    match myDu with
    | A str -> ()
    | B i -> ()
    | _ -> ()



// = same symbol for both:
// - C# == equality operator
// - F# let binding => assigning to weve

// Structural Equality, by default:
// - Records
// - Discriminated Unions
// - Tuples
// Not only there SE, but there is also immutability!

// Partial Application and Currying
let add3 a b c = a + b + c
let add2 a b = a + b
let add1 a = add2 a 1
let add1v2 = add2 1

let op (a: int32) (b: string) = (string a) + b
let op1 = op 1



let twoRecordsAreEqual = { A = 42; B = "sdf" } = { A = 42; B = "sdf" }

type Michelle<'T> = (string * 'T)

let michelle : Michelle<int32> = ("hello", 42)

let f1 a = a

let f2 x y = x.ToString() + y

let myFunc1 a b = a + b
let myFunc2 a b = a * b

let e = "strg" |> f2 42

// Domain errors => Result
// Infra errors => exceptions bubbling up
