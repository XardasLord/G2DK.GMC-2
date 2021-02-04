using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GothicModComposer.Models.IniFiles
{
	public static class IniFileHelper
	{
		public const string CommentRegex = @"(?<Comment>^;.*)";
		public const string SectionRegex = @"\[(?<Header>\w+)\]\n(?<Attributes>[\s\S]+?(?![^[]))";
		public const string AttributeRegex = @"(?<Key>\w+)\s*=\s*?(?<Value>.*)";

		public static List<IniBlock> CreateSections(string iniFileContent)
		{
			var sectionRegex = new Regex(SectionRegex);
			var matches = sectionRegex.Matches(iniFileContent);
			var blocks = new List<IniBlock>();

			foreach (Match match in matches)
			{
				blocks.Add(CreateSingleSection(match));
			}

			return blocks;
		}

		private static IniBlock CreateSingleSection(Match match)
		{
			var block = new IniBlock(match.Groups["Header"].Value);
			var regex = new Regex(AttributeRegex);
			var attributes = regex.Matches(match.Groups["Attributes"].Value);
			foreach (Match attribute in attributes)
			{
				block.Set(attribute.Groups["Key"].Value, attribute.Groups["Value"].Value);
			}

			return block;
		}
	}
}