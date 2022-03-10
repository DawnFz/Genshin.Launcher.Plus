using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows;
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
            DataContext = new GuidePageViewModel();
        }

        private void ThisPageRemove(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(System.IO.Path.Combine(GamePath.Text, "Yuanshen.exe")) && !File.Exists(System.IO.Path.Combine(GamePath.Text, "GenshinImpact.exe")))
            {
                FlipView.SelectedIndex = 0;
                ErrorTitle.Text = MainBase.lang.PathErrorMessageStr;
            }
            else
            {
                IniControl.GamePath = GamePath.Text;
                MainWindow mainWindow = new();
                Window window = Window.GetWindow(this);
                window.Close();
                mainWindow.Show();
            }
        }

        private void Dirchoose_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new(MainBase.lang.GameDirMsg);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                GamePath.Text = dialog.FileName;
            }
        }
    }
}
