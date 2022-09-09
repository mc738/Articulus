namespace Articulus.Import

open System
open Articulus.Store


[<AutoOpen>]
module Impl =
    
    open System.IO

    type ArticulusFile =
        { Name: string
          Metadata: Map<string, string>
          Title: string
          Date: DateTime
          Order: int
          Lines: string list }

    [<RequireQualifiedAccess>]
    type ImportResult =
        | ArticleAdded of Name: string
        | VersionAdded of Name: string
        | Skipped of Message: string
        | Failure of Message: string

    let scanDirectory (path: string) =
        Directory.EnumerateFiles(path, "*.md")
        |> List.ofSeq

    let readFile (path: string) =
        readLines path
        |> Result.map FDOM.Core.Parsing.Parser.ExtractMetadata
        |> Result.bind (fun (md, ls) ->

            match Metadata.getCommonMetadata md with
            | Ok cmd ->
                { Name = Path.GetFileNameWithoutExtension path
                  Metadata = md
                  Title = cmd.Title
                  Date = cmd.Date
                  Order = cmd.Order
                  Lines = ls }
                |> Ok
            | Error es ->
                let eMsg = es |> String.concat " "
                Error $"The following errors occured while fetching metadata: {eMsg}")

    let printResult (result: ImportResult) =
        match result with
        | ImportResult.ArticleAdded name -> printfn $"Article `{name}` added"
        | ImportResult.VersionAdded name -> printfn $"Article `{name}` version added"
        | ImportResult.Skipped message -> printfn $"Page skipped. Reason: {message}"
        | ImportResult.Failure message -> printfn $"Page import failed. Error: {message}"

    let importFile (store: ArticulusStore) (file: ArticulusFile) =
        let (result, isNew) =
            match store.GetArticle file.Name with
            | Some ar ->
                // TODO check for updates to title, date and ordered.
                store.AddArticleVersion(file.Name, false, file.Lines), false
            | None ->
                // Add new
                store.AddArticle(file.Name, file.Title, file.Order, file.Date)
                store.AddArticleVersion(file.Name, false, file.Lines), true

        match result, isNew with
        | AddArticleVersionResult.Added, true -> ImportResult.ArticleAdded file.Name
        | AddArticleVersionResult.Added, false -> ImportResult.VersionAdded file.Name
        | AddArticleVersionResult.NoChange, _ -> ImportResult.Skipped "No changes."
        | AddArticleVersionResult.ArticleNotFound, _ -> ImportResult.Failure "Article not found."

    let importFiles (store: ArticulusStore) (resultHandler: ImportResult -> unit) (path: string) =
        scanDirectory path
        |> List.iter (fun p ->
            match readFile p with
            | Ok af -> importFile store af
            | Error e -> ImportResult.Failure $"Failed load file `{p}`. {e}"
            |> resultHandler)