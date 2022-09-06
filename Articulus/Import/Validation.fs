namespace Articulus.Import

module Validation =
    
    [<RequireQualifiedAccess>]
    type ValidationError =
        | MissingTitle
        | MissingDate
        | MissingOrder
        
    let validateMetadata (metadata: Map<string, string>) =
        [ match metadata.TryFind "articulus:title" with
          | Some _ -> Ok()
          | None -> Error ValidationError.MissingTitle
          match metadata.TryFind "articulus:created_on" with
          | Some _ -> Ok()
          | None -> Error ValidationError.MissingDate
          match metadata.TryFind "articulus:order" with
          | Some _ -> Ok()
          | None -> Error ValidationError.MissingOrder ]
        |> groupResults


