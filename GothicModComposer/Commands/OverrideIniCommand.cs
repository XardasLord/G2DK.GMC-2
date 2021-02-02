using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GothicModComposer.Models;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class OverrideIniCommand : ICommand
	{
		private readonly GothicFolder _gothicFolder;
		private readonly GmcFolder _gmcFolder;
		private readonly List<string> _iniOverrides;
		public string CommandName => "Override .ini file";

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
			{
				// TODO: Add to executed command list for undo purposes.
			}

			OverrideIni();
		}

		public void Undo()
		{
			Logger.Warn("Undo is not implemented");
		}

		private void OverrideIni()
		{
			if (!FileHelper.Exists(_gothicFolder.GothicIniFilePath))
				throw new Exception("Gothic.ini file was not found.");
			
			var gothicIni = _gothicFolder.GetGothicIniContent();
			var iniBlocks = IniFileHelper.CreateSections(gothicIni); // TODO: Replace IniBlock class to something like IniFile class
			OverrideAttributes(iniBlocks);
			SaveIniFile(iniBlocks);

			throw new NotImplementedException();
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
		}
	}
}