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
