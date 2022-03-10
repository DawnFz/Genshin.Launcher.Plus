using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace GenShin_Launcher_Plus.Views
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : UserControl
    {

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = new ViewModels.SettingsPageViewModel(DialogCoordinator.Instance);
        }
    }
}
