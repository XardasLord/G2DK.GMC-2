﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.IniFiles;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers.FileSystem;

namespace GothicModComposer.Commands
{
    public class OverrideIniCommand : ICommand
	{
		public string CommandName => "Override .ini file";

		private readonly IProfile _profile;
        private readonly IFileSystemWithLogger _fileSystem;
        private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public OverrideIniCommand(IProfile profile, IFileSystemWithLogger fileSystem)
        {
            _profile = profile;
            _fileSystem = fileSystem;
        }

        public void Execute()
		{
			if (!_profile.IniOverrides.Any())
			{
				Logger.Info("There is no .ini attributes to override.", true);
				return;
			}

			if (_profile.GmcFolder.CreateGmcFolder())
				ExecutedActions.Push(CommandActionIO.DirectoryCreated(_profile.GmcFolder.BasePath));

			OverrideIni();
		}

		public void Undo() => ExecutedActions.Undo();

		private void OverrideIni()
		{
			if (!_fileSystem.File.Exists(_profile.GothicFolder.GothicIniFilePath))
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

                var key = attribute.Groups["Key"].Value;
                var value = attribute.Groups["Value"].Value;

                var blockToOverride = iniBlocks.FirstOrDefault(block => block.Contains(key));
				if (blockToOverride is null)
					return;
				
                var overridesSectionBlock = iniBlocks.FirstOrDefault(block => block.Header.Equals(IniFileHelper.OverridesSectionHeader));
				if (overridesSectionBlock is null)
					iniBlocks.Add(new IniBlock(IniFileHelper.OverridesSectionHeader));

                overridesSectionBlock = iniBlocks.Single(block => block.Header.Equals(IniFileHelper.OverridesSectionHeader));
				overridesSectionBlock.Set($"{blockToOverride.Header}.{key}", value);
				
                Logger.Info($"Overriden {blockToOverride.Header}.{key}={value}.", true);
			});

		}

		private void SaveIniFile(List<IniBlock> iniBlocks)
		{
			_profile.GothicFolder.SaveGmcIni(iniBlocks);

			Logger.Info($"Created file {_profile.GothicFolder.GmcIniFilePath}.", true);

			ExecutedActions.Push(CommandActionIO.FileCreated(_profile.GothicFolder.GmcIniFilePath));
		}
	}
}