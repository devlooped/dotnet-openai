```shell
> oai auth --help
USAGE:
    oai auth [OPTIONS] <COMMAND>

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    login <project>    Authenticate to OpenAI.                                  
                                                                                
                       Supports API key autentication using the Git Credential  
                       Manager for storage.                                     
                                                                                
                       Switch easily between keys by just specifying the project
                       name after initial login with `--with-token`.            
                                                                                
                       Alternatively, oai will use the authentication token     
                       found in environment variables                           
                       with the name `OPENAI_API_KEY`.                          
                       This method is most suitable for "headless" use such as  
                       in automation.                                           
                                                                                
                       For example, to use oai in GitHub Actions, add           
                       `OPENAI_API_KEY: ${{ secrets.OPENAI_API_KEY }}` to "env" 
    logout             Log out of api.openai.com                                
    status                                                                      
    token              Print the auth token oai is configured to use            
```
