using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.Daedalus;
using GothicModComposer.Utils.IOHelpers;
using GothicModComposer.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Commands
{
	// TODO: NEEDS REFACTOR, LOGIC COPIED FROM OLD GMC TOOL
	public class UpdateDialoguesCommand : ICommand
	{
		public string CommandName => "Update dialogues";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

        private const string OuCslFileName = "OU.CSL";
        private const string OuBinFileName = "OU.BIN";

        public UpdateDialoguesCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
		{
            if (!_profile.CommandsConditions.UpdateDialoguesStepRequired)
            {
                if (!FileHelper.Exists(Path.Combine(_profile.GothicFolder.CutsceneFolderPath, OuCslFileName)))
                {
                    Logger.Warn("Update dialogues is not required, but OU.CSL file does not exist so this step is needed.");
                }
                else
                {
                    Logger.Info("Update dialogues is not required, so this step can be skipped.", true);
                    return;
                }
            }

            var scriptFilesPaths = ScriptTreeReader.Parse(_profile.GothicFolder.GothicSrcFilePath);
			var dialoguePopupsRecords = ReadDialoguesFromScripts(scriptFilesPaths);
			
			var ouBinFilePath = Path.Combine(_profile.GothicFolder.CutsceneFolderPath, OuBinFileName);
			if (FileHelper.Exists(ouBinFilePath))
			{
				var tmpCommandActionBackupPath =
					Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), Path.GetFileName(ouBinFilePath));

				FileHelper.CopyWithOverwrite(ouBinFilePath, tmpCommandActionBackupPath);
				FileHelper.DeleteIfExists(ouBinFilePath);

				ExecutedActions.Push(CommandActionIO.FileDeleted(ouBinFilePath, tmpCommandActionBackupPath));
			}

			var ouCslPath = Path.Combine(_profile.GothicFolder.CutsceneFolderPath, OuCslFileName);
			
			if (FileHelper.Exists(ouCslPath))
			{
				var tmpCommandActionBackupPath =
					Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), Path.GetFileName(ouCslPath));

				FileHelper.CopyWithOverwrite(ouCslPath, tmpCommandActionBackupPath);

                GenerateOuCslFile(ouCslPath, dialoguePopupsRecords);

                ExecutedActions.Push(CommandActionIO.FileCopiedWithOverwrite(ouCslPath, tmpCommandActionBackupPath));
			}
			else
			{
                GenerateOuCslFile(ouCslPath, dialoguePopupsRecords);

				ExecutedActions.Push(CommandActionIO.FileCreated(ouCslPath));
			}

			Logger.Info("Updated dialogues in OU.CSL file.");
		}

        public void Undo() => ExecutedActions.Undo();

		private static List<Tuple<string, string>> ReadDialoguesFromScripts(List<string> scriptPaths)
		{
			var dialogues = new List<Tuple<string, string>>();

			using (var progress = new ProgressBar(scriptPaths.Count, "Updating dialogues", ProgressBarOptionsHelper.Get()))
			{
				var counter = 1;

				Parallel.ForEach(scriptPaths, script =>
                {
					dialogues.AddRange(GetMatchingDialoguesFromFile(script,
						$"{GothicRegexHelper.MultiLineComment}|{GothicRegexHelper.SvmPattern}"));
					dialogues.AddRange(GetMatchingDialoguesFromFile(script,
						$"{GothicRegexHelper.MultiLineComment}|{GothicRegexHelper.AiOutputPattern}"));

					progress.Tick($"Updated {counter++} of {scriptPaths.Count} dialogues");
				});
			}

			Logger.Info("Svm and AI_Output count: " + dialogues.Count);

			return dialogues;
		}

		private static IEnumerable<Tuple<string, string>> GetMatchingDialoguesFromFile(string filepath, string pattern)
		{
			var content = File.ReadAllText(filepath, EncodingHelper.GothicEncoding);
			var collection = new Regex(pattern, RegexOptions.Multiline).Matches(content);
			var list = new List<Tuple<string, string>>();

			foreach (Match match in collection)
			{
				if (match.Groups["Comment"].Success || match.Groups["Dialogue"].Value == string.Empty)
					continue;

				list.Add(new Tuple<string, string>(match.Groups["Identifier"].Value, match.Groups["Dialogue"].Value));
			}

			return list;
		}

        private static void GenerateOuCslFile(string ouCslPath, List<Tuple<string, string>> dialoguePopupsRecords)
        {
            Logger.Info("Generating OU.CSL file started.");

            File.WriteAllText(ouCslPath, CslWriter.GenerateContent(dialoguePopupsRecords), EncodingHelper.GothicEncoding);

            Logger.Info("Generating OU.CSL file completed.");
        }
	}
}