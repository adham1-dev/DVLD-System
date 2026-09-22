using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Presentation.Logger.StoringStrategies
{
    public class EventViewStoringStrategy : ILogger
    {
        string _sourceName;
        public EventViewStoringStrategy(string sourceName)
        {
            _sourceName = sourceName;
            if (!EventLog.SourceExists(_sourceName))
            {
                EventLog.CreateEventSource(_sourceName, "Application");
            }
        }

        public void StoreLog(string message, EventLogEntryType type)
        {
            try
            {
                EventLog.WriteEntry(_sourceName, message, type);
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }
        }

    }
}
