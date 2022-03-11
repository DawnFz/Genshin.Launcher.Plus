using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
            LoadProgramCore lpc = new ();
            lpc.LoadLanguageCore(MainBase.IniModel.ReadLang);
            lpc.LoadUpdateCore();
            InitializeComponent();
            DataContext = new MainWindowViewModel(DialogCoordinator.Instance);
            MainFlipView.DataContext = MainBase.noab;
            HomePage.Children.Add(new Views.HomePage());
            if (!File.Exists(Path.Combine(MainBase.IniModel.GamePath==null?"": MainBase.IniModel.GamePath, "Yuanshen.exe")) && !File.Exists(Path.Combine(MainBase.IniModel.GamePath == null ? "" : MainBase.IniModel.GamePath, "GenshinImpact.exe")))
            {
                MainGrid.Children.Add(new Views.GuidePage());
            }
            
            Task checkupdate = new(() =>
            {
                string newver = MainBase.update.Version;
                string version = Application.ResourceAssembly.GetName().Version.ToString();
                if (version != newver)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MainGrid.Children.Add(new Views.UpdatePage());
                    });           
                }
            });
            checkupdate.Start();    
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
            AddUsersPage.Children.Add(new Views.AddUsersPage());
            MainFlipView.SelectedIndex = 2;
        }

        private void LangBtn_Click(object sender, RoutedEventArgs e)
        {
            SwitchLanguages.Children.Clear();
            SwitchLanguages.Children.Add(new Views.SwitchLanguagesPage());
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
