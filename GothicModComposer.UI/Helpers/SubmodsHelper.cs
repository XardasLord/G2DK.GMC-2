using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using GothicModComposer.UI.Models;
using System.Collections.ObjectModel;

namespace GothicModComposer.UI.Helpers
{
    class SubmodsHelper
    {
        public ObservableCollection<Submod> submods = new ObservableCollection<Submod>();      
        public string path = @"C:\GothicForTests\System";
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
        public void ReadObjectData(List<string> modObject,string path2)
        {
            System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
            Submod submod = new Submod();
            submod.Title = modObject[0];
            submod.Version = modObject[1];
            submod.Authors = modObject[2].Split(',');
            submod.Webpage = modObject[3];
            if (modObject[4].Contains(".rtf")) {
                rtBox.Rtf= File.ReadAllText(Path.Combine(path,modObject[4].Split(">")[1]));
                submod.Description = rtBox.Text;
;            }
            else
                submod.Description = modObject[4];
            using (Icon ico = Icon.ExtractAssociatedIcon(Path.Combine(path, modObject[5])))
            {
                submod.Icon = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }

            submod.iniFileName = Path.GetFileName(path2);
            submods.Add(submod);
        }

        public void ProcessFile(string path)
        {
            bool trigger = false;
            bool starter = false;
            string infoHeader = "[INFO]";
            string filesHeader = "[FILES]";
            List<string> modObject = new List<string>();
            foreach (string line in File.ReadLines(path))
            {
                if (line.Contains(infoHeader))
                {
                    trigger = true;
                    starter = true;
                    continue;
                }
                if (line.Contains(filesHeader))
                {
                    trigger = false;
                    break;
                }
                if (trigger)
                {
                    if (line.Contains("="))
                        modObject.Add(line.Split('=')[1].Trim());
                    else
                        continue;
                }
            }
            if (starter) {
                ReadObjectData(modObject,path);
            }
            
        }
    }
}