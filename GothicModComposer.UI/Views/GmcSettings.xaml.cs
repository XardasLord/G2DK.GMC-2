using GothicModComposer.UI.ViewModels;

namespace GothicModComposer.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GmcSettings
    {
        public GmcSettings(GmcSettingsVM gmcSettingsVM)
        {
            InitializeComponent();

            DataContext = gmcSettingsVM;
        }
    }
}
