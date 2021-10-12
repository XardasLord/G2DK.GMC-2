using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace GothicModComposer.UI.Helpers
{
    class SubmodsHelper
    {
        public List<Submod> submods = new List<Submod>();
        string path = @"C:\GothicForTests\System";
        string textHeader = "[INFO]";
        public void Main()
        {
            if (Directory.Exists(path))
            {
                ProcessDirectory(path);
            }
            else
            {
                Debug.WriteLine("{0} is not a valid directory.", path);
            }
        }
        public void ProcessDirectory(string targetDirectory)
        {
            var fileEntries = Directory.GetFiles(targetDirectory, "*.ini");

            foreach (string fileName in fileEntries)
                ProcessFile(fileName);
        }
        public void ReadObjectData(List<string> modObject)
        {
            Submod submod = new Submod();
            submod.Title = modObject[0];
            submod.Version = modObject[1];
            submod.Authors = modObject[2].Split(',');
            submod.Webpage = modObject[3];
            submod.Description = modObject[4];
            submod.Icon = modObject[5];

            submods.Add(submod);
        }

        public void printListedObjects()
        {
            foreach (var submod in submods)
            {
                submod.Talk();
            }
        }
        public void ProcessFile(string path)
        {
            bool trigger = false;
            bool starter = false;
            List<string> modObject = new List<string>();
            foreach (string line in File.ReadLines(path))
            {
                if (line.Contains(textHeader))
                {
                    trigger = true;
                    starter = true;
                    continue;
                }
                if (line.Contains("[FILES]"))
                {
                    trigger = false;
                    break;
                }
                if (trigger)
                {
                    //Debug.WriteLine(line);
                    if (line.Contains("="))
                        modObject.Add(line.Split('=')[1].Trim());
                    else
                        continue;
                }
            }
            if (starter) {
                ReadObjectData(modObject);
            }
            
        }
    }
    public class Submod
    {
        public string Title;
        public string Version;
        public string[] Authors;
        public string Webpage;
        public string Description;
        public string Icon;

        public void Talk()
        {
            Debug.WriteLine(Title + Version + Authors + Webpage + Description + Icon);
        }
    }
}