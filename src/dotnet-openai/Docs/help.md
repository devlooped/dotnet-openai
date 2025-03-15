```shell
> oai --help
USAGE:
    oai [OPTIONS] <COMMAND>

EXAMPLES:
    oai file list --jq '.[].id'
    oai file list --jq ".[] | { id: .id, name: .filename, purpose: .purpose }"
    oai file list --jq ".[] | select(.sizeInBytes > 100000) | .id"
    oai vector create --name my-store --meta 'key1=value1' --meta 'key2=value'
    oai vector create --name with-files --file asdf123 --file qwer456

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    auth       
    file       
    vector     
```
