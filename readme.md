![Icon](assets/img/icon.png) OpenAI CLI
============

[![Version](https://img.shields.io/nuget/vpre/dotnet-openai.svg?color=royalblue)](https://www.nuget.org/packages/dotnet-openai)
[![Downloads](https://img.shields.io/nuget/dt/dotnet-openai.svg?color=green)](https://www.nuget.org/packages/dotnet-openai)
[![License](https://img.shields.io/github/license/devlooped/dotnet-openai.svg?color=blue)](https://github.com//devlooped/dotnet-openai/blob/main/license.txt)
[![Build](https://github.com/devlooped/dotnet-openai/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/devlooped/dotnet-openai/actions/workflows/build.yml)

<!-- #content -->

An OpenAI CLI for managing files, vectors and more

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
--file asdf123 --file qwer456
    openai vector search mystore "what's the return policy on headphones?" 
--score 0.7 --filter region=us
    openai vector search mystore "most popular stores?" -f region=us -f 
popularity>=80

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    auth        
    file        
    vector      
    model       
    sponsor     
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
    list               Lists projects that have been authenticated and can be   
                       used with the login command                              
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
--file asdf123 --file qwer456
    openai vector search mystore "what's the return policy on headphones?" 
--score 0.7 --filter region=us
    openai vector search mystore "most popular stores?" -f region=us -f 
popularity>=80
    openai file add mystore store.md -a region=us
    openai file add mystore nypop.md -a region=us -a popularity=90

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

<!-- src/dotnet-openai/Docs/vector.md -->

Supports [semantic search](https://platform.openai.com/docs/guides/retrieval#semantic-search) 
including [attribute filtering](https://platform.openai.com/docs/guides/retrieval#attribute-filtering):

<!-- include src/dotnet-openai/Docs/vector-search.md -->
```shell
> openai vector search --help
DESCRIPTION:
Performs semantic search against a vector store

USAGE:
    openai vector search <STORE> <QUERY> [OPTIONS]

EXAMPLES:
    openai vector search mystore "what's the return policy on headphones?" 
--score 0.7 --filter region=us
    openai vector search mystore "most popular stores?" -f region=us -f 
popularity>=80

ARGUMENTS:
    <STORE>    The ID or name of the vector store
    <QUERY>    The query to search for           

OPTIONS:
                             DEFAULT                                            
    -h, --help                          Prints help information                 
        --jq [EXPRESSION]               Filter JSON output using a jq expression
        --json                          Output as JSON. Implied when using --jq 
        --monochrome                    Disable colors when rendering JSON to   
                                        the console                             
    -f, --filter                        Vector file attributes to filter as KEY[
                                        =|!=|>|>=|<|<=]VALUE                    
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

EXAMPLES:
    openai file add mystore store.md -a region=us
    openai file add mystore nypop.md -a region=us -a popularity=90

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    add <STORE> <FILE_ID>       Add file to vector store                     
    delete <STORE> <FILE_ID>    Remove file from vector store                
    list <STORE>                List files associated with vector store      
    modify <STORE> <FILE_ID>    Add or modify file attributes in vector store
    view <STORE> <FILE_ID>      View file association to a vector store      
```

<!-- src/dotnet-openai/Docs/vector-file.md -->

<!-- include src/dotnet-openai/Docs/vector-file-add.md -->
```shell
> openai vector file add --help
DESCRIPTION:
Add file to vector store

USAGE:
    openai vector file add <STORE> <FILE_ID> [OPTIONS]

EXAMPLES:
    openai file add mystore store.md -a region=us
    openai file add mystore nypop.md -a region=us -a popularity=90

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

<!-- src/dotnet-openai/Docs/vector-file-add.md -->

<!-- include src/dotnet-openai/Docs/vector-file-list.md -->
```shell
> openai vector file list --help
DESCRIPTION:
List files associated with vector store

USAGE:
    openai vector file list <STORE> [OPTIONS]

ARGUMENTS:
    <STORE>    The ID or name of the vector store

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

## Sponsoring

You can support further development of this tool by sponsoring. The tool offers various 
commands to sync your sponsorship status, powered by [SponsorLink](https://github.com/devlooped/#sponsorlink).

<!-- include src/dotnet-openai/Docs/sponsor.md -->
```shell
> openai sponsor --help
USAGE:
    openai sponsor [OPTIONS] <COMMAND>

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    check     Checks the current sponsorship status with devlooped, entirely    
              offline                                                           
    config    Manages sponsorlink configuration                                 
    view      Validates and displays your sponsor manifest for devlooped, if    
              present                                                           
    sync      Synchronizes your sponsorship manifest for devlooped              
```

<!-- src/dotnet-openai/Docs/sponsor.md -->
<!-- include src/dotnet-openai/Docs/sponsor-sync.md -->
```shell
> openai sponsor sync --help
DESCRIPTION:
Synchronizes your sponsorship manifest for devlooped

USAGE:
    openai sponsor sync [OPTIONS]

OPTIONS:
    -h, --help          Prints help information                                 
        --autosync      Enable or disable automatic synchronization of expired  
                        manifests                                               
    -f, --force         Force sync, regardless of expiration of manifests found 
                        locally                                                 
    -v, --validate      Validate local manifests using the issuer public key    
    -u, --unattended    Prevent interactive credentials refresh                 
        --with-token    Read GitHub authentication token from standard input for
                        sync                                                    
```

<!-- src/dotnet-openai/Docs/sponsor-sync.md -->

<!-- #content -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
# Sponsors 

<!-- sponsors.md -->
[![Clarius Org](https://avatars.githubusercontent.com/u/71888636?v=4&s=39 "Clarius Org")](https://github.com/clarius)
[![MFB Technologies, Inc.](https://avatars.githubusercontent.com/u/87181630?v=4&s=39 "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![Khamza Davletov](https://avatars.githubusercontent.com/u/13615108?u=11b0038e255cdf9d1940fbb9ae9d1d57115697ab&v=4&s=39 "Khamza Davletov")](https://github.com/khamza85)
[![SandRock](https://avatars.githubusercontent.com/u/321868?u=99e50a714276c43ae820632f1da88cb71632ec97&v=4&s=39 "SandRock")](https://github.com/sandrock)
[![DRIVE.NET, Inc.](https://avatars.githubusercontent.com/u/15047123?v=4&s=39 "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Keith Pickford](https://avatars.githubusercontent.com/u/16598898?u=64416b80caf7092a885f60bb31612270bffc9598&v=4&s=39 "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://avatars.githubusercontent.com/u/127185?u=7f50babfc888675e37feb80851a4e9708f573386&v=4&s=39 "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://avatars.githubusercontent.com/u/67574?u=3991fb983e1c399edf39aebc00a9f9cd425703bd&v=4&s=39 "Kori Francis")](https://github.com/kfrancis)
[![Reuben Swartz](https://avatars.githubusercontent.com/u/724704?u=2076fe336f9f6ad678009f1595cbea434b0c5a41&v=4&s=39 "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://avatars.githubusercontent.com/u/480334?v=4&s=39 "Jacob Foshee")](https://github.com/jfoshee)
[![](https://avatars.githubusercontent.com/u/33566379?u=bf62e2b46435a267fa246a64537870fd2449410f&v=4&s=39 "")](https://github.com/Mrxx99)
[![Eric Johnson](https://avatars.githubusercontent.com/u/26369281?u=41b560c2bc493149b32d384b960e0948c78767ab&v=4&s=39 "Eric Johnson")](https://github.com/eajhnsn1)
[![Jonathan ](https://avatars.githubusercontent.com/u/5510103?u=98dcfbef3f32de629d30f1f418a095bf09e14891&v=4&s=39 "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Ken Bonny](https://avatars.githubusercontent.com/u/6417376?u=569af445b6f387917029ffb5129e9cf9f6f68421&v=4&s=39 "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://avatars.githubusercontent.com/u/122666?v=4&s=39 "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://avatars.githubusercontent.com/u/5989304?v=4&s=39 "agileworks-eu")](https://github.com/agileworks-eu)
[![Zheyu Shen](https://avatars.githubusercontent.com/u/4067473?v=4&s=39 "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://avatars.githubusercontent.com/u/87844133?v=4&s=39 "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://avatars.githubusercontent.com/u/16239022?v=4&s=39 "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://avatars.githubusercontent.com/u/68428092?v=4&s=39 "4OTC")](https://github.com/4OTC)
[![domischell](https://avatars.githubusercontent.com/u/66068846?u=0a5c5e2e7d90f15ea657bc660f175605935c5bea&v=4&s=39 "domischell")](https://github.com/DominicSchell)
[![Adrian Alonso](https://avatars.githubusercontent.com/u/2027083?u=129cf516d99f5cb2fd0f4a0787a069f3446b7522&v=4&s=39 "Adrian Alonso")](https://github.com/adalon)
[![torutek](https://avatars.githubusercontent.com/u/33917059?v=4&s=39 "torutek")](https://github.com/torutek)
[![mccaffers](https://avatars.githubusercontent.com/u/16667079?u=110034edf51097a5ee82cb6a94ae5483568e3469&v=4&s=39 "mccaffers")](https://github.com/mccaffers)
[![Seika Logiciel](https://avatars.githubusercontent.com/u/2564602?v=4&s=39 "Seika Logiciel")](https://github.com/SeikaLogiciel)
[![Andrew Grant](https://avatars.githubusercontent.com/devlooped-user?s=39 "Andrew Grant")](https://github.com/wizardness)


<!-- sponsors.md -->
[![Sponsor this project](https://avatars.githubusercontent.com/devlooped-sponsor?s=118 "Sponsor this project")](https://github.com/sponsors/devlooped)

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
