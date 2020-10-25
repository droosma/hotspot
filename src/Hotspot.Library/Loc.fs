namespace Hotspot

module Loc =
    
    open System
    open System.IO

    type LineStats = {
        Ext : string
        Lines : int
        LoC : int
        CommentLines : int
    }

    type LineType = | Comment | Code | Empty

    let inspectLine (line : string) = 
        let mutable t = Empty
        let mutable prevWasSlash = false
        for c in line do
            if t = Empty && Char.IsWhiteSpace c then 
                prevWasSlash <- false
                ignore()
            elif t = Empty && c = '/' then
                if prevWasSlash then 
                    t <- Comment
                else prevWasSlash <- true
            else t <- Code
        t


    let getStats filePath =
        let lineTypes = File.ReadLines(filePath) |> Seq.map (inspectLine) |> Seq.toList
        {
            Ext = FileInfo(filePath).Extension
            Lines = lineTypes |> List.length
            LoC = lineTypes |> List.filter (fun x -> x = Code) |> List.length
            CommentLines = lineTypes |> List.filter (fun x -> x = Comment) |> List.length
        }