```shell
> openai vector search --help
USAGE:
    openai vector search <ID> <QUERY> [OPTIONS]

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
    -a, --attribute                     Vector file attributes to filter  as    
                                        KEY=VALUE                               
    -r, --rewrite            True       Automatically rewrite your queries for  
                                        optimal performance                     
    -s, --score <SCORE>      0.5        The minimum score to include in results 
```
