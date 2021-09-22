using System.Collections.Generic;
using System.IO;
using System.Text;
using GothicModComposer.Models.Interfaces;

namespace GothicModComposer.Utils.IOHelpers
{
    public class GothicVdfsConfigWriter
    {
        public static string GenerateContent(IGothicVdfsConfig config, string gothicRoot, string outputPath)
        {
            var outputModFile = Path.Combine(outputPath, config.Filename);
            var builder = new StringBuilder();
            AppendSection(builder, "BEGINVDF", GenerateHeader(config.Comment, gothicRoot, outputModFile));
            AppendSection(builder, "FILES", config.Directories);
            AppendSection(builder, "INCLUDE", config.Include);
            AppendSection(builder, "EXCLUDE", config.Exclude);
            AppendSection(builder, "ENDVDF", new List<string>());

            return builder.ToString();
        }

        private static List<string> GenerateHeader(string comment, string gothicRoot, string outputVdf)
            => new()
            {
                $"Comment={comment}",
                $"BaseDir={gothicRoot}",
                $"VDFName={outputVdf}"
            };

        private static void AppendSection(StringBuilder builder, string header, List<string> lines)
        {
            builder.AppendLine($"[{header}]");
            lines.ForEach(line => builder.AppendLine(line));
        }
    }
}