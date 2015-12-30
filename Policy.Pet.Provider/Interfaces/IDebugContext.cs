using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policy.Pets.Provider.Interfaces
{
    public interface IDebugContext
    {
        LogLevel DebugMode { get; set; }
        List<object> Context { get; set; }
        void Log(string format, params object[] args);
        void Log(object data);
        void Log(LogLevel debugMode, object data);
        void Log(LogLevel debugMode, string format, params object[] args);
    }
}
