![Icon](assets/img/icon.png) OpenAI CLI
============

[![Version](https://img.shields.io/nuget/vpre/dotnet-openai.svg?color=royalblue)](https://www.nuget.org/packages/dotnet-openai)
[![Downloads](https://img.shields.io/nuget/dt/dotnet-openai.svg?color=green)](https://www.nuget.org/packages/dotnet-openai)
[![License](https://img.shields.io/github/license/devlooped/dotnet-openai.svg?color=blue)](https://github.com//devlooped/dotnet-openai/blob/main/license.txt)
[![Build](https://github.com/devlooped/dotnet-openai/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/devlooped/dotnet-openai/actions/workflows/build.yml)

<!-- #content -->

## Usage

<!-- include src/dotnet-openai/Docs/help.md -->
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
```

<!-- src/dotnet-openai/Docs/help.md -->

## Authentication

Authentication is managed for you by the CLI, using the [Git Credential Manager](https://github.com/git-ecosystem/git-credential-manager) 
as the cross-platform secure storage for your API key(s). You can login multiple project/key 
combination and then just change the active one without ever re-entering the keys.

See [authentication](https://platform.openai.com/docs/api-reference/authentication) for more details.

<!-- include src/dotnet-openai/Docs/auth.md -->
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
    status             Shows the current authentication status                  
    token              Print the auth token openai is configured to use         
```

<!-- src/dotnet-openai/Docs/auth.md -->
<!-- include src/dotnet-openai/Docs/auth-login.md -->
```shell
> openai auth login --help
DESCRIPTION:
Authenticate to OpenAI. 

Supports API key autentication using the Git Credential Manager for storage.

Switch easily between keys by just specifying the project name after initial 
login with `--with-token`.

Alternatively, openai will use the authentication token found in environment 
variables with the name `OPENAI_API_KEY`.
This method is most suitable for "headless" use such as in automation.

For example, to use openai in GitHub Actions, add `OPENAI_API_KEY: ${{ 
secrets.OPENAI_API_KEY }}` to "env"

USAGE:
    openai auth login <project> [OPTIONS]

ARGUMENTS:
    <project>    OpenAI project the API key belongs to

OPTIONS:
    -h, --help          Prints help information       
        --with-token    Read token from standard input
```

<!-- src/dotnet-openai/Docs/auth-login.md -->
<!-- include src/dotnet-openai/Docs/auth-logout.md -->
```shell
> openai auth logout --help
DESCRIPTION:
Log out of api.openai.com

USAGE:
    openai auth logout [OPTIONS]

OPTIONS:
    -h, --help    Prints help information
```

<!-- src/dotnet-openai/Docs/auth-logout.md -->
<!-- include src/dotnet-openai/Docs/auth-status.md -->
```shell
> openai auth status --help
DESCRIPTION:
Shows the current authentication status

USAGE:
    openai auth status [OPTIONS]

OPTIONS:
    -h, --help          Prints help information
        --show-token    Display the auth token 
```

<!-- src/dotnet-openai/Docs/auth-status.md -->

## Files

Implements the [Files API](https://platform.openai.com/docs/api-reference/files).

<!-- include src/dotnet-openai/Docs/file.md -->
```shell
> openai file --help
USAGE:
    openai file [OPTIONS] <COMMAND>

EXAMPLES:
    openai file list --jq '.[].id'
    openai file list --jq ".[] | select(.sizeInBytes > 100000) | .id"

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    upload <FILE>    Upload a local file, specifying its purpose
    delete <ID>      Delete a file by its ID                    
    list             List files                                 
    view <ID>        View a file by its ID                      
```

<!-- src/dotnet-openai/Docs/file.md -->

<!-- include src/dotnet-openai/Docs/file-list.md -->
```shell
> openai file list --help
DESCRIPTION:
List files

USAGE:
    openai file list [OPTIONS]

EXAMPLES:
    openai file list --jq '.[].id'
    openai file list --jq ".[] | select(.sizeInBytes > 100000) | .id"

OPTIONS:
    -h, --help                  Prints help information                         
        --jq [EXPRESSION]       Filter JSON output using a jq expression        
        --json                  Output as JSON. Implied when using --jq         
        --monochrome            Disable colors when rendering JSON to the       
                                console                                         
        --range [EXPRESSION]    C# range expression to flexibly slice results   
        --skip [SKIP]           Number of items to skip from the results        
        --take [TAKE]           Number of items to take from the results        
    -p, --purpose [PURPOSE]     Purpose of the file (assistants,                
                                assistants_output, batch, batch_output, evals,  
                                fine-tune, fine-tune-results, user_data, vision)
```

<!-- src/dotnet-openai/Docs/file-list.md -->

## Vector Stores

Implements the [Vector Stores API](https://platform.openai.com/docs/api-reference/vector-stores).

<!-- include src/dotnet-openai/Docs/vector.md -->
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
    create                 Creates a vector store                         
    modify <ID>            Modify a vector store                          
    delete <ID>            Delete a vector store by ID                    
    list                   List vector stores                             
    view <ID>              View a store by its ID                         
    search <ID> <QUERY>    Performs semantic search against a vector store
    file                   Vector store files operations                  
```

<!-- src/dotnet-openai/Docs/vector.md -->

<!-- include src/dotnet-openai/Docs/vector-search.md -->
```shell
> openai vector search --help
DESCRIPTION:
Performs semantic search against a vector store

USAGE:
    openai vector search <ID> <QUERY> [OPTIONS]

EXAMPLES:
    openai vector search mystore "what's the return policy on headphones?" 
--score 0.7

ARGUMENTS:
    <ID>       The ID of the vector store
    <QUERY>    The query to search for   

OPTIONS:
                             DEFAULT                                            
    -h, --help                          Prints help information                 
        --jq [EXPRESSION]               Filter JSON output using a jq expression
        --json                          Output as JSON. Implied when using --jq 
        --monochrome                    Disable colors when rendering JSON to   
                                        the console                             
    -r, --rewrite            True       Automatically rewrite your queries for  
                                        optimal performance                     
    -s, --score <SCORE>      0.5        The minimum score to include in results 
```

<!-- src/dotnet-openai/Docs/vector-search.md -->

<!-- include src/dotnet-openai/Docs/vector-file.md -->
```shell
> openai vector file --help
DESCRIPTION:
Vector store files operations

USAGE:
    openai vector file [OPTIONS] <COMMAND>

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    add <STORE_ID> <FILE_ID>       Add file to vector store               
    delete <STORE_ID> <FILE_ID>    Remove file from vector store          
    list <STORE_ID>                List files associated with vector store
    view <STORE_ID> <FILE_ID>      View file association to a vector store
```

<!-- src/dotnet-openai/Docs/vector-file.md -->

<!-- include src/dotnet-openai/Docs/vector-file-add.md -->
```shell
> openai vector file add --help
DESCRIPTION:
Add file to vector store

USAGE:
    openai vector file add <STORE_ID> <FILE_ID> [OPTIONS]

ARGUMENTS:
    <STORE_ID>    The ID of the vector store        
    <FILE_ID>     File ID to add to the vector store

OPTIONS:
    -h, --help               Prints help information                          
        --jq [EXPRESSION]    Filter JSON output using a jq expression         
        --json               Output as JSON. Implied when using --jq          
        --monochrome         Disable colors when rendering JSON to the console
    -a, --attribute          Attributes to add to the vector file as KEY=VALUE
```

<!-- src/dotnet-openai/Docs/vector-file-add.md -->

<!-- include src/dotnet-openai/Docs/vector-file-list.md -->
```shell
> openai vector file list --help
DESCRIPTION:
List files associated with vector store

USAGE:
    openai vector file list <STORE_ID> [OPTIONS]

ARGUMENTS:
    <STORE_ID>    The ID of the vector store

OPTIONS:
                                DEFAULT                                         
    -h, --help                               Prints help information            
        --jq [EXPRESSION]                    Filter JSON output using a jq      
                                             expression                         
        --json                               Output as JSON. Implied when using 
                                             --jq                               
        --monochrome                         Disable colors when rendering JSON 
                                             to the console                     
        --range [EXPRESSION]                 C# range expression to flexibly    
                                             slice results                      
        --skip [SKIP]                        Number of items to skip from the   
                                             results                            
        --take [TAKE]                        Number of items to take from the   
                                             results                            
    -f, --filter                completed    Filter by status (in_progress,     
                                             completed, failed, cancelled)      
```

<!-- src/dotnet-openai/Docs/vector-file-list.md -->

## Models

List and view available models via the [models API](https://platform.openai.com/docs/api-reference/models/list);

<!-- include src/dotnet-openai/Docs/model.md -->
```shell
> openai model --help
USAGE:
    openai model [OPTIONS] <COMMAND>

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    list         List available models
    view <ID>    View model details   
```

<!-- src/dotnet-openai/Docs/model.md -->

<!-- #content -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
# Sponsors 

<!-- sponsors.md -->
[![Clarius Org](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/clarius.png "Clarius Org")](https://github.com/clarius)
[![MFB Technologies, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MFB-Technologies-Inc.png "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![Torutek](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/torutek-gh.png "Torutek")](https://github.com/torutek-gh)
[![DRIVE.NET, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/drivenet.png "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Keith Pickford](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Keflon.png "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tbolon.png "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/kfrancis.png "Kori Francis")](https://github.com/kfrancis)
[![Toni Wenzel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/twenzel.png "Toni Wenzel")](https://github.com/twenzel)
[![Uno Platform](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/unoplatform.png "Uno Platform")](https://github.com/unoplatform)
[![Dan Siegel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/dansiegel.png "Dan Siegel")](https://github.com/dansiegel)
[![Reuben Swartz](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/rbnswartz.png "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jfoshee.png "Jacob Foshee")](https://github.com/jfoshee)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Mrxx99.png "")](https://github.com/Mrxx99)
[![Eric Johnson](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/eajhnsn1.png "Eric Johnson")](https://github.com/eajhnsn1)
[![Ix Technologies B.V.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/IxTechnologies.png "Ix Technologies B.V.")](https://github.com/IxTechnologies)
[![David JENNI](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/davidjenni.png "David JENNI")](https://github.com/davidjenni)
[![Jonathan ](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Jonathan-Hickey.png "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Charley Wu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/akunzai.png "Charley Wu")](https://github.com/akunzai)
[![Ken Bonny](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KenBonny.png "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/SimonCropp.png "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/agileworks-eu.png "agileworks-eu")](https://github.com/agileworks-eu)
[![sorahex](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/sorahex.png "sorahex")](https://github.com/sorahex)
[![Zheyu Shen](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/arsdragonfly.png "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/vezel-dev.png "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/ChilliCream.png "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/4OTC.png "4OTC")](https://github.com/4OTC)
[![Vincent Limo](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/v-limo.png "Vincent Limo")](https://github.com/v-limo)
[![Jordan S. Jones](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jordansjones.png "Jordan S. Jones")](https://github.com/jordansjones)
[![domischell](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/DominicSchell.png "domischell")](https://github.com/DominicSchell)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
