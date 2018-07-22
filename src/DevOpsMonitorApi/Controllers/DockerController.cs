using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DevOpsMonitorApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DockerController : ControllerBase
    {
        private ILogger _logger;
        private IConfiguration _config;

        public DockerController(ILogger logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(Constants.AliveMessage);
        }

        [HttpPost]
        public void Post([FromBody] List<string> fullImageNames)
        {
            foreach (var imageName in fullImageNames)
            {
                var tagName = GetTagName(imageName);
                Environment.SetEnvironmentVariable(Constants.Environment.DockerImageTag, tagName);
                Process process = StartDockerPullProcess(imageName);

                LogInfo(process);

                var processErrorOutput = GetError(process);

                if (!string.IsNullOrEmpty(processErrorOutput))
                {
                    _logger.Error(processErrorOutput);
                    return;
                }
            }

            DockerComposeCommand(Constants.ExternalCommands.DockerComposeDown);

            DockerComposeCommand(Constants.ExternalCommands.DockerComposeUp);
        }

        private string GetTagName(string imageName)
        {
            var parts = imageName.Split(':');
            if (parts != null && parts.Length > 1)
            {
                return parts[parts.Length - 1];
            }

            return Constants.DefaultTag;
        }

        private string DockerComposeCommand(string argument)
        {
            var command = new ProcessStartInfo(Constants.ExternalCommands.DockerCompose)
            {
                Arguments = $"-f {_config[Constants.ConfigKeys.ComposeFile]} {argument}",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(command);

            if (argument != Constants.ExternalCommands.DockerComposeUp)
                LogInfo(process);

            return GetError(process);
        }

        private string GetError(Process process)
        {
            var processErrorOutput = string.Empty;
            while (!process.StandardError.EndOfStream)
            {
                var newLine = process.StandardError.ReadLine();
                processErrorOutput += $"\t{newLine}\n";
                _logger.Error(newLine);
            }

            return processErrorOutput;
        }

        private static Process StartDockerPullProcess(string imageName)
        {
            var dockerCommand = new ProcessStartInfo(Constants.ExternalCommands.Docker)
            {
                Arguments = String.Format(Constants.ExternalCommands.DockerPullArgList, imageName),
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(dockerCommand);
            return process;
        }

        private void LogInfo(Process process)
        {
            var processOutput = string.Empty;

            while (!process.StandardOutput.EndOfStream)
            {
                var newLine = process.StandardOutput.ReadLine();
                processOutput += $"\t{newLine}\n";
                _logger.Info(newLine);
            }
        }
    }
}