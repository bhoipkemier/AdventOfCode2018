module Day1

open System.Collections.Generic

let private data = DayCode.getData ("Day1_0.txt")

let private numbers = data |> Seq.map(int)

let private sum = fun acc elem -> acc + elem

let Program1 () = numbers |> Seq.reduce(sum) |> string

let Program2 () = 
    let mutable currentFrequency = 0
    let seenFrequencies = new HashSet<int>()

    while seenFrequencies.Contains currentFrequency |> not do
        for number in numbers do
            if (seenFrequencies.Contains currentFrequency |> not) then
                seenFrequencies.Add currentFrequency |> ignore
                currentFrequency <- currentFrequency + number
    currentFrequency |> string
