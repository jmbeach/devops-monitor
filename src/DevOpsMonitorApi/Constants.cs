using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOpsMonitorApi
{
    public static class Constants
    {
        public const string FileNameDateFormat = "MM-dd-yy-hh";
        public const string LogFileName = "dev-ops-monior-log{0}.log";
        public const string DefaultTag = "latest";
        public const string AliveMessage = "Dev Ops service is alive";

        public class Environment
        {
            public const string DockerImageTag = "DOCKER_IMAGE_TAG";
        }

        public class ConfigKeys
        {
            public const string ComposeFile = "compose-file-path";
        }

        public class ExternalCommands
        {
            public const string Docker = "docker";
            public const string DockerPullArgList = "pull {0}";
            public const string DockerCompose = "docker-compose";
            public const string DockerComposeUp = "up -d";
            public const string DockerComposeDown = "down";
        }
    }
}
