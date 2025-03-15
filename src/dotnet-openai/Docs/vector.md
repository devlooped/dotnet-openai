```shell
> openai vector --help
USAGE:
    openai vector [OPTIONS] <COMMAND>

EXAMPLES:
    openai vector create --name my-store --meta 'key1=value1' --meta 
'key2=value'
    openai vector create --name with-files --file asdf123 --file qwer456

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    create         Creates a vector store       
    modify <ID>    Modify a vector store        
    delete <ID>    Delete a vector store by ID  
    list           List vector stores           
    view <ID>      View a store by its ID       
    file           Vector store files operations
```
