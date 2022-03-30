using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace GenShin_Launcher_Plus.Views
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = new ViewModels.HomePageViewModel(DialogCoordinator.Instance);
            LabGrid.DataContext = App.Current.NoticeOverAllBase;
        }
    }
}
