using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOpsMonitorApi
{
    public interface ILogger
    {
        void Info(string text);
        void Error(string text);
    }
}
