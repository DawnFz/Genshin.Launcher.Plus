using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
            DataContext = new MainWindowViewModel(DialogCoordinator.Instance,this);
            MainFlipView.DataContext = App.Current.NoticeOverAllBase;
            HomePage.Children.Add(new Views.HomePage());
        }

        private void WindowDragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SettingsPageButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage.Children.Clear();
            SettingsPage.Children.Add(new Views.SettingsPage());
            MainFlipView.SelectedIndex = 1;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUsersPage.Children.Clear();
            AddUsersPage.Children.Add(new Views.UsersPage());
            MainFlipView.SelectedIndex = 2;
        }

        private void LangBtn_Click(object sender, RoutedEventArgs e)
        {
            SwitchLanguages.Children.Clear();
            SwitchLanguages.Children.Add(new Views.LanguagesPage());
            MainFlipView.SelectedIndex = 3;
        }

        private void Help_Copy_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo info = new()
            {
                FileName = "https://www.dawnfz.com/document/",
                UseShellExecute = true,
            };
            Process.Start(info);
        }
    }
}
