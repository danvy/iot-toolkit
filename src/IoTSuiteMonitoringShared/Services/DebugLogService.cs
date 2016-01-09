using System.Diagnostics;

namespace Danvy.Services
{
    public class DebugLogService : ILogService
    {
        public void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }
    }
}
