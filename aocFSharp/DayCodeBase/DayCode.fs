module DayCode

open System.IO

let Program1 () = ""

let Program2 () = ""

let getData (filePath:string) = seq {
    use sr = new StreamReader ("Data\\" + filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}
