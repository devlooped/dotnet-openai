```shell
> openai vector --help
USAGE:
    openai vector [OPTIONS] <COMMAND>

EXAMPLES:
    openai vector create --name mystore --meta 'key1=value1' --meta 'key2=value'
    openai vector create --name myfiles --file asdf123 --file qwer456
    openai vector search asdf123 "what's the return policy on headphones?" 
--score 0.7

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    create                 Creates a vector store                         
    modify <ID>            Modify a vector store                          
    delete <ID>            Delete a vector store by ID                    
    list                   List vector stores                             
    view <ID>              View a store by its ID                         
    search <ID> <QUERY>    Performs semantic search against a vector store
    file                   Vector store files operations                  
```
