using GenShin_Launcher_Plus.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace GenShin_Launcher_Plus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            AddConfig.CheckIni();
            DataContext = new ViewModels.MainWindowViewModel(DialogCoordinator.Instance);
            MainFlipView.DataContext = ViewModels.MainBase.noab;
            HomePage.Children.Add(new Views.HomePage());
            if (!File.Exists(Path.Combine(IniControl.GamePath, "Yuanshen.exe")) && !File.Exists(Path.Combine(IniControl.GamePath, "GenshinImpact.exe")))
            {
                MainGrid.Children.Add(new Views.GuidePage());
            }
            string newver = FilesControl.MiddleText(FilesControl.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$ver$]", "[#ver#]");
            string version = Application.ResourceAssembly.GetName().Version.ToString();
            if (version != newver)
            {
                MainGrid.Children.Add(new Views.UpdatePage());
            }
        }

        private void WindowDragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SettingsPageButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage.Children.Clear();
            SettingsPage.Children.Add(new Views.SettingsPage());
            MainFlipView.SelectedIndex= 1;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUsersPage.Children.Clear();
            AddUsersPage.Children.Add(new Views.AddUsersPage());
            MainFlipView.SelectedIndex = 2;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Add(new Views.HelpsPage());
        }

        private void MainFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainFlipView.SelectedIndex == 0)
            {
                HomePage.Children.Clear();
                HomePage.Children.Add(new Views.HomePage());
            }
        }
    }
}
