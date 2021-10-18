using System.Windows.Media.Imaging;

namespace GothicModComposer.UI.Models
{
    public class Submod
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string[] Authors { get; set; }
        public string Webpage   { get; set; }
        public string Description   { get; set; }
        public BitmapSource Icon    { get; set; }
        public bool IsSelected { get; set; }
        public string iniFileName { get; set; }
        public void SetAsSelected() => IsSelected = true;
        public void SetAsUnselected() => IsSelected = false;
    }
}
