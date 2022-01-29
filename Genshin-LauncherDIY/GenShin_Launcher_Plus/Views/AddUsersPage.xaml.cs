using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
