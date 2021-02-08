using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.IniFiles;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class OverrideIniCommand : ICommand
	{
		public string CommandName => "Override .ini file";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public OverrideIniCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
		{
			if (!_profile.IniOverrides.Any())
			{
				Logger.Info("There is no .ini attributes to override.");
				return;
			}

			if (DirectoryHelper.CreateIfDoesNotExist(_profile.GmcFolder.BasePath))
				ExecutedActions.Push(CommandActionIO.DirectoryCreated(_profile.GmcFolder.BasePath));

			OverrideIni();
		}

		public void Undo() => ExecutedActions.Undo();

		private void OverrideIni()
		{
			if (!FileHelper.Exists(_profile.GothicFolder.GothicIniFilePath))
				throw new Exception("Gothic.ini file was not found.");
			
			var gothicIni = _profile.GothicFolder.GetGothicIniContent();
			var iniBlocks = IniFileHelper.CreateSections(gothicIni); // TODO: Replace IniBlock class to something like IniFile class
			OverrideAttributes(iniBlocks);
			SaveIniFile(iniBlocks);
		}

		private void OverrideAttributes(List<IniBlock> iniBlocks)
		{
			var regex = new Regex(IniFileHelper.AttributeRegex);

			_profile.IniOverrides.ForEach(item => {
				var attribute = regex.Match(item);

				iniBlocks.ForEach(block => {
					var key = attribute.Groups["Key"].Value;
					var value = attribute.Groups["Value"].Value;

					if (!block.Contains(key)) 
						return;

					block.Set(key, value);
					Logger.Info($"Overriden {key}={value} in section: [{block.Header}].");
				});
			});

		}

		private void SaveIniFile(List<IniBlock> iniBlocks)
		{
			_profile.GothicFolder.SaveGmcIni(iniBlocks);

			Logger.Info($"Created file {_profile.GothicFolder.GmcIniFilePath}.");

			ExecutedActions.Push(CommandActionIO.FileCreated(_profile.GothicFolder.GmcIniFilePath));
		}
	}
}