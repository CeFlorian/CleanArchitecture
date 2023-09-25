using NorthWind.Entities.Interfaces;
using System.Diagnostics;

namespace NorthWind.Loggers
{
    public class DebugStatusLogger : IApplicationStatusLogger
    {
        public async Task Log(string message)
        {
            Debug.WriteLine($"*** DSL: {message}");
        }
    }
}