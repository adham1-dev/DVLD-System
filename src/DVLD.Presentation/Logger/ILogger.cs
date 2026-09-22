using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Presentation.Logger
{
    public interface ILogger
    {
        void StoreLog(string message, EventLogEntryType type);
    }
}
