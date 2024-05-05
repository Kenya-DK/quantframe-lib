using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.Utils
{
    [Flags]
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Debug,
        Trace,
        Critical,
    }
    public class Logger
    {
        public static LogLevel LogLevel = LogLevel.Info;
        private static string LogLevelToString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return "INFO";
                case LogLevel.Warning:
                    return "WARNING";
                case LogLevel.Error:
                    return "ERROR";
                case LogLevel.Debug:
                    return "DEBUG";
                case LogLevel.Trace:
                    return "TRACE";
                case LogLevel.Critical:
                    return "CRITICAL";
                default:
                    return "UNKNOWN";
            }
        }
        private static ConsoleColor LogLevelToColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return ConsoleColor.DarkGreen;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Debug:
                    return ConsoleColor.Green;
                case LogLevel.Trace:
                    return ConsoleColor.Magenta;
                case LogLevel.Critical:
                    return ConsoleColor.DarkRed;
                default:
                    return ConsoleColor.White;
            }
        }
        public static void WriteWithColor(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void WriteBox(ConsoleColor color, string message)
        {
            WriteWithColor(ConsoleColor.Cyan, "[");
            WriteWithColor(color, message);
            WriteWithColor(ConsoleColor.Cyan, "] ");
        }
        public static void Log(LogLevel level, string component, string message)
        {
            // Console.ForegroundColor = ConsoleColor.Cyan;
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WriteBox(ConsoleColor.White, time);
            WriteBox(LogLevelToColor(level), LogLevelToString(level));
            WriteBox(ConsoleColor.Magenta, component);
            Console.WriteLine(message);
        }
        public static void Info(string component, string message)
        {
            Log(LogLevel.Info, component, message);
        }
        public static void Warning(string component, string message)
        {
            Log(LogLevel.Warning, component, message);
        }
        public static void Error(string component, string message)
        {
            Log(LogLevel.Error, component, message);
        }
        public static void Debug(string component, string message)
        {
            Log(LogLevel.Debug, component, message);
        }
        public static void Trace(string component, string message)
        {
            Log(LogLevel.Trace, component, message);
        }
        public static void Critical(string component, string message)
        {
            Log(LogLevel.Critical, component, message);
        }
    }
}
