using System;
using System.Diagnostics;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Provider
{
    public class Profiler : IDisposable
    {
        private readonly Stopwatch _watch;
        private readonly IDebugContext _debugContext;
        private readonly LogLevel _logLevel;
        private readonly string _name;

        public Profiler(IDebugContext debugContext, LogLevel logLevel, string name = null)
        {
            _debugContext = debugContext;
            _logLevel = logLevel;
            _name = name;

            _watch = Stopwatch.StartNew();
            _debugContext.Log(logLevel, "Start: {0}", _name ?? String.Empty);
        }

        public Profiler(IDebugContext debugContext, string name = null) :
            this(debugContext, LogLevel.All, name)
        {
        }

        public void Dispose()
        {
            _watch.Stop();
            _debugContext.Log(_logLevel, "End: {0} - Execution time: {1} ms", _name ?? String.Empty, _watch.ElapsedMilliseconds);
        }
    }
}
