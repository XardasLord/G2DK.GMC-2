﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using CommandLine;
using GothicModComposer.Builders;
using GothicModComposer.Models;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer
{
	internal class Launcher
	{
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

		[STAThread]
		private static void Main(string[] args)
        {
            SetTitle();
			MaximizeWindow();

			Parser.Default.ParseArguments<GmcInitialParameter>(args)
				.WithParsed(RunGmc);

			Console.ReadKey();
		}

        private static void RunGmc(GmcInitialParameter parameters)
		{
            var fullVersion = Assembly.GetExecutingAssembly().GetName().Version;
			var stopWatch = new Stopwatch();

			var gmcManager = GmcManagerBuilder.PrepareGmcExecutor(parameters.Profile, parameters.AbsolutePathToProject, parameters.AbsolutePathToGothic2Game);

			try
			{
				stopWatch.Start();
                Logger.Info($"GMC v{fullVersion?.Major}.{fullVersion?.Minor}.{fullVersion?.Build}", true);
				Logger.Info($"GMC build with profile {parameters.Profile} started...", true);

				gmcManager.Run();

				stopWatch.Stop();
				Logger.Info($"GMC build finished. Execution time: {stopWatch.Elapsed}", true);
				Logger.Info("You can close the application.", true);
			}
			catch (Exception e)
			{
				Logger.Error(e.Message);

				Console.WriteLine("Press any key to start the undo changes process...");
				Console.ReadKey();

				stopWatch.Restart();
				Logger.Info("GMC undo changes started...", true);

				gmcManager.Undo();

				stopWatch.Stop();
				Logger.Info($"GMC undo changes finished. Execution time: {stopWatch.Elapsed}", true);
				Logger.Info("You can close the application.", true);
			}
			finally
			{
				Logger.SaveLogs(gmcManager.GmcFolder.LogsFolderPath);
			}
		}

        private static void SetTitle()
        {
            var fullVersion = Assembly.GetExecutingAssembly().GetName().Version;

			Console.Title = $"GMC v{fullVersion?.Major}.{fullVersion?.Minor}.{fullVersion?.Build}";
        }

		private static void MaximizeWindow()
		{
			var p = Process.GetCurrentProcess();
			ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
		}
	}
}
