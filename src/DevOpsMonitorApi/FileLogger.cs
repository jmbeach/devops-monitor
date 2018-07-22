using System;
using System.IO;

namespace DevOpsMonitorApi
{
    public class FileLogger : ILogger
    {
        private string _fileName;

        public FileLogger(string fileName)
        {
            _fileName = fileName;
        }

        public void Info(string text)
        {
            using (var writer = new StreamWriter(_fileName, true))
            {
                writer.WriteLine($"INFO-{DateTime.Now}:\t{text}");
            }
        }

        public void Error(string text)
        {
            using (var writer = new StreamWriter(_fileName, true))
            {
                writer.WriteLine($"ERROR-{DateTime.Now}:\t{text}");
            }
        }
    }
}
