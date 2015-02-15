using System.Diagnostics;
using Splat;

namespace PromotionViabilityWpf
{
    class DebugLogger : ILogger
    {
        public void Write(string message, LogLevel logLevel)
        {
            Debug.WriteLine(message);
        }

        public LogLevel Level
        { get; set; }
    }
}
