namespace Articulus.Store

open System
open System.IO
open System.Security.Cryptography
open System.Text
open Freql.Core.Common.Types
open Freql.Sqlite

[<AutoOpen>]
module private Internal =

    open Articulus.Store.Persistence

    let toLines (str: string) = str.Split Environment.NewLine

    let hashStream (hasher: SHA256) (stream: Stream) =
        stream.Seek(0L, SeekOrigin.Begin) |> ignore

        let hash =
            hasher.ComputeHash stream |> Convert.ToHexString

        stream.Seek(0L, SeekOrigin.Begin) |> ignore
        hash

    let hashBytes (hasher: SHA256) (bytes: byte array) =
        hasher.ComputeHash bytes |> Convert.ToHexString

    let strEquals (strA: string) (strB: string) =
        String.Equals(strA, strB, StringComparison.OrdinalIgnoreCase)

    let initialize (ctx: SqliteContext) =
        [ Records.MetadataItem.CreateTableSql()
          Records.Article.CreateTableSql()
          Records.Resource.CreateTableSql()
          Records.ArticleVersion.CreateTableSql()
          Records.ResourceVersion.CreateTableSql()
          Records.ArticleResource.CreateTableSql()
          Records.ArticleMetadataItem.CreateTableSql()
          Records.ResourceMetadataItem.CreateTableSql() ]
        |> List.iter (ctx.ExecuteSqlNonQuery >> ignore)

        ctx

    let createOrOpen (path: string) =
        match File.Exists path with
        | true -> SqliteContext.Open path
        | false -> SqliteContext.Create path |> initialize

    let fetchArticle (ctx: SqliteContext) (name: string) =
        Operations.selectArticleRecord ctx [ "WHERE name = @0" ] [ name ]

    let fetchAllArticles (ctx: SqliteContext) =
        Operations.selectArticleRecords ctx [] []

    let fetchOrderedArticles (ctx: SqliteContext) =
        Operations.selectArticleRecords ctx [ "ORDER BY article_order DESC LIMIT 1" ] []

    let addArticle (ctx: SqliteContext) (name: string) (title: string) (order: int) (articleDate: DateTime) =
        ({ Name = name
           Title = title
           ArticleOrder = order
           Active = true
           ArticleDate = articleDate }: Parameters.NewArticle)
        |> Operations.insertArticle ctx

    let addArticleVersion
        (ctx: SqliteContext)
        (id: string)
        (name: string)
        (version: int)
        (stream: Stream)
        (hash: string)
        (isDraft: bool)
        =
        ({ Id = id
           ArticleName = name
           Version = version
           CreatedOn = DateTime.UtcNow
           RawBlob = BlobField.FromStream stream
           Hash = hash
           IsDraft = isDraft }: Parameters.NewArticleVersion)
        |> Operations.insertArticleVersion ctx

    let fetchLatestArticle (ctx: SqliteContext) =
        Operations.selectArticleRecord ctx [ "ORDER BY article_order DESC LIMIT 1" ] []

    let fetchLatestArticleVersion (ctx: SqliteContext) (name: string) =
        Operations.selectArticleVersionRecord ctx [ "WHERE article_name = @0 ORDER BY version DESC LIMIT 1" ] [ name ]

    let fetchLatestNonDraftArticleVersion (ctx: SqliteContext) (name: string) =
        Operations.selectArticleVersionRecord
            ctx
            [ "WHERE article_name = @0 AND is_draft = 0 ORDER BY version DESC LIMIT 1" ]
            [ name ]

[<RequireQualifiedAccess>]
type AddArticleVersionResult =
    | ArticleNotFound
    | NoChange
    | Added

type ArticulusStore(ctx: SqliteContext) =

    static member Create(path: string) = createOrOpen path |> ArticulusStore

    member _.GetLatestArticleOrder() =
        fetchLatestArticle ctx
        |> Option.map (fun ar -> ar.ArticleOrder)
        |> Option.defaultValue 0
    
    member _.AddArticle(name, title, order, date) = addArticle ctx name title order date

    member _.GetArticle(name) = fetchArticle ctx name

    member _.GetAllArticles() =
        fetchAllArticles ctx
        |> List.sortBy (fun ar -> ar.ArticleOrder)
        
    member _.AddArticleVersion(name: string, isDraft: bool, lines: string list) =
        match fetchArticle ctx name with
        | Some ar ->
            use ms =
                new MemoryStream(
                    lines
                    |> String.concat Environment.NewLine
                    |> Encoding.UTF8.GetBytes
                )

            let hash = hashStream (SHA256.Create()) ms

            fetchLatestArticleVersion ctx ar.Name
            |> Option.map (fun avr ->
                match strEquals hash avr.Hash, avr.IsDraft, isDraft with
                | true, true, true
                | true, false, false
                | true, false, true -> AddArticleVersionResult.NoChange
                | true, true, false
                | false, _, _ ->
                    addArticleVersion ctx (Guid.NewGuid().ToString("n")) name (avr.Version + 1) ms hash isDraft
                    AddArticleVersionResult.Added)
            |> Option.defaultWith (fun _ ->
                addArticleVersion ctx (Guid.NewGuid().ToString("n")) name 1 ms hash isDraft
                AddArticleVersionResult.Added)
        | None -> AddArticleVersionResult.ArticleNotFound
  
    member _.GetLatestArticleVersion(name: string, includeDrafts: bool) =
        fetchArticle ctx name
        |> Option.bind (fun ar ->
            match includeDrafts with
            | true -> fetchLatestArticleVersion ctx ar.Name
            | false -> fetchLatestNonDraftArticleVersion ctx ar.Name)
    
    member _.GetRawArticle(name: string, includeDrafts: bool) =
        fetchArticle ctx name
        |> Option.bind (fun ar ->
            match includeDrafts with
            | true -> fetchLatestArticleVersion ctx ar.Name
            | false -> fetchLatestNonDraftArticleVersion ctx ar.Name)
        |> Option.map (fun avr ->
            avr.RawBlob.ToBytes()
            |> Encoding.UTF8.GetString
            |> toLines)
