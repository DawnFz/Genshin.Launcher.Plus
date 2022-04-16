using GenShin_Launcher_Plus.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace GenShin_Launcher_Plus.Views
{
    /// <summary>
    /// DemoSetting.xaml 的交互逻辑
    /// </summary>
    public partial class DemoSetting : UserControl
    {
        public DemoSetting()
        {
            DataContext = new SettingsPageViewModel(DialogCoordinator.Instance);
            InitializeComponent();
        }
    }
}
