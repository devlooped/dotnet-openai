```shell
> openai auth --help
USAGE:
    openai auth [OPTIONS] <COMMAND>

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    login <project>    Authenticate to OpenAI.                                  
                                                                                
                       Supports API key autentication using the Git Credential  
                       Manager for storage.                                     
                                                                                
                       Switch easily between keys by just specifying the project
                       name after initial login with `--with-token`.            
                                                                                
                       Alternatively, openai will use the authentication token  
                       found in environment variables with the name             
                       `OPENAI_API_KEY`.                                        
                       This method is most suitable for "headless" use such as  
                       in automation.                                           
                                                                                
                       For example, to use openai in GitHub Actions, add        
                       `OPENAI_API_KEY: ${{ secrets.OPENAI_API_KEY }}` to "env" 
    logout             Log out of api.openai.com                                
    status                                                                      
    token              Print the auth token openai is configured to use         
```
