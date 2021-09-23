using System.Windows.Data;
using GothicModComposer.UI.Models;
using GothicModComposer.UI.ViewModels;

namespace GothicModComposer.UI.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GmcSettings
    {
        public GmcSettings(GmcSettingsVM gmcSettingsVM)
        {
            InitializeComponent();

            DataContext = gmcSettingsVM;

            var collectionView = new ListCollectionView(gmcSettingsVM.GmcConfiguration.IniOverrides);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(IniOverride.Section)));

            OverridesIniTable.ItemsSource = collectionView;
        }
    }
}