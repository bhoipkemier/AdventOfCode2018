module Day2

open System.Collections.Generic
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Collections

let private appearsExactly (times: int) (id: seq<char>) = 
    let groups = Seq.groupBy (fun idChar -> idChar) id
    let matchingGroups = Seq.filter(fun group -> ((snd group |> Seq.length) = times)) groups
    matchingGroups |> Seq.length > 0

let private appearsTwice = 2 |> appearsExactly
let private appearsThrice = 3 |> appearsExactly

let private data = DayCode.getData ("Day2_0.txt")


let Program1 () = 
    let twoSetCount = data |> Seq.filter(appearsTwice) |> Seq.length
    let threeSetCount = data |> Seq.filter(appearsThrice) |> Seq.length
    twoSetCount * threeSetCount |> string


let Program2 () = ""
