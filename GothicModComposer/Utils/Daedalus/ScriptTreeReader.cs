using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Utils.Daedalus
{
    public static class ScriptTreeReader
    {
	    /// <summary>
        /// Regex pattern to get paths from .src files ignoring lines that start with a comment.
        /// Also extracting Filepath and extensions separately.
        /// </summary>
        private const string RegexPattern = @"^(?!\/\/)(?<Filepath>.*?)(?<Extension>[\*|\.][\S]*)";

        public static List<string> Parse(string filepath)
        {
            Regex regex = new Regex(RegexPattern, RegexOptions.Multiline);
            MatchCollection allMatches = regex.Matches(File.ReadAllText(filepath));
            return GetAllScriptFiles(filepath, allMatches);
        }

        public static List<string> ReadAllFiles(string filepath)
        {
            List<string> list = Parse(filepath);
            Regex multiLine = new Regex($"{GothicRegexHelper.MultiLineComment}", RegexOptions.IgnoreCase);
            Regex singleLine = new Regex("^[//]+.+");
            return list.Select(file => {
                string content = File.ReadAllText(file, EncodingHelper.GothicEncoding);
                content = multiLine.Replace(content, "");
                return singleLine.Replace(content, "");
            }).ToList();
        }

        public static List<KeyValuePair<string, string>> ReadAllFilesAndMap(string filepath)
        {
            List<string> list = Parse(filepath);
            Regex multiLine = new Regex($"{GothicRegexHelper.MultiLineComment}", RegexOptions.IgnoreCase);
            Regex singleLine = new Regex("^[//]+.+");
            return list.Select(file => {
                string content = File.ReadAllText(file, EncodingHelper.GothicEncoding);
                content = multiLine.Replace(content, "");
                return new KeyValuePair<string, string>(file, singleLine.Replace(content, ""));
            }).ToList();
        }

        private static List<string> GetAllScriptFiles(string filepath, MatchCollection collection)
        {
            List<string> toReturn = new List<string>();
            foreach (Match file in collection)
            {
                string fullPath = GetFullPath(filepath, file.Groups["Filepath"].Value, file.Groups["Extension"].Value);
                switch (file.Groups["Extension"].Value)
                {
                    case ".d":
                    case ".D":
                        toReturn.Add(fullPath);
                        break;
                    case "*.d":
                    case "*.D":
                        List<string> list = new List<string>(Directory.GetFiles(Path.GetDirectoryName(fullPath)));
                        List<string> filtered = list.Where(item => Regex.IsMatch(item, @"\.[dD]$")).ToList();
                        toReturn.AddRange(filtered);
                        break;
                    case ".src":
                        toReturn.AddRange(Parse(fullPath));
                        break;
                }
            }

            return toReturn.Distinct().ToList();
        }

        private static string GetFullPath(string srcFile, string relativePath, string extension)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(srcFile), relativePath)) + extension;
        }

    }
}