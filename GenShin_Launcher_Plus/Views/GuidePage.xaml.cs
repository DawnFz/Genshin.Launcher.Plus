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
            //提醒：改一下这里的逻辑
            if (!File.Exists(Path.Combine(GamePath.Text, "Yuanshen.exe")) && !File.Exists(Path.Combine(GamePath.Text, "GenshinImpact.exe")))
            {
                FlipView.SelectedIndex = 0;
                ErrorTitle.Text = MainBase.lang.PathErrorMessageStr;
            }
            else
            {
                MainBase.IniModel.GamePath = GamePath.Text;
                MainBase.IniModel = new();
                MainWindow mainWindow = new();
                mainWindow.Show();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = mainWindow;
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
