using System;
using System.Diagnostics;
using System.IO;
using GothicModComposer.UI.Application;

namespace GothicModComposer.UI.Infrastructure
{
    public class GmcExecutor : IGmcExecutor
    {
        private const string UpdateFileName = "Update.bat";
        private const string ComposeFileName = "Compose.bat";
        private const string RunModFileName = "RunMod.bat";
        private const string RestoreGothicFileName = "RestoreGothic.bat";
        private const string BuildModFileFileName = "BuildModFile.bat";
        private const string EnableVDFFileName = "EnableVDF.bat";

        public void Execute(GmcExecutionProfile profile)
        {
#if DEBUG
            var gmcLocation = @"F:\Gry\Gothic II Dzieje Khorinis\TheHistoryOfKhorinis\_Tools\GMC 2\GMC UI Release";
#else 
            var gmcLocation = AppDomain.CurrentDomain.BaseDirectory;
#endif

            var fileNameToRun = GetFileNameToExecute(profile, gmcLocation);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileNameToRun,
                    UseShellExecute = true
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static string GetFileNameToExecute(GmcExecutionProfile profile, string gmcLocation)
        {
            return profile switch
            {
                GmcExecutionProfile.Update => Path.Combine(gmcLocation, "..", UpdateFileName),
                GmcExecutionProfile.Compose => Path.Combine(gmcLocation, "..", ComposeFileName),
                GmcExecutionProfile.RunMod => Path.Combine(gmcLocation, "..", RunModFileName),
                GmcExecutionProfile.RestoreGothic => Path.Combine(gmcLocation, "..", RestoreGothicFileName),
                GmcExecutionProfile.BuildModFile => Path.Combine(gmcLocation, "..", BuildModFileFileName),
                GmcExecutionProfile.EnableVDF => Path.Combine(gmcLocation, "..", EnableVDFFileName),
                _ => throw new ArgumentOutOfRangeException(nameof(profile))
            };
        }
    }
}