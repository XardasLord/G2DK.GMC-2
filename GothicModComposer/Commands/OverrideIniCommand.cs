using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class OverrideIniCommand : ICommand
	{
		public string CommandName => "Override .ini file";

		private readonly GothicFolder _gothicFolder;
		private readonly GmcFolder _gmcFolder;
		private readonly List<string> _iniOverrides;

		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public OverrideIniCommand(GothicFolder gothicFolder, GmcFolder gmcFolder, List<string> iniOverrides)
		{
			_gothicFolder = gothicFolder;
			_gmcFolder = gmcFolder;
			_iniOverrides = iniOverrides;
		}

		public void Execute()
		{
			if (!_iniOverrides.Any())
			{
				Logger.Info("There is no .ini attributes to override.");
				return;
			}

			if (DirectoryHelper.CreateIfDoesNotExist(_gmcFolder.BasePath))
				ExecutedActions.Push(new CommandActionIO(CommandActionIOType.DirectoryCreate, null, _gmcFolder.BasePath));

			OverrideIni();
		}

		public void Undo()
		{
			if (!ExecutedActions.Any())
			{
				Logger.Info("There is nothing to undo, because no actions were executed."); // TODO: Introduce something like `NoExecutedAction.Undo();`
				return;
			}

			while (ExecutedActions.Count > 0)
			{
				var executedAction = ExecutedActions.Pop();
				executedAction?.Undo();
			}
		}

		private void OverrideIni()
		{
			if (!FileHelper.Exists(_gothicFolder.GothicIniFilePath))
				throw new Exception("Gothic.ini file was not found.");
			
			var gothicIni = _gothicFolder.GetGothicIniContent();
			var iniBlocks = IniFileHelper.CreateSections(gothicIni); // TODO: Replace IniBlock class to something like IniFile class
			OverrideAttributes(iniBlocks);
			SaveIniFile(iniBlocks);
		}

		private void OverrideAttributes(List<IniBlock> iniBlocks)
		{
			var regex = new Regex(IniFileHelper.AttributeRegex);

			_iniOverrides.ForEach(item => {
				var attribute = regex.Match(item);

				iniBlocks.ForEach(block => {
					var key = attribute.Groups["Key"].Value;
					var value = attribute.Groups["Value"].Value;

					if (!block.Contains(key)) 
						return;

					block.Set(key, value);
					Logger.Info($"Overriden {key}={value} in section: [{block.Header}]");
				});
			});

		}

		private void SaveIniFile(List<IniBlock> iniBlocks)
		{
			_gothicFolder.SaveGmcIni(iniBlocks);

			Logger.Info($"Created file {_gothicFolder.GmcIniFilePath}.");

			ExecutedActions.Push(new CommandActionIO(CommandActionIOType.FileCreate, null, _gothicFolder.GmcIniFilePath));
		}
	}
}