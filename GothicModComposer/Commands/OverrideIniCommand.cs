using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Configurations;
using GothicModComposer.Models.IniFiles;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;
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
				Logger.Info("There is no .ini attributes to override.", true);

			if (!_profile.IniOverridesSystemPack.Any())
				Logger.Info("There is no System Pack .ini attributes to override.", true);

			if (!_profile.IniOverrides.Any() && !_profile.IniOverridesSystemPack.Any())
				return;

			if (_profile.GmcFolder.CreateGmcFolder())
				ExecutedActions.Push(CommandActionIO.DirectoryCreated(_profile.GmcFolder.BasePath));

			OverrideIni();
		}

		public void Undo() => ExecutedActions.Undo();

		private void OverrideIni()
		{
			if (!_fileSystem.File.Exists(_profile.GothicFolder.GothicIniFilePath))
				throw new Exception("Gothic.ini file was not found.");
			
			// if (!_fileSystem.File.Exists(_profile.GothicFolder.SystemPackIniFilePath))
			// 	throw new Exception("SystemPack.ini file was not found.");

			var world = _profile.ProfileType == ProfilePresetType.RunMod ? _profile.DefaultWorld : string.Empty;
			
			var defaultGmcIniBlocks = GmcIniHelper.GetDefaultGmcIni(world);

			var systemPackIni = _profile.GothicFolder.GetSystemPackIniContent();
			var systemPackIniBlocks = IniFileHelper.CreateSections(systemPackIni); // TODO: Replace IniBlock class to something like IniFile class
			
			OverrideGothicIniAttributes(defaultGmcIniBlocks, systemPackIniBlocks);

			// Merge
			// defaultGmcIniBlocks.AddRange(systemPackIniBlocks);
				
			SaveIniFile(defaultGmcIniBlocks);
		}

		private void OverrideGothicIniAttributes(ICollection<IniBlock> gothicIniBlocks, ICollection<IniBlock> systemPackIniBlocks)
		{
			var regex = new Regex(IniFileHelper.AttributeRegex);

			_profile.IniOverrides.ForEach(item =>
			{ 
				VerifySingleIniItem(gothicIniBlocks, regex, item, false);
			});

			// _profile.IniOverridesSystemPack.ForEach(item =>
			// { 
			// 	VerifySingleIniItem(systemPackIniBlocks, regex, item, true);
			// });
		}

		private static void VerifySingleIniItem(ICollection<IniBlock> iniBlocks, Regex regex, IniOverride item, bool isSystemPack = false) // TODO: Refactor parameters
        {
            var formatItem = $"{item.Key}={item.Value}";

			var attribute = regex.Match(formatItem);

			var section = item.Section;
			var key = attribute.Groups["Key"].Value;
			var value = attribute.Groups["Value"].Value;
			
			var overrideSectionHeaderName = isSystemPack
				? IniFileHelper.OverridesSystemPackSectionHeader
				: IniFileHelper.OverridesGothicSectionHeader;
			
			var overridesSectionBlock = iniBlocks.FirstOrDefault(block => block.Header.Equals(overrideSectionHeaderName));
			if (overridesSectionBlock is null)
				iniBlocks.Add(new IniBlock(overrideSectionHeaderName));
			
			overridesSectionBlock = iniBlocks.Single(block => block.Header.Equals(overrideSectionHeaderName));

			var blockToOverride = iniBlocks.FirstOrDefault(block => block.Contains(key));
			if (blockToOverride is null)
			{
				// Not found in default ini section, so we only add
				overridesSectionBlock.Set($"{section}.{key}", value);
				
				Logger.Info($"Overriden {section}.{key}={value}", true);
			}
			else
			{
				overridesSectionBlock.Set($"{blockToOverride.Header}.{key}", value);

				Logger.Info($"Overriden {blockToOverride.Header}.{key}={value}", true);
			
				// Delete from original ini section
				blockToOverride.Remove(key);
			
				if (!blockToOverride.Properties.Any())
					iniBlocks.Remove(blockToOverride);
			}
        }

		private void SaveIniFile(List<IniBlock> iniBlocks)
		{
			_profile.GothicFolder.SaveGmcIni(iniBlocks);

			Logger.Info($"Created file {_profile.GothicFolder.GmcIniFilePath}.", true);

			ExecutedActions.Push(CommandActionIO.FileCreated(_profile.GothicFolder.GmcIniFilePath));
		}
	}
}