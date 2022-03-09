using GenShin_Launcher_Plus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenShin_Launcher_Plus.Views
{
    /// <summary>
    /// SwitchLanguagesPage.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchLanguagesPage : UserControl
    {
        public SwitchLanguagesPage()
        {
            InitializeComponent();
            DataContext = new SwitchLanguagesPageViewModel();
        }

        private void RemoveThisPage(object sender, RoutedEventArgs e)
        {
            MainBase.noab.MainPagesIndex = 0;
        }
    }
}
