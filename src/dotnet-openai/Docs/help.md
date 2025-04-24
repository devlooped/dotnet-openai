```shell
> openai --help
USAGE:
    openai [OPTIONS] <COMMAND>

EXAMPLES:
    openai file list --jq '.[].id'
    openai file list --jq ".[] | select(.sizeInBytes > 100000) | .id"
    openai vector create --name mystore --meta 'key1=value1' --meta 'key2=value'
    openai vector create --name myfiles --file asdf123 --file qwer456
    openai vector search mystore "what's the return policy on headphones?" 
--score 0.7

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    auth        
    file        
    vector      
    model       
    sponsor     
```
