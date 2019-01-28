using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EynwaDiscordBot.Models.Diagnostics
{
    public class DebugProcessTimer : IDisposable
    {
#if DEBUG
        private readonly DateTime start = DateTime.Now;
        private readonly string label;

        public DebugProcessTimer(string label)
        {
            this.label = label;
        }

        public void Dispose()
        {
            Debug.WriteLine($"[ExecutionTime - {label}] : {this.EllapsedMilliseconds()}ms");
        }

        public double EllapsedMilliseconds()
        {
            return (DateTime.Now - this.start).TotalMilliseconds;
        }
#else
        public DebugProcessTimer(string label)
        {
        }

        public void Dispose()
        {
        }

        public double EllapsedMilliseconds()
        {
            return 0;
        }
#endif
    }
}
