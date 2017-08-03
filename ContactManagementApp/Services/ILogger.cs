using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagementApp.Services
{
    //TODO: Implement additional methods for more grainular logging and log level configuration
    //TODO: Use DI framework to easily switch between logging frameworks
    //TODO: Support different types of logs (web service, VM, model, repository, etc)
    interface ILogger
    {
        void LogError(string message);

        void LogInformation(string message);
    }
}
