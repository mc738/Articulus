namespace Articulus

open FDOM.Rendering
open Fluff.Core

module Rendering =
    
    
    let renderPage (template: Mustache.Template) (lines: string list) =
        FDOM.Core.Parsing.Parser.ParseLines(lines).CreateBlockContent()
        
        //|> fun bc -> Mustache.replace bc true template

