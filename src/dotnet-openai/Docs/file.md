```shell
> oai file --help
USAGE:
    oai file [OPTIONS] <COMMAND>

EXAMPLES:
    oai file list --jq '.[].id'
    oai file list --jq ".[] | { id: .id, name: .filename, purpose: .purpose }"
    oai file list --jq ".[] | select(.sizeInBytes > 100000) | .id"

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    upload <FILE>    Upload a local file, specifying its purpose
    delete <ID>      Delete a file by its ID                    
    list             List files                                 
```
