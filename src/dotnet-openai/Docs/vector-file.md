```shell
> openai vector file --help
DESCRIPTION:
Vector store files operations

USAGE:
    openai vector file [OPTIONS] <COMMAND>

EXAMPLES:
    openai file add mystore store.md -a region=us
    openai file add mystore nypop.md -a region=us -a popularity=90

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    add <STORE> <FILE_ID>       Add file to vector store               
    delete <STORE> <FILE_ID>    Remove file from vector store          
    list <STORE>                List files associated with vector store
    view <STORE> <FILE_ID>      View file association to a vector store
```
