```shell
> openai file --help
USAGE:
    openai file [OPTIONS] <COMMAND>

EXAMPLES:
    openai file list --jq '.[].id'
    openai file list --jq ".[] | { id: .id, name: .filename, purpose: .purpose 
}"
    openai file list --jq ".[] | select(.sizeInBytes > 100000) | .id"

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    upload <FILE>    Upload a local file, specifying its purpose
    delete <ID>      Delete a file by its ID                    
    list             List files                                 
    view <ID>        View a file by its ID                      
```
