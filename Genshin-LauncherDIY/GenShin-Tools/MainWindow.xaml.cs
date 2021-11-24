using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace GenShin_Tools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(@"Config\Bg.png"))
            {//用bg.png
                ImageBrush b = new ImageBrush();
                Uri uri = new Uri(System.Environment.CurrentDirectory + @"\\Config\\Bg.png", UriKind.Absolute);
                b.ImageSource = new BitmapImage(uri);
                b.Stretch = Stretch.Fill;
                BGW.Background = b;
            }
            {//判断config是否存在
                if (Directory.Exists(Environment.CurrentDirectory + @"\\Config"))
                {
                    Config.setConfig.checkini();
                }
                else
                {
                    Directory.CreateDirectory("Config");
                    Config.setConfig.checkini();
                }
            }
        }
        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }
        //使用CMD方式启动原神，否则更改服务器会失效
        public void Rungenshin(params string[] command)
        {
            using (Process pc = new Process())
            {
                pc.StartInfo.FileName = "cmd.exe";
                pc.StartInfo.CreateNoWindow = true;//隐藏窗口
                pc.StartInfo.RedirectStandardError = true;//重定向错误
                pc.StartInfo.RedirectStandardInput = true;//重定向输入
                pc.StartInfo.RedirectStandardOutput = true;//重定向输出
                pc.StartInfo.UseShellExecute = false;
                pc.Start();
                int lenght = command.Length;
                foreach (string com in command)
                {
                    pc.StandardInput.WriteLine(com);
                }
                pc.StandardInput.WriteLine("exit");
                pc.StandardInput.AutoFlush = true;
                pc.Close();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //自定义分辨率启动
            if (!Config.IniGS.isAutoSize)
            {
                Config.Settings.FullS = "0";
            }
            else if (Config.IniGS.isAutoSize)
            {
                Config.Settings.FullS = "1";
            }
            if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//YuanShen.exe") == false)
            {
                this.ShowMessageAsync("错误", "游戏路径为空或游戏文件不存在\r\n请点击右上角蓝色按钮进入设置填写游戏目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                this.ShowMessageAsync("错误", Config.Settings.GamePath + "//Genshin Impact Game//YuanShen.exe", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
            else
            {
  
                Thread game = new Thread(() =>
                {
                    //次线程，防止UI假死
                    Dispatcher.Invoke(new Action(() =>
                    {
                        Rungenshin(Config.Settings.GamePath.Substring(0, 1) + ":", "cd " + Config.Settings.GamePath + "//Genshin Impact Game", "YuanShen.exe " + "-screen-fullscreen " + Config.Settings.FullS + " -screen-height " + Config.Settings.Height + " -screen-width " + Config.Settings.Width);
                    }));
                });
                game.Start();
                WindowState = WindowState.Minimized;
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
            if (Directory.Exists(Config.Settings.GamePath + "//Genshin Impact Game//ScreenShot") == false)
            {
                this.ShowMessageAsync("错误", "请先输入正确的游戏路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                return;
            }
            else
            {
                Process.Start(Config.Settings.GamePath + "//Genshin Impact Game//ScreenShot");
            }
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            setWindow set = new setWindow();
            set.Show();
        }

        private void QqG_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://jq.qq.com/?_wv=1027&k=Kxt00f0Y");
        }

        private async void About_Click(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("关于", "这是一个由WPF编写的原神启动器\r\n\r\n你可以使用本启动器做到：\r\n快速的跳转到游戏的照相保存目录\r\n自定义任意分辨率和选择是否全屏来启动游戏\r\n选择哔哩哔哩服或者选择米哈游官服进行启动\r\n\r\n编写：DawnFz，您可以跳转到Github以获取项目源代码", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" , NegativeButtonText = "GitHub"})) != MessageDialogResult.Affirmative)
            {
                Process.Start("https://github.com/DawnFz/Genshin-LauncherDIY");
            }
        }
    }
}
