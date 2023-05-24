namespace Articulus.Rendering

open System
open System.IO
open System.Text
open System.Text.Json
open System.Text.Json.Serialization
open Articulus.Store
open FDOM.Core.Common
open FDOM.Rendering
open Fluff.Core

[<AutoOpen>]
module Impl =

    type FragmentDateFormatConverter(format: string) =
        inherit JsonConverter<DateTime>()
        
        override fdfc.Write(writer: Utf8JsonWriter, date: DateTime, options: JsonSerializerOptions) =
            writer.WriteStringValue(date.ToString(format))
            
        override fdfc.Read(reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            DateTime.ParseExact(reader.GetString(), format, null)
            
    [<CLIMutable>]
    type FragmentData =
        { [<JsonPropertyName("title")>]
          Title: string
          [<JsonPropertyName("items")>]
          Items: FragmentDataItem seq }
        
        member fd.Serialize(dateFormat: string) =
            let jso = JsonSerializerOptions()
            jso.Converters.Add(FragmentDateFormatConverter(dateFormat))
            JsonSerializer.Serialize(fd, jso)

    and [<CLIMutable>] FragmentDataItem =
        { [<JsonPropertyName("title_html")>]
          TitleHtml: string
          [<JsonPropertyName("summary_html")>]
          SummaryHtml: string
          [<JsonPropertyName("link")>]
          Link: string
          [<JsonPropertyName("date")>]
          Date: DateTime }

    let createBlocks (lines: string list) =
        FDOM
            .Core
            .Parsing
            .Parser
            .ParseLines(lines)
            .CreateBlockContent()

    let getContent (block: DOM.BlockContent) =
        match block with
        | DOM.Header headerBlock -> Some headerBlock.Content
        | DOM.Paragraph paragraphBlock -> Some paragraphBlock.Content
        | _ -> None

    let getFragmentDataItem (link: string) (blocks: DOM.BlockContent list) =
        // Take the first header and paragraph.
        match blocks.Length > 2 with
        | true ->
            match getContent blocks[0], getContent blocks[1] with
            | Some titleContent, Some summaryContent ->
                { TitleHtml = Html.renderInlineItems titleContent
                  SummaryHtml = Html.renderInlineItems summaryContent
                  Link = link
                  Date = DateTime.Now }
                |> Ok
            | None, _ -> Error "Could not get title content."
            | _, None -> Error "Could not get summary content"
        | false -> Error ""

    let renderArticle
        (data: Mustache.Data)
        (template: Mustache.Template)
        (outputPath: string)
        (name: string)
        (blocks: DOM.BlockContent list)
        =

        try
            Html.renderBlocksWithTemplate template data blocks
            |> fun r -> File.WriteAllText(Path.Combine(outputPath, $"{name}.html"), r)

            Ok blocks
        with
        | exn -> Error $"Unhandled exception while rendering article `{name}`. Message: {exn.Message}"

    let render
        (store: ArticulusStore)
        (values: (string * Mustache.Value) list)
        (template: Mustache.Template)
        (outputRoot: string)
        (outputDirectory: string)
        (name: string)
        =
        match store.GetLatestArticleVersion(name, false) with
        | Some avr ->
            let outputPath =
                Path.Combine(outputRoot, outputDirectory)

            if Directory.Exists outputPath |> not then
                Directory.CreateDirectory outputPath |> ignore

            let data =
                ({ Values = values @ [] |> Map.ofList
                   Partials = Map.empty }: Mustache.Data)

            let link =
                $"./{outputDirectory}/{name}.html"

            avr.RawBlob.ToBytes()
            |> Encoding.UTF8.GetString
            |> fun r -> r.Split Environment.NewLine
            |> List.ofArray
            |> FDOM.Core.Parsing.Parser.ParseLines
            |> fun p -> p.CreateBlockContent()
            |> renderArticle data template outputPath name
            |> Result.bind (getFragmentDataItem link)
        | None -> Error $"Article `{name}` not found."

    let renderAll
        (store: ArticulusStore)
        (values: (string * Mustache.Value) list)
        (template: Mustache.Template)
        (outputRoot: string)
        (outputDirectory: string)
        =
        store.GetAllArticles()
        |> List.map (fun ar -> render store values template outputRoot outputDirectory ar.Name)
