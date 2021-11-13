using System.Collections.Generic;
using System.Text;
using GothicModComposer.Core.Models.IniFiles;

namespace GothicModComposer.Core.Utils.IOHelpers
{
    public static class GothicIniWriter
    {
        public static string GenerateContent(List<IniBlock> iniBlocks)
        {
            var sb = new StringBuilder();
            iniBlocks.ForEach(item => AppendBlock(sb, item));
            return sb.ToString();
        }

        private static void AppendBlock(StringBuilder builder, IniBlock iniBlock)
        {
            builder.AppendLine($"[{iniBlock.Header}]");
            iniBlock.Properties.ForEach(item => builder.AppendLine($"{item.Key}={item.Value}"));
        }
    }
}