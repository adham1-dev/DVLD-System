using DVLD.Presentation.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.Logger
{
    public class AppLogger
    {
        public static ILogger Provider { get; set; }


        //This method for Windows forms only
        private static MethodBase GetCallerMethod()
        {
            StackFrame[] frames = new StackTrace(true).GetFrames();

            if (frames == null)
                return null;

            foreach (StackFrame frame in frames)
            {
                MethodBase method = frame.GetMethod();
                Type type = method?.DeclaringType;

                if (type == null)
                    continue;

                if (typeof(Control).IsAssignableFrom(type))
                    return method;
            }

            return null;
        }


        static string Format(string cls, string meth, string msg, Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"[Location] {cls} -> {meth}");

            if(!string.IsNullOrEmpty(msg))
                sb.AppendLine($"[Message] {msg}");

            if (ex == null)
                return sb.ToString();

            sb.AppendLine($"[Exception] {ex.Message}");
            sb.AppendLine($"\n\n-------------------------------------\n");
            sb.Append(FormatStackTrace(ex));

            return sb.ToString();
        }

        private static string FormatStackTrace(Exception ex)
        {
            if (ex == null)
                return string.Empty;

            var trace = new StackTrace(ex, true);
            var frames = trace.GetFrames();

            if (frames == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[Stack Trace]");

            int index = 1;

            foreach (var frame in frames)
            {
                MethodBase method = frame.GetMethod();

                if (method == null)
                    continue;

                string className = method.DeclaringType?.Name ?? "Unknown";
                string methodName = method.Name;

                string parameters = string.Join(", ",
                    method.GetParameters()
                          .Select(p => $"{p.ParameterType.Name} {p.Name}"));

                int line = frame.GetFileLineNumber();

                sb.AppendLine(
                    $"{index++}. {className} -> {methodName}({parameters})" +
                    (line > 0 ? $"   (Line {line})" : ""));
            }

            return sb.ToString();
        }


        static void BuildAndWriteLog(string message, EventLogEntryType type, Exception ex = null)
        {
            var method = GetCallerMethod();

            string log = Format(
                method?.DeclaringType?.FullName ?? "UnknownClass",
                method?.Name ?? "UnknownMethod",
                message,
                ex);

            Provider.StoreLog(log, type);
        }

        //Log Infos
        public static void LogInfo(string infoMessage) => BuildAndWriteLog(infoMessage, EventLogEntryType.Information);
        public static void LogWarning(string warningMessage) => BuildAndWriteLog(warningMessage, EventLogEntryType.Warning);


        //Log errors (catch it from ex)
        public static void LogError(Exception ex) => BuildAndWriteLog(ex.Message, EventLogEntryType.Error, ex);
        public static void LogError(string userMessage, Exception ex) => BuildAndWriteLog(userMessage, EventLogEntryType.Error, ex);


    }
}
