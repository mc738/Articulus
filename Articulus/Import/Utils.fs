namespace Articulus.Import

[<AutoOpen>]
module Utils =
    
    open System.IO
    
    let readLines (path: string) =
        try
            File.ReadAllLines path |> List.ofArray |> Ok
        with
        | exn -> Error exn.Message
        
    let groupResults<'T> (results: Result<unit, 'T> list) =
        results
        |> List.choose (fun r -> match r with Ok _ -> None | Error e -> Some e)
        |> fun r ->
            match r.IsEmpty with
            | true -> Ok ()
            | false -> Error r
