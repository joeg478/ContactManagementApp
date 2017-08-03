using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagementApp.Services
{
    public class EventViewerLogger : ILogger
    {
        private const string SOURCE = "ContactManagement";
        private const string LOG_NAME = "Application";

        public void LogError(string message)
        {
            WriteEntry(message, EventLogEntryType.Error);
        }

        public void LogInformation(string message)
        {
            WriteEntry(message, EventLogEntryType.Information);
        }

        private void WriteEntry(string message, EventLogEntryType entrytype)
        {
            if (!EventLog.SourceExists(SOURCE))
                EventLog.CreateEventSource(SOURCE, LOG_NAME);

            EventLog.WriteEntry(SOURCE, message, entrytype);
        }
    }
}
