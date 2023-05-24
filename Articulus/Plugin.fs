namespace Articulus

open System.Text.Json
open FsToolbox.Core

module Plugin =

    open System.Text.Json.Serialization

    [<CLIMutable>]
    type ArticulusConfiguration =
        { StorePath: string
          TemplatePath: string
          DataPath: string
          OutputPath: string
          OutputDirectory: string
          IndexPageTemplatePath: string
          IndexPageName: string
          FragmentName: string
          IconScript: string option
          IndexStyles: string list
          IndexScripts: string list
          ArticleStyles: string list
          ArticleScripts: string list
          IndexNavBar: NavBarConfiguration option
          ArticleNavBar: NavBarConfiguration option }

        static member TryDeserialize(json: JsonElement) =
            match
                Json.tryGetStringProperty "storePath" json,
                Json.tryGetStringProperty "templatePath" json,
                Json.tryGetStringProperty "dataPath" json,
                Json.tryGetStringProperty "outputPath" json,
                Json.tryGetStringProperty "outputDirectory" json,
                Json.tryGetStringProperty "indexPageTemplatePath" json,
                Json.tryGetStringProperty "indexPageName" json,
                Json.tryGetStringProperty "fragmentName" json
            with
            | Some sp, Some tp, Some dp, Some op, Some od, Some ipt, Some ipn, Some fn ->
                { StorePath = sp
                  TemplatePath = tp
                  DataPath = dp
                  OutputPath = op
                  OutputDirectory = od
                  IndexPageTemplatePath = ipt
                  IndexPageName = ipn
                  FragmentName = fn
                  IconScript = Json.tryGetStringProperty "iconScript" json 
                  IndexStyles =
                    Json.tryGetProperty "indexStyles" json
                    |> Option.bind Json.tryGetStringArray
                    |> Option.defaultValue []
                  IndexScripts =
                    Json.tryGetProperty "indexScripts" json
                    |> Option.bind Json.tryGetStringArray
                    |> Option.defaultValue []
                  ArticleStyles =
                    Json.tryGetProperty "articleStyles" json
                    |> Option.bind Json.tryGetStringArray
                    |> Option.defaultValue []
                  ArticleScripts =
                    Json.tryGetProperty "articleScripts" json
                    |> Option.bind Json.tryGetStringArray
                    |> Option.defaultValue []
                  IndexNavBar =
                    Json.tryGetProperty "indexNavBar" json
                    |> Option.bind NavBarConfiguration.Deserialize
                  ArticleNavBar =
                    Json.tryGetProperty "articleNavBar" json
                    |> Option.bind NavBarConfiguration.Deserialize }
                |> Ok
            | None, _, _, _, _, _, _, _ -> Error "Missing `storePath` property"
            | _, None, _, _, _, _, _, _ -> Error "Missing `templatePath` property"
            | _, _, None, _, _, _, _, _ -> Error "Missing `dataPath` property"
            | _, _, _, None, _, _, _, _ -> Error "Missing `outputPath` property"
            | _, _, _, _, None, _, _, _ -> Error "Missing `outputDirectory` property"
            | _, _, _, _, _, None, _, _ -> Error "Missing `indexPageTemplatePath` property"
            | _, _, _, _, _, _, None, _ -> Error "Missing `indexPageName` property"
            | _, _, _, _, _, _, _, None -> Error "Missing `fragmentName` property"




    and NavBarConfiguration =
        { TemplateName: string
          DataPath: string }

        static member TryDeserialize(json: JsonElement) =
            match Json.tryGetStringProperty "templateName" json, Json.tryGetStringProperty "dataPath" json with
            | Some tn, Some dp -> Ok { TemplateName = tn; DataPath = dp }
            | None, _ -> Error "Missing `templateName` property"
            | _, None -> Error "Missing `dataPath` property"

        static member Deserialize(json: JsonElement) =
            match NavBarConfiguration.TryDeserialize json with
            | Ok nbc -> Some nbc
            | Error _ -> None
