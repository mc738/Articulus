namespace Articulus

open System.IO


module Import =

    [<AutoOpen>]
    module private Utils =

        let handleValidationResults (results: Result<unit, string> list) =
            results
            |> List.choose (fun r -> match r with Ok _ -> None | Error e -> Some e)
            |> fun r ->
                match r.IsEmpty with
                | true -> Ok ()
                | false -> Error r
             
        
        let readLines (path: string) =
            try
                File.ReadAllLines path |> List.ofArray |> Ok
            with
            | exn -> Error exn.Message

    type ArticulusFile =
        { Name: string
          Metadata: Map<string, string>
          Lines: string list }

        member af.Validate() =
            [ match af.Metadata.TryFind "" with
              | Some _ -> Ok()
              | None -> Error ""
              match af.Metadata.TryFind "" with
              | Some _ -> Ok()
              | None -> Error ""
              match af.Metadata.TryFind "" with
              | Some _ -> Ok()
              | None -> Error ""
              match af.Metadata.TryFind "" with
              | Some _ -> Ok()
              | None -> Error "" ]
            |> handleValidationResults 

    let scanDirectory (path: string) = Directory.EnumerateFiles(path, "*.md")

    let readFile (path: string) =
        readLines path
        |> Result.map FDOM.Core.Parsing.Parser.ExtractMetadata
        |> Result.map (fun (md, ls) -> { Name = Path.GetFileNameWithoutExtension path; Metadata = md; Lines = ls })
