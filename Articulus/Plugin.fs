namespace Articulus

module Plugin =

    open System.Text.Json.Serialization

    [<CLIMutable>]
    type ArticulusConfiguration =
        { [<JsonPropertyName("storePath")>]
          StorePath: string
          [<JsonPropertyName("templatePath")>]
          TemplatePath: string
          [<JsonPropertyName("dataPath")>]
          DataPath: string
          [<JsonPropertyName("outputPath")>]
          OutputPath: string
          [<JsonPropertyName("outputDirectory")>]
          OutputDirectory: string
          [<JsonPropertyName("indexPageTemplatePath")>]
          IndexPageTemplatePath: string
          [<JsonPropertyName("indexPageName")>]
          IndexPageName: string
          [<JsonPropertyName("fragmentName")>]
          FragmentName: string }
