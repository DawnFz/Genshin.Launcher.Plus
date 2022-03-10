using System.Windows;
using System.Windows.Controls;
using GenShin_Launcher_Plus.ViewModels;

namespace GenShin_Launcher_Plus.Views
{
    /// <summary>
    /// AddUsersPage.xaml 的交互逻辑
    /// </summary>
    public partial class AddUsersPage : UserControl
    {
        public AddUsersPage()
        {
            InitializeComponent();
            DataContext = new AddUsersPageViewModel();
        }

        private void RemoveThisPage(object sender, RoutedEventArgs e)
        {
            MainBase.noab.MainPagesIndex = 0;
        }
    }
}
