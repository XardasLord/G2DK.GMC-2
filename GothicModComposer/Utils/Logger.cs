using System;
using GothicModComposer.Utils.IOHelpers;
using Serilog;

namespace GothicModComposer.Utils
{
    public static class Logger
    {
        private static readonly string CommandSeparator =
            $"{Environment.NewLine}{new string('-', 100)}{Environment.NewLine}";

        public static void Info(string message, bool display = false)
        {
            var value = $"[INFO] {message}";
            Log.Information(message);

            if (display)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(value);
            }
        }

        public static void Warn(string message)
        {
            var value = $"[WARN] {message}";
            Log.Warning(message);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
        }

        public static void Error(string message)
        {
            var value = $"[ERROR] {message}";
            Log.Error(message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
        }

        public static void zLog(string message)
        {
            var value = $"[zLog] {message}";
            Log.Information(value);
        }

        public static void StartCommand(string message)
        {
            var value = $"{CommandSeparator}[COMMAND] {message.ToUpper()}";
            Log.Information(value);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
        }

        public static void FinishCommand(string message)
        {
            var value = $"[COMMAND] {message}{CommandSeparator}";
            Log.Information(value);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
        }

        public static void StartCommandUndo(string message)
        {
            var value = $"{CommandSeparator}[UNDO COMMAND] {message.ToUpper()}";
            Log.Information(value);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(value);
        }

        public static void FinishCommandUndo(string message)
        {
            var value = $"[UNDO COMMAND] {message}{CommandSeparator}";
            Log.Information(value);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(value);
        }

        public static void SaveLogs()
        {
            Log.CloseAndFlush();
        }
    }
}