namespace Articulus

open System
open System.Text.Json.Serialization

module Common =

    [<CLIMutable>]
    type ArticulusFragmentData =
        { [<JsonPropertyName("title")>]
          Title: string
          [<JsonPropertyName("items")>]
          Items: ArticulusFragmentDataItem seq }

    and [<CLIMutable>] ArticulusFragmentDataItem =
        { [<JsonPropertyName("title")>]
          Title: string
          [<JsonPropertyName("date")>]
          Date: DateTime
          [<JsonPropertyName("description")>]
          Description: string
          [<JsonPropertyName("link")>]
          Link: string }


    