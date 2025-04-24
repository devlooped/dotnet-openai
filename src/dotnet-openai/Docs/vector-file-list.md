```shell
> openai vector file list --help
DESCRIPTION:
List files associated with vector store

USAGE:
    openai vector file list <STORE> [OPTIONS]

ARGUMENTS:
    <STORE>    The ID or name of the vector store

OPTIONS:
                                DEFAULT                                         
    -h, --help                               Prints help information            
        --jq [EXPRESSION]                    Filter JSON output using a jq      
                                             expression                         
        --json                               Output as JSON. Implied when using 
                                             --jq                               
        --monochrome                         Disable colors when rendering JSON 
                                             to the console                     
        --range [EXPRESSION]                 C# range expression to flexibly    
                                             slice results                      
        --skip [SKIP]                        Number of items to skip from the   
                                             results                            
        --take [TAKE]                        Number of items to take from the   
                                             results                            
    -f, --filter                completed    Filter by status (in_progress,     
                                             completed, failed, cancelled)      
```
