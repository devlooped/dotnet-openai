```shell
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
