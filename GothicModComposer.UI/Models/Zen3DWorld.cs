namespace GothicModComposer.UI.Models
{
    public class Zen3DWorld
    {
        public Zen3DWorld(string fullPath, string name)
        {
            var indexofWorldSubPath = fullPath.IndexOf("Worlds\\");

            FullPath = fullPath;
            Path = fullPath.Remove(0, indexofWorldSubPath + 7);
            Name = name;
            PathWithoutExtension = Path.Replace(Name, System.IO.Path.GetFileNameWithoutExtension(Name));
        }

        public string FullPath { get; }
        public string Path { get; }
        public string Name { get; }
        public string PathWithoutExtension { get; }

        public bool IsSelected { get; set; }

        public void SetAsSelected() => IsSelected = true;
        public void SetAsUnselected() => IsSelected = false;
    }
}