```shell
> openai file list --help
DESCRIPTION:
List files

USAGE:
    openai file list [OPTIONS]

EXAMPLES:
    openai file list --jq '.[].id'
    openai file list --jq ".[] | { id: .id, name: .filename, purpose: .purpose 
}"
    openai file list --jq ".[] | select(.sizeInBytes > 100000) | .id"

OPTIONS:
    -h, --help                  Prints help information                         
        --jq [EXPRESSION]       Filter JSON output using a jq expression        
        --json                  Output as JSON. Implied when using --jq         
        --monochrome            Disable colors when rendering JSON to the       
                                console                                         
        --range [EXPRESSION]    C# range expression to flexibly slice results   
        --skip [SKIP]           Number of items to skip from the results        
        --take [TAKE]           Number of items to take from the results        
    -p, --purpose [PURPOSE]     Purpose of the file (assistants,                
                                assistants_output, batch, batch_output, evals,  
                                fine-tune, fine-tune-results, user_data, vision)
```
