namespace Articulus.Store.Persistence

open System
open System.Text.Json.Serialization
open Freql.Core.Common
open Freql.Sqlite

/// Module generated on 06/09/2022 17:56:38 (utc) via Freql.Sqlite.Tools.
[<RequireQualifiedAccess>]
module Records =
    /// A record representing a row in the table `article_metadata`.
    type ArticleMetadataItem =
        { [<JsonPropertyName("articleVersionId")>] ArticleVersionId: int
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("value")>] Value: string }
    
        static member Blank() =
            { ArticleVersionId = 0
              Name = String.Empty
              Value = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE article_metadata (
	article_version_id TEXT NOT NULL,
	name TEXT NOT NULL,
	value TEXT NOT NULL,
	CONSTRAINT article_metadata_PK PRIMARY KEY (article_version_id,name),
	CONSTRAINT article_metadata_FK FOREIGN KEY (article_version_id) REFERENCES article_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              article_version_id,
              name,
              value
        FROM article_metadata
        """
    
        static member TableName() = "article_metadata"
    
    /// A record representing a row in the table `article_resources`.
    type ArticleResource =
        { [<JsonPropertyName("articleVersionId")>] ArticleVersionId: int
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: int
          [<JsonPropertyName("outputPath")>] OutputPath: string }
    
        static member Blank() =
            { ArticleVersionId = 0
              ResourceVersionId = 0
              OutputPath = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE article_resources (
	article_version_id TEXT NOT NULL,
	resource_version_id TEXT NOT NULL,
	output_path TEXT NOT NULL,
	CONSTRAINT article_resources_PK PRIMARY KEY (article_version_id,resource_version_id),
	CONSTRAINT article_resources_FK FOREIGN KEY (article_version_id) REFERENCES article_versions(id),
	CONSTRAINT article_resources_FK_1 FOREIGN KEY (resource_version_id) REFERENCES resource_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              article_version_id,
              resource_version_id,
              output_path
        FROM article_resources
        """
    
        static member TableName() = "article_resources"
    
    /// A record representing a row in the table `article_versions`.
    type ArticleVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("articleName")>] ArticleName: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("rawBlob")>] RawBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("isDraft")>] IsDraft: bool }
    
        static member Blank() =
            { Id = String.Empty
              ArticleName = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              RawBlob = BlobField.Empty()
              Hash = String.Empty
              IsDraft = true }
    
        static member CreateTableSql() = """
        CREATE TABLE article_versions (
	id TEXT NOT NULL,
	article_name TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	raw_blob BLOB NOT NULL,
	hash TEXT NOT NULL, is_draft INTEGER NOT NULL,
	CONSTRAINT article_versions_PK PRIMARY KEY (id),
	CONSTRAINT article_versions_UN UNIQUE (article_name,version),
	CONSTRAINT article_versions_FK FOREIGN KEY (article_name) REFERENCES articles(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              id,
              article_name,
              version,
              created_on,
              raw_blob,
              hash,
              is_draft
        FROM article_versions
        """
    
        static member TableName() = "article_versions"
    
    /// A record representing a row in the table `articles`.
    type Article =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("articleOrder")>] ArticleOrder: int
          [<JsonPropertyName("active")>] Active: bool
          [<JsonPropertyName("articleDate")>] ArticleDate: DateTime }
    
        static member Blank() =
            { Name = String.Empty
              Title = String.Empty
              ArticleOrder = 0
              Active = true
              ArticleDate = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE articles (
	name TEXT NOT NULL,
	title TEXT NOT NULL,
	article_order INTEGER NOT NULL, active INTEGER NOT NULL, article_date TEXT NOT NULL,
	CONSTRAINT articles_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              name,
              title,
              article_order,
              active,
              article_date
        FROM articles
        """
    
        static member TableName() = "articles"
    
    /// A record representing a row in the table `metadata`.
    type MetadataItem =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("value")>] Value: string }
    
        static member Blank() =
            { Name = String.Empty
              Value = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE metadata (
	name TEXT NOT NULL,
	value TEXT NOT NULL,
	CONSTRAINT metadata_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              name,
              value
        FROM metadata
        """
    
        static member TableName() = "metadata"
    
    /// A record representing a row in the table `resource_metadata`.
    type ResourceMetadataItem =
        { [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: int
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("value")>] Value: string }
    
        static member Blank() =
            { ResourceVersionId = 0
              Name = String.Empty
              Value = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_metadata (
	resource_version_id TEXT NOT NULL,
	name TEXT NOT NULL,
	value TEXT NOT NULL,
	CONSTRAINT resource_metadata_PK PRIMARY KEY (name,resource_version_id),
	CONSTRAINT resource_metadata_FK FOREIGN KEY (resource_version_id) REFERENCES resource_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_version_id,
              name,
              value
        FROM resource_metadata
        """
    
        static member TableName() = "resource_metadata"
    
    /// A record representing a row in the table `resource_versions`.
    type ResourceVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceName")>] ResourceName: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("rawBlob")>] RawBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string }
    
        static member Blank() =
            { Id = String.Empty
              ResourceName = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              RawBlob = BlobField.Empty()
              Hash = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_versions (
	id TEXT NOT NULL,
	resource_name TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	raw_blob BLOB NOT NULL,
	hash TEXT NOT NULL,
	CONSTRAINT resource_versions_PK PRIMARY KEY (id),
	CONSTRAINT resource_versions_UN UNIQUE (resource_name,version),
	CONSTRAINT resource_versions_FK FOREIGN KEY (resource_name) REFERENCES resources(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              id,
              resource_name,
              version,
              created_on,
              raw_blob,
              hash
        FROM resource_versions
        """
    
        static member TableName() = "resource_versions"
    
    /// A record representing a row in the table `resources`.
    type Resource =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("extension")>] Extension: string }
    
        static member Blank() =
            { Name = String.Empty
              Extension = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE resources (
	name TEXT NOT NULL,
	extension TEXT NOT NULL,
	CONSTRAINT resources_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              name,
              extension
        FROM resources
        """
    
        static member TableName() = "resources"
    

/// Module generated on 06/09/2022 17:56:38 (utc) via Freql.Tools.
[<RequireQualifiedAccess>]
module Parameters =
    /// A record representing a new row in the table `article_metadata`.
    type NewArticleMetadataItem =
        { [<JsonPropertyName("articleVersionId")>] ArticleVersionId: int
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("value")>] Value: string }
    
        static member Blank() =
            { ArticleVersionId = 0
              Name = String.Empty
              Value = String.Empty }
    
    
    /// A record representing a new row in the table `article_resources`.
    type NewArticleResource =
        { [<JsonPropertyName("articleVersionId")>] ArticleVersionId: int
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: int
          [<JsonPropertyName("outputPath")>] OutputPath: string }
    
        static member Blank() =
            { ArticleVersionId = 0
              ResourceVersionId = 0
              OutputPath = String.Empty }
    
    
    /// A record representing a new row in the table `article_versions`.
    type NewArticleVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("articleName")>] ArticleName: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("rawBlob")>] RawBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("isDraft")>] IsDraft: bool }
    
        static member Blank() =
            { Id = String.Empty
              ArticleName = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              RawBlob = BlobField.Empty()
              Hash = String.Empty
              IsDraft = true }
    
    
    /// A record representing a new row in the table `articles`.
    type NewArticle =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("articleOrder")>] ArticleOrder: int
          [<JsonPropertyName("active")>] Active: bool
          [<JsonPropertyName("articleDate")>] ArticleDate: DateTime }
    
        static member Blank() =
            { Name = String.Empty
              Title = String.Empty
              ArticleOrder = 0
              Active = true
              ArticleDate = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `metadata`.
    type NewMetadataItem =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("value")>] Value: string }
    
        static member Blank() =
            { Name = String.Empty
              Value = String.Empty }
    
    
    /// A record representing a new row in the table `resource_metadata`.
    type NewResourceMetadataItem =
        { [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: int
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("value")>] Value: string }
    
        static member Blank() =
            { ResourceVersionId = 0
              Name = String.Empty
              Value = String.Empty }
    
    
    /// A record representing a new row in the table `resource_versions`.
    type NewResourceVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceName")>] ResourceName: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("rawBlob")>] RawBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string }
    
        static member Blank() =
            { Id = String.Empty
              ResourceName = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              RawBlob = BlobField.Empty()
              Hash = String.Empty }
    
    
    /// A record representing a new row in the table `resources`.
    type NewResource =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("extension")>] Extension: string }
    
        static member Blank() =
            { Name = String.Empty
              Extension = String.Empty }
    
    
/// Module generated on 06/09/2022 17:56:38 (utc) via Freql.Tools.
[<RequireQualifiedAccess>]
module Operations =

    let buildSql (lines: string list) = lines |> String.concat Environment.NewLine

    /// Select a `Records.ArticleMetadataItem` from the table `article_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ArticleMetadataItem>` and uses Records.ArticleMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectArticleMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectArticleMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ArticleMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ArticleMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ArticleMetadataItem>` and uses Records.ArticleMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectArticleMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectArticleMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ArticleMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ArticleMetadataItem>(sql, parameters)
    
    let insertArticleMetadataItem (context: SqliteContext) (parameters: Parameters.NewArticleMetadataItem) =
        context.Insert("article_metadata", parameters)
    
    /// Select a `Records.ArticleResource` from the table `article_resources`.
    /// Internally this calls `context.SelectSingleAnon<Records.ArticleResource>` and uses Records.ArticleResource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectArticleResourceRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectArticleResourceRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ArticleResource.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ArticleResource>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ArticleResource>` and uses Records.ArticleResource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectArticleResourceRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectArticleResourceRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ArticleResource.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ArticleResource>(sql, parameters)
    
    let insertArticleResource (context: SqliteContext) (parameters: Parameters.NewArticleResource) =
        context.Insert("article_resources", parameters)
    
    /// Select a `Records.ArticleVersion` from the table `article_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ArticleVersion>` and uses Records.ArticleVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectArticleVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectArticleVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ArticleVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ArticleVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ArticleVersion>` and uses Records.ArticleVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectArticleVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectArticleVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ArticleVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ArticleVersion>(sql, parameters)
    
    let insertArticleVersion (context: SqliteContext) (parameters: Parameters.NewArticleVersion) =
        context.Insert("article_versions", parameters)
    
    /// Select a `Records.Article` from the table `articles`.
    /// Internally this calls `context.SelectSingleAnon<Records.Article>` and uses Records.Article.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectArticleRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectArticleRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Article.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Article>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Article>` and uses Records.Article.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectArticleRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectArticleRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Article.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Article>(sql, parameters)
    
    let insertArticle (context: SqliteContext) (parameters: Parameters.NewArticle) =
        context.Insert("articles", parameters)
    
    /// Select a `Records.MetadataItem` from the table `metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.MetadataItem>` and uses Records.MetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.MetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.MetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.MetadataItem>` and uses Records.MetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.MetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.MetadataItem>(sql, parameters)
    
    let insertMetadataItem (context: SqliteContext) (parameters: Parameters.NewMetadataItem) =
        context.Insert("metadata", parameters)
    
    /// Select a `Records.ResourceMetadataItem` from the table `resource_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceMetadataItem>` and uses Records.ResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceMetadataItem>` and uses Records.ResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceMetadataItem>(sql, parameters)
    
    let insertResourceMetadataItem (context: SqliteContext) (parameters: Parameters.NewResourceMetadataItem) =
        context.Insert("resource_metadata", parameters)
    
    /// Select a `Records.ResourceVersion` from the table `resource_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceVersion>` and uses Records.ResourceVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceVersion>` and uses Records.ResourceVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceVersion>(sql, parameters)
    
    let insertResourceVersion (context: SqliteContext) (parameters: Parameters.NewResourceVersion) =
        context.Insert("resource_versions", parameters)
    
    /// Select a `Records.Resource` from the table `resources`.
    /// Internally this calls `context.SelectSingleAnon<Records.Resource>` and uses Records.Resource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Resource.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Resource>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Resource>` and uses Records.Resource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Resource.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Resource>(sql, parameters)
    
    let insertResource (context: SqliteContext) (parameters: Parameters.NewResource) =
        context.Insert("resources", parameters)
    