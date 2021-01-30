using System;
using System.Collections.Generic;

namespace GothicModComposer.Utils
{
	public static class Logger
	{
		private static readonly List<string> Logs = new List<string>();
		private static readonly string CommandSeparator = $"{Environment.NewLine}{new string('-', 80)}{Environment.NewLine}";

		public static void Info(string message)
		{
			var value = $"[INFO] {message}";
			Logs.Add(value);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(value);
		}

		public static void Warn(string message)
		{
			var value = $"[WARN] {message}";
			Logs.Add(value);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(value);
		}

		public static void Error(string message)
		{
			var value = $"[ERROR] {message}";
			Logs.Add(value);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(value);
		}

		public static void StartCommand(string message)
		{
			var value = $"{CommandSeparator}[COMMAND] {message.ToUpper()}";
			Logs.Add(value);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(value);
		}

		public static void FinishCommand(string message)
		{
			var value = $"[COMMAND] {message}{CommandSeparator}";
			Logs.Add(value);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(value);
		}

		public static void StartCommandUndo(string message)
		{
			var value = $"{CommandSeparator}[UNDO COMMAND] {message.ToUpper()}";
			Logs.Add(value);
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine(value);
		}

		public static void FinishCommandUndo(string message)
		{
			var value = $"[UNDO COMMAND] {message}{CommandSeparator}";
			Logs.Add(value);
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine(value);
		}
	}
}