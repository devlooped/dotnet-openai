```shell
> openai vector --help
USAGE:
    openai vector [OPTIONS] <COMMAND>

EXAMPLES:
    openai vector create --name mystore --meta 'key1=value1' --meta 'key2=value'
    openai vector create --name myfiles --file asdf123 --file qwer456
    openai vector search mystore "what's the return policy on headphones?" 
--score 0.7

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    create                    Creates a vector store                         
    modify <STORE>            Modify a vector store                          
    delete <STORE>            Delete a vector store by ID or name            
    list                      List vector stores                             
    view <STORE>              View a store by its ID or name                 
    search <STORE> <QUERY>    Performs semantic search against a vector store
    file                      Vector store files operations                  
```
