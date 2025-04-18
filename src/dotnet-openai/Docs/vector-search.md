```shell
> openai vector search --help
DESCRIPTION:
Performs semantic search against a vector store

USAGE:
    openai vector search <ID> <QUERY> [OPTIONS]

EXAMPLES:
    openai vector search asdf123 "what's the return policy on headphones?" 
--score 0.7

ARGUMENTS:
    <ID>       The ID of the vector store
    <QUERY>    The query to search for   

OPTIONS:
                             DEFAULT                                            
    -h, --help                          Prints help information                 
        --jq [EXPRESSION]               Filter JSON output using a jq expression
        --json                          Output as JSON. Implied when using --jq 
        --monochrome                    Disable colors when rendering JSON to   
                                        the console                             
    -r, --rewrite            True       Automatically rewrite your queries for  
                                        optimal performance                     
    -s, --score <SCORE>      0.5        The minimum score to include in results 
```
