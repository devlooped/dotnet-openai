```shell
> openai vector file add --help
DESCRIPTION:
Add file to vector store

USAGE:
    openai vector file add <STORE> <FILE_ID> [OPTIONS]

ARGUMENTS:
    <STORE>      The ID or name of the vector store
    <FILE_ID>    File ID to add to the vector store

OPTIONS:
    -h, --help               Prints help information                          
        --jq [EXPRESSION]    Filter JSON output using a jq expression         
        --json               Output as JSON. Implied when using --jq          
        --monochrome         Disable colors when rendering JSON to the console
    -a, --attribute          Attributes to add to the vector file as KEY=VALUE
```
