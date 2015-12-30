using System.Collections.Generic;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Provider
{
    public class DebugContext : IDebugContext
    {
        public LogLevel DebugMode { get; set; }
        public DebugContext()
        {
            Context = new List<object>();
        }

        public List<object> Context { get; set; }

        public void Log(string format, params object[] args)
        {
            Log(LogLevel.All, format, args);
        }

        public void Log(LogLevel debugMode, string format, params object[] args)
        {
            if (debugMode <= DebugMode)
            {
                Context.Add(string.Format(format, args));
            }
        }

        public void Log(object data)
        {
            Log(LogLevel.All, data);
        }

        public void Log(LogLevel debugMode, object data)
        {
            if (debugMode <= DebugMode)
            {
                Context.Add(data);
            }
        }
    }
}
