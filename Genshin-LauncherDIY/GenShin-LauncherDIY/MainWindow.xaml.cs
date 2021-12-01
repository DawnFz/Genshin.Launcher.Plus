using GenShin_LauncherDIY.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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

namespace GenShin_LauncherDIY
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            string Version = Application.ResourceAssembly.GetName().Version.ToString();
            InitializeComponent();
            TitleMain.Content = "原神启动器Plus  " + Version;
            if (File.Exists(@"Config\Bg.png"))
            {//用bg.png
                ImageBrush b = new ImageBrush();
                Uri uri = new Uri(System.Environment.CurrentDirectory + @"\\Config\\Bg.png", UriKind.Absolute);
                b.ImageSource = new BitmapImage(uri);
                b.Stretch = Stretch.Fill;
                BGW.Background = b;
            }
            {//判断版本
                String ver = Utils.UtilsTools.MiddleText(Utils.UtilsTools.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$ver$]", "[#ver#]");
                if (Version != ver)
                {
                    VerCheck();
                }
            }
            {//判断config是否存在
                if (Directory.Exists(Environment.CurrentDirectory + @"\\Config") && Directory.Exists(Environment.CurrentDirectory + @"\\UserData"))
                {
                    Config.setConfig.checkini();
                }
                else
                {
                    Directory.CreateDirectory("Config");
                    Directory.CreateDirectory("UserData");
                    Config.setConfig.checkini();
                }
            }
            {//首次启动向导 
                //this.ShowMessageAsync("首次启动向导", Config.Settings.hajimete, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }
        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }
        //使用CMD方式启动原神，否则更改服务器会失效
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //自定义分辨率启动
            if (!Config.IniGS.isAutoSize)
                Config.Settings.FullS = "0";
            else if (Config.IniGS.isAutoSize)
                Config.Settings.FullS = "1";
            //判断无边框
            if (!Config.IniGS.isPopup)
                Config.Settings.GamePopup = "";
            else
                Config.Settings.GamePopup = " -popupwindow";
            if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//YuanShen.exe") == true)//启动国服
            {
                Thread game = new Thread(() =>
                {
                    //次线程，防止UI假死
                    Dispatcher.Invoke(new Action(() =>
                    {
                        UtilsTools.Rungenshin(Config.Settings.GamePath.Substring(0, 1) + ":", "cd " + Config.Settings.GamePath + "//Genshin Impact Game", "YuanShen.exe " + "-screen-fullscreen " + Config.Settings.FullS + " -screen-height " + Config.Settings.Height + " -screen-width " + Config.Settings.Width + Config.Settings.GamePopup);
                    }));
                });
                game.Start();
                WindowState = WindowState.Minimized;
            }
            else if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//GenshinImpact.exe") == true)//启动国际服
            {
                Thread game = new Thread(() =>
                {
                    //次线程，防止UI假死
                    Dispatcher.Invoke(new Action(() =>
                    {
                        UtilsTools.Rungenshin(Config.Settings.GamePath.Substring(0, 1) + ":", "cd " + Config.Settings.GamePath + "//Genshin Impact Game", "GenshinImpact.exe " + "-screen-fullscreen " + Config.Settings.FullS + " -screen-height " + Config.Settings.Height + " -screen-width " + Config.Settings.Width);
                    }));
                });
                game.Start();
                WindowState = WindowState.Minimized;
            }
            else
            {
                this.ShowMessageAsync("错误", "游戏路径为空或游戏文件不存在\r\n请点击右侧设置按钮进入设置填写游戏目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }

        private void CloseW_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ScreenSrc_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Config.Settings.GamePath + "//Genshin Impact Game//ScreenShot") == true)
            {
                Process.Start(Config.Settings.GamePath + "//Genshin Impact Game//ScreenShot");

            }
            else
            {
                this.ShowMessageAsync("错误", "请先输入正确的游戏路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            setWindow set = new setWindow();
            set.ShowDialog();
        }


        private void QqG_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://jq.qq.com/?_wv=1027&k=Kxt00f0Y");
        }

        private async void About_Click(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("关于", Config.Settings.aboutthis, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定", NegativeButtonText = "GitHub" })) != MessageDialogResult.Affirmative)
            {
                Process.Start("https://github.com/DawnFz/Genshin-LauncherDIY");
            }
        }
        private async void VerCheck()
        {
            String notify = Utils.UtilsTools.MiddleText(Utils.UtilsTools.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$notify$]", "[#notify#]");
            notify = notify.Replace("/n/", Environment.NewLine);
            String msgtl = Utils.UtilsTools.MiddleText(Utils.UtilsTools.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$msgtl$]", "[#msgtl#]");
            if ((await this.ShowMessageAsync(msgtl, notify, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "继续使用", NegativeButtonText = "下载更新" })) != MessageDialogResult.Affirmative)
            {
                this.ShowMessageAsync("提示", "访问密码：etxd\r\n已复制到剪切板", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                Clipboard.SetText("etxd");
                Process.Start("https://pan.baidu.com/s/1-5zQoVfE7ImdXrn8OInKqg");
            }
        }

        private async void SaveAcc_Click(object sender, RoutedEventArgs e)
        {
            SaveAccIni save = new SaveAccIni();
            save.ShowDialog();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
