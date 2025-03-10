using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.sharedLibrary.Logs
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebugger(ex.Message);

        }

        public static void LogToFile(string Message) => Log.Information(Message);

        public static void LogToDebugger(string Message) => Log.Debug(Message);

        public static void LogToConsole(string Message) => Log.Warning(Message);




    }
}
