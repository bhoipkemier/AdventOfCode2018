// Learn more about F# at http://fsharp.org

open System
open DayCode
open Day2

[<EntryPoint>]
let main argv =
    printfn "%s" (Program1 ())
    printfn "======================================="
    printfn "%s" (Program2 ())
    Console.ReadLine() |> ignore
    0 // return an integer exit code
