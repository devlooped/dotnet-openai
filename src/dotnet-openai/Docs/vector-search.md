```shell
> openai vector search --help
DESCRIPTION:
Performs semantic search against a vector store

USAGE:
    openai vector search <STORE> <QUERY> [OPTIONS]

EXAMPLES:
    openai vector search mystore "what's the return policy on headphones?" 
--score 0.7
    openai vector search mystore "physical stores?" --filter region=us
    openai vector search mystore "most popular stores?" -f region=us -f 
popularity>=80

ARGUMENTS:
    <STORE>    The ID or name of the vector store
    <QUERY>    The query to search for           

OPTIONS:
                             DEFAULT                                            
    -h, --help                          Prints help information                 
        --jq [EXPRESSION]               Filter JSON output using a jq expression
        --json                          Output as JSON. Implied when using --jq 
        --monochrome                    Disable colors when rendering JSON to   
                                        the console                             
    -f, --filter                        Vector file attributes to filter as KEY[
                                        =|!=|>|>=|<|<=]VALUE                    
    -r, --rewrite            True       Automatically rewrite your queries for  
                                        optimal performance                     
    -s, --score <SCORE>      0.5        The minimum score to include in results 
```
