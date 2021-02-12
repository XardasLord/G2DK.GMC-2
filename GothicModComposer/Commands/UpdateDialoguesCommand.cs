using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.Daedalus;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	// TODO: NEEDS REFACTOR, LOGIC COPIED FROM OLD GMC TOOL
	public class UpdateDialoguesCommand : ICommand
	{
		public string CommandName => "Update dialogues";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public UpdateDialoguesCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
		{
			var scriptFilesPaths = ScriptTreeReader.Parse(_profile.GothicFolder.GothicSrcFilePath);
			var dialoguePopupsRecords = ReadDialoguesFromScripts(scriptFilesPaths);
			
			var ouBinFilePath = Path.Combine(_profile.GothicFolder.CutsceneFolderPath, "OU.BIN");
			if (FileHelper.Exists(ouBinFilePath))
			{
				var tmpCommandActionBackupPath =
					Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), Path.GetFileName(ouBinFilePath));

				FileHelper.Copy(ouBinFilePath, tmpCommandActionBackupPath);
				FileHelper.DeleteIfExists(ouBinFilePath);

				ExecutedActions.Push(CommandActionIO.FileDeleted(ouBinFilePath, tmpCommandActionBackupPath));
			}

			var ouCslPath = Path.Combine(_profile.GothicFolder.CutsceneFolderPath, "OU.CSL");
			
			if (FileHelper.Exists(ouCslPath))
			{
				var tmpCommandActionBackupPath =
					Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), Path.GetFileName(ouCslPath));

				FileHelper.Copy(ouCslPath, tmpCommandActionBackupPath);

				File.WriteAllText(ouCslPath, CslWriter.GenerateContent(dialoguePopupsRecords), EncodingHelper.GothicEncoding);
				ExecutedActions.Push(CommandActionIO.FileCopiedWithOverwrite(ouCslPath, tmpCommandActionBackupPath));
			}
			else
			{
				File.WriteAllText(ouCslPath, CslWriter.GenerateContent(dialoguePopupsRecords), EncodingHelper.GothicEncoding);
				ExecutedActions.Push(CommandActionIO.FileCreated(ouCslPath));
			}

			Logger.Info("Updated dialogues in OU.CSL file");
		}

		public void Undo() => ExecutedActions.Undo();

		private static List<Tuple<string, string>> ReadDialoguesFromScripts(List<string> scriptPaths)
		{
			var dialogues = new List<Tuple<string, string>>();

			scriptPaths.ForEach(script => {
				dialogues.AddRange(GetMatchingDialoguesFromFile(script,
					$"{GothicRegexHelper.MultiLineComment}|{GothicRegexHelper.SvmPattern}"));
				dialogues.AddRange(GetMatchingDialoguesFromFile(script,
					$"{GothicRegexHelper.MultiLineComment}|{GothicRegexHelper.AiOutputPattern}"));
			});

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
	}
}