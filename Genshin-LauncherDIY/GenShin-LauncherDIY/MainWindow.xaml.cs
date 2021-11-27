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
            InitializeComponent();
            if (File.Exists(@"Config\Bg.png"))
            {//用bg.png
                ImageBrush b = new ImageBrush();
                Uri uri = new Uri(System.Environment.CurrentDirectory + @"\\Config\\Bg.png", UriKind.Absolute);
                b.ImageSource = new BitmapImage(uri);
                b.Stretch = Stretch.Fill;
                BGW.Background = b;
            }

            {//判断版本
                String ver = Utils.UtilsTools.MiddleText(Utils.UtilsTools.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[ver]", "[/ver]");
                if ("1.0.4"!=ver)
                {
                    VerCheck();
                }

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
            if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//YuanShen.exe") == true)//启动国服
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
            else if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//GenshinImpact.exe") == true)//启动国际服
            {
                Thread game = new Thread(() =>
                {
                    //次线程，防止UI假死
                    Dispatcher.Invoke(new Action(() =>
                    {
                        Rungenshin(Config.Settings.GamePath.Substring(0, 1) + ":", "cd " + Config.Settings.GamePath + "//Genshin Impact Game", "GenshinImpact.exe " + "-screen-fullscreen " + Config.Settings.FullS + " -screen-height " + Config.Settings.Height + " -screen-width " + Config.Settings.Width);
                    }));
                });
                game.Start();
                WindowState = WindowState.Minimized;
            }
            else
            {
                this.ShowMessageAsync("错误", "游戏路径为空或游戏文件不存在\r\n请点击右上角蓝色按钮进入设置填写游戏目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
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
            if ((await this.ShowMessageAsync("关于", "这是一个由WPF编写的原神启动器\r\n\r\n你可以使用本启动器做到以下操作：\r\n1.快速跳转到游戏的照相保存文件夹\r\n2.自定义任意分辨率和是否全屏启动游戏\r\n3.选择哔哩哔哩服或者米哈游官服进行启动\r\n4.修复MihoyoSDK缺失导致的解析错误或未初始化\r\n\r\n编写：DawnFz (ねねだん)\r\n联系邮箱：admin@dawnfz.com\r\n您可以跳转到Github以获取本项目源代码", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" , NegativeButtonText = "GitHub"})) != MessageDialogResult.Affirmative)
            {
                Process.Start("https://github.com/DawnFz/Genshin-LauncherDIY");
            }
        }
        private async void VerCheck()
        {
            String notify = Utils.UtilsTools.MiddleText(Utils.UtilsTools.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[notify]", "[/notify]");
            if ((await this.ShowMessageAsync("提示", notify, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "继续使用", NegativeButtonText = "下载更新" })) != MessageDialogResult.Affirmative)
            {
                this.ShowMessageAsync("提示", "访问密码：etxd\r\n已复制到剪切板", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                Clipboard.SetText("etxd");
                Process.Start("https://pan.baidu.com/s/1-5zQoVfE7ImdXrn8OInKqg");
            }
        }
    }
}
