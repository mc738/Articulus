namespace Articulus.Import

module Metadata =

    open System

    [<RequireQualifiedAccess>]
    type FetchKeyResult<'T> =
        | Found of 'T
        | NotFound
        | ParsingError

        member fkr.ToErrorMessage(key: string) =
            match fkr with
            | Found _ -> None
            | NotFound -> Some $"Key `{key}` not found."
            | ParsingError -> Some $"Key `{key}` could not be parsed."

    type CommonMetadata =
        { Title: string
          Date: DateTime
          Order: int }

    let titleKey = "articulus:title"

    let dateKey = "articulus:created_on"

    let orderKey = "articulus:order"

    let getTitle (md: Map<string, string>) =
        md.TryFind titleKey
        |> Option.map FetchKeyResult.Found
        |> Option.defaultValue FetchKeyResult.NotFound

    let getDate (md: Map<string, string>) =
        match md.TryFind dateKey with
        | Some dv ->
            match DateTime.TryParse dv with
            | true, v -> FetchKeyResult.Found v
            | false, _ -> FetchKeyResult.ParsingError
        | None -> FetchKeyResult.NotFound

    let getOrder (md: Map<string, string>) =
        match md.TryFind orderKey with
        | Some ov ->
            match Int32.TryParse ov with
            | true, v -> FetchKeyResult.Found v
            | false, _ -> FetchKeyResult.ParsingError
        | None -> FetchKeyResult.NotFound

    let getCommonMetadata (md: Map<string, string>) =
        match getTitle md, getDate md, getOrder md with
        | FetchKeyResult.Found title, FetchKeyResult.Found date, FetchKeyResult.Found order ->
            { Title = title
              Date = date
              Order = order }
            |> Ok
        | title, date, order ->
            [ title.ToErrorMessage(titleKey)
              date.ToErrorMessage(dateKey)
              order.ToErrorMessage(orderKey) ]
            |> List.choose id
            |> Error