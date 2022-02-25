using GenShin_Launcher_Plus.Core;
using MahApps.Metro.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void ThisPageRemove(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(System.IO.Path.Combine(GamePath.Text, "Yuanshen.exe")) && !File.Exists(System.IO.Path.Combine(GamePath.Text, "GenshinImpact.exe")))
            {
                FlipView.SelectedIndex = 0;
                ErrorTitle.Text = "路径不正确或路径内不含游戏客户端，请重新选择";
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
            CommonOpenFileDialog dialog = new("请选择原神游戏本体所在文件夹");
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                GamePath.Text = dialog.FileName;
            }
        }
    }
}
