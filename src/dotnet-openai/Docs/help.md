```shell
> oai --help
USAGE:
    oai [OPTIONS] <COMMAND>

EXAMPLES:
    oai file list --jq '.[].id'
    oai file list --jq ".[] | { id: .id, name: .filename, purpose: .purpose }"
    oai file list --jq ".[] | select(.sizeInBytes > 100000) | .id"

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    auth     
    file     
