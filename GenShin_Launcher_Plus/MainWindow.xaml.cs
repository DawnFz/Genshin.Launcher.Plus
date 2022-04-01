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
            App.Current.LoadProgramCore.LoadLanguageCore();
            DataContext = new MainWindowViewModel(DialogCoordinator.Instance);
            MainFlipView.DataContext = App.Current.NoticeOverAllBase;
            HomePage.Children.Add(new Views.HomePage());
            if (!File.Exists(Path.Combine(App.Current.IniModel.GamePath ?? "", "Yuanshen.exe")) &&
                !File.Exists(Path.Combine(App.Current.IniModel.GamePath ?? "", "GenshinImpact.exe")))
            {
                MainGrid.Children.Add(new Views.GuidePage());
            }

            string newver = App.Current.UpdateObject.Version;
            string version = Application.ResourceAssembly.GetName().Version.ToString();
            if (version != newver && !App.Current.IsLoading)
            {
                MainGrid.Children.Add(new Views.UpdatePage());
                App.Current.IsLoading = true;
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
