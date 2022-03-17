using GenShin_Launcher_Plus.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace GenShin_Launcher_Plus.Views
{
    /// <summary>
    /// GuidePage.xaml 的交互逻辑
    /// </summary>
    public partial class GuidePage : UserControl
    {
        public GuidePage()
        {
            InitializeComponent();
            DataContext = new GuidePageViewModel(DialogCoordinator.Instance);
        }
    }
}
