![Icon](assets/img/icon.png) OpenAI CLI
============

[![Version](https://img.shields.io/nuget/vpre/dotnet-openai.svg?color=royalblue)](https://www.nuget.org/packages/dotnet-openai)
[![Downloads](https://img.shields.io/nuget/dt/dotnet-openai.svg?color=green)](https://www.nuget.org/packages/dotnet-openai)
[![License](https://img.shields.io/github/license/devlooped/dotnet-openai.svg?color=blue)](https://github.com//devlooped/dotnet-openai/blob/main/license.txt)
[![Build](https://github.com/devlooped/dotnet-openai/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/devlooped/dotnet-openai/actions/workflows/build.yml)

<!-- #content -->

## Usage

<!-- include src/dotnet-openai/help.md -->
> openai --help
USAGE:
    openai [OPTIONS] <COMMAND>

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    auth     
```

<!-- src/dotnet-openai/help.md -->
<!-- include src/dotnet-openai/auth.md -->
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
                       found in environment variables                           
                       with the name `OPENAI_API_KEY`.                          
                       This method is most suitable for "headless" use such as  
                       in automation.                                           
                                                                                
                       For example, to use openai in GitHub Actions, add        
                       `OPENAI_API_KEY: ${{ secrets.OPENAI_API_KEY }}` to "env" 
    logout             Log out of api.openai.com                                
    status                                                                      
    token              Print the auth token openai is configured to use         
```

<!-- src/dotnet-openai/auth.md -->
<!-- include src/dotnet-openai/auth-login.md -->
```shell
> openai auth login --help
DESCRIPTION:
Authenticate to OpenAI. 

Supports API key autentication using the Git Credential Manager for storage.

Switch easily between keys by just specifying the project name after initial 
login with `--with-token`.

Alternatively, openai will use the authentication token found in environment 
variables 
with the name `OPENAI_API_KEY`.
This method is most suitable for "headless" use such as in automation.

For example, to use openai in GitHub Actions, add `OPENAI_API_KEY: ${{ 
secrets.OPENAI_API_KEY }}` to "env".

USAGE:
    openai auth login <project> [OPTIONS]

ARGUMENTS:
    <project>    OpenAI project the API key belongs to

OPTIONS:
    -h, --help          Prints help information       
        --with-token    Read token from standard input
```

<!-- src/dotnet-openai/auth-login.md -->
<!-- include src/dotnet-openai/auth-logout.md -->
```shell
> openai auth logout --help
DESCRIPTION:
Log out of api.openai.com

USAGE:
    openai auth logout [OPTIONS]

OPTIONS:
    -h, --help    Prints help information
```

<!-- src/dotnet-openai/auth-logout.md -->
<!-- include src/dotnet-openai/auth-status.md -->
```shell
> openai auth status --help
USAGE:
    openai auth status [OPTIONS]

OPTIONS:
    -h, --help          Prints help information
        --show-token    Display the auth token 
```

<!-- src/dotnet-openai/auth-status.md -->

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
[![Jakob Tikjøb Andersen](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jakobt.png "Jakob Tikjøb Andersen")](https://github.com/jakobt)
[![Tino Hager](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tinohager.png "Tino Hager")](https://github.com/tinohager)
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
[![Joseph Kingry](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jkingry.png "Joseph Kingry")](https://github.com/jkingry)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
