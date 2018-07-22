DevOps Monitor
=====

DevOps Monitor is a simple HTTP service implemented as a .NET Core Web API. It's core functionality is to restart docker containers when requested.

It is tested and working on a Digital Ocean VM running Ubuntu.

It is also working from a BitBucket pipeline build configuration.

# Installation

Clone the project, then build it using either Visual Studio or the dotnet CLI. Deploy the resulting binaries to your server where your Docker containers are running. Run `dotnet DevOpsMonitorApi.dll` and now your service will be listening for requests.

# Configuration

DevOps Monitor expects an appsettings.json file in the src directory with the following settings:

```json
{                                                          
    "Logging": {                                           
        "LogLevel": {                                      
            "Default": "Warning"                           
        }                                                  
    },                                                     
    "AllowedHosts": "*",                                   
    "compose-file-path": "<path-to-docker-compose.yml>"
}                                                          
```

# Endpoints

## /docker GET

Prints out a message if the server is healthy.

## /docker POST

Pulls specified docker images and then runs `docker compose down` and `docker compose up` using the docker-compose.yml file specified in appsettings.json.

### Example Body

```json
[
	"example/image:example-tag",
	"example/image2:example-tag2"
]
```