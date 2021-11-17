using System;
using Serilog;

namespace GothicModComposer.Core.Utils
{
    public static class Logger
    {
        private static readonly string CommandSeparator =
            $"{Environment.NewLine}{new string('-', 100)}{Environment.NewLine}";

        public static void Info(string message, bool display = false)
        {
            var value = $"[INFO] {message}";
            Log.Debug(message);

            if (display)
            {
                Log.Information(value);
            }
        }

        public static void Warn(string message)
        {
            var value = $"[WARN] {message}";
            Log.Warning(value);
        }

        public static void Error(string message)
        {
            var value = $"[ERROR] {message}";
            Log.Error(value);
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
        }

        public static void FinishCommand(string message)
        {
            var value = $"[COMMAND] {message}{CommandSeparator}";
            Log.Information(value);
        }

        public static void StartCommandUndo(string message)
        {
            var value = $"{CommandSeparator}[UNDO COMMAND] {message.ToUpper()}";
            Log.Information(value);
        }

        public static void FinishCommandUndo(string message)
        {
            var value = $"[UNDO COMMAND] {message}{CommandSeparator}";
            Log.Information(value);
        }

        public static void SaveLogs()
        {
            Log.CloseAndFlush();
        }
    }
}