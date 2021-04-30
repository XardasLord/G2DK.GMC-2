﻿using System;
using System.Diagnostics;
using System.IO;
using GothicModComposer.UI.Enums;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.ViewModels;

namespace GothicModComposer.UI.Services
{
    public class GmcExecutor : IGmcExecutor
    {
        //private const string UpdateFileName = "Update.bat";
        //private const string ComposeFileName = "Compose.bat";
        //private const string RunModFileName = "RunMod.bat";
        //private const string RestoreGothicFileName = "RestoreGothic.bat";
        //private const string BuildModFileFileName = "BuildModFile.bat";
        //private const string EnableVDFFileName = "EnableVDF.bat";

        public void Execute(GmcExecutionProfile profile)
        {
#if DEBUG
            var gmcLocation = @"C:\localRepository\Gothic 2 Dzieje Khorinis\GMC-2\GothicModComposer\bin\Debug\net5.0-windows";
#else
            var gmcLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Release");
#endif

            var settingsVM = new GmcSettingsVM();

            //var fileNameToRun = GetFileNameToExecute(profile, gmcLocation);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(gmcLocation, "GMC-2.exe"),
                    ArgumentList =
                    {
                        $"--gothic2Path={settingsVM.GmcConfiguration.Gothic2RootPath}",
                        $"--modPath={settingsVM.GmcConfiguration.ModificationRootPath}",
                        $"--profile={profile}",
                        $"--configurationFile={settingsVM.GmcSettingsJsonFilePath}"

                    },
                    UseShellExecute = false
                }
            };

            process.Start();
            process.WaitForExit();
        }

        //private static string GetFileNameToExecute(GmcExecutionProfile profile, string gmcLocation)
        //{
        //    return profile switch
        //    {
        //        GmcExecutionProfile.Update => Path.Combine(gmcLocation, "..", UpdateFileName),
        //        GmcExecutionProfile.Compose => Path.Combine(gmcLocation, "..", ComposeFileName),
        //        GmcExecutionProfile.RunMod => Path.Combine(gmcLocation, "..", RunModFileName),
        //        GmcExecutionProfile.RestoreGothic => Path.Combine(gmcLocation, "..", RestoreGothicFileName),
        //        GmcExecutionProfile.BuildModFile => Path.Combine(gmcLocation, "..", BuildModFileFileName),
        //        GmcExecutionProfile.EnableVDF => Path.Combine(gmcLocation, "..", EnableVDFFileName),
        //        _ => throw new ArgumentOutOfRangeException(nameof(profile))
        //    };
        //}
    }
}