using GenShin_LauncherDIY.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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
            RunLoad();
            //避免更新器需要置换，每次重新操作
            {
                var updUri = "pack://application:,,,/Res/Update.dll";
                var uri = new Uri(updUri, UriKind.RelativeOrAbsolute);
                var stream = Application.GetResourceStream(uri).Stream;
                Utils.UtilsTools.StreamToFile(stream, @"Update.exe");
            }
            //每次启动写入当前文件名以便更新操作
            Config.IniGS.EXEname(System.IO.Path.GetFileName(Assembly.GetEntryAssembly().Location));
        }



        //窗口随意拖动
        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }
        //启动游戏按钮事件-使用CMD方式启动原神，否则更改服务器会失效
        private void RunGame_Click(object sender, RoutedEventArgs e)
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
            //判断解锁帧率上限
            if (Config.IniGS.isUnFPS == true)
            {
                if (File.Exists(@"unlockfps.exe"))
                {
                    File.Delete(@"unlockfps.exe");
                    var fpsUri = "pack://application:,,,/Res/unlockfps.dll";
                    var uri = new Uri(fpsUri, UriKind.RelativeOrAbsolute);
                    var stream = Application.GetResourceStream(uri).Stream;
                    Utils.UtilsTools.StreamToFile(stream, @"unlockfps.exe");
                }
                else
                {
                    var fpsUri = "pack://application:,,,/Res/unlockfps.dll";
                    var uri = new Uri(fpsUri, UriKind.RelativeOrAbsolute);
                    var stream = Application.GetResourceStream(uri).Stream;
                    Utils.UtilsTools.StreamToFile(stream, @"unlockfps.exe");
                }

                if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//YuanShen.exe") == true)
                {
                    Process.Start(@"unlockfps.exe");
                    Thread game = new Thread(() =>
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            UtilsTools.Rungenshin(Config.Settings.GamePath.Substring(0, 1) + ":", "cd " + Config.Settings.GamePath + "//Genshin Impact Game", "YuanShen.exe " + "-screen-fullscreen " + Config.Settings.FullS + " -screen-height " + Config.Settings.Height + " -screen-width " + Config.Settings.Width + Config.Settings.GamePopup);
                        }));
                    });
                    game.Start();
                    WindowState = WindowState.Minimized;
                }
                else if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//GenshinImpact.exe") == true)
                {
                    Process.Start(@"unlockfps.exe");
                    Thread game = new Thread(() =>
                    {
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
            else
            {
                if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//YuanShen.exe") == true)
                {
                    Thread game = new Thread(() =>
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            UtilsTools.Rungenshin(Config.Settings.GamePath.Substring(0, 1) + ":", "cd " + Config.Settings.GamePath + "//Genshin Impact Game", "YuanShen.exe " + "-screen-fullscreen " + Config.Settings.FullS + " -screen-height " + Config.Settings.Height + " -screen-width " + Config.Settings.Width + Config.Settings.GamePopup);
                        }));
                    });
                    game.Start();
                    WindowState = WindowState.Minimized;
                }
                else if (File.Exists(Config.Settings.GamePath + "//Genshin Impact Game//GenshinImpact.exe") == true)
                {
                    Thread game = new Thread(() =>
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            UtilsTools.Rungenshin(Config.Settings.GamePath.Substring(0, 1) + ":", "cd " + Config.Settings.GamePath + "//Genshin Impact Game", "GenshinImpact.exe " + "-screen-fullscreen " + Config.Settings.FullS + " -screen-height " + Config.Settings.Height + " -screen-width " + Config.Settings.Width + Config.Settings.GamePopup);
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
        }
        //关闭按钮事件
        private void CloseW_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //最小化按钮事件
        private void Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        //截图目录按钮事件
        private void ScreenSrc_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Config.Settings.GamePath + "//Genshin Impact Game//ScreenShot") == true)
            {
                Process.Start(Config.Settings.GamePath + "//Genshin Impact Game//ScreenShot");

            }
            else
            {
                this.ShowMessageAsync("错误提示", "本功能为打开游戏内截图照相保存目录\r\n没有检测到照相文件或者请先输入正确的游戏路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }
        //设置按钮事件
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            setWindow set = new setWindow();
            set.ShowDialog();
        }
        //QQ群按钮事件
        private void QqG_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://jq.qq.com/?_wv=1027&k=Kxt00f0Y");
        }
        //保存账号按钮事件
        private void SaveAcc_Click(object sender, RoutedEventArgs e)
        {
            SaveAccIni save = new SaveAccIni();
            save.ShowDialog();
        }
        //关于按钮事件
        private async void About_Click(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("关于", Config.Settings.aboutthis, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定", NegativeButtonText = "GitHub" })) != MessageDialogResult.Affirmative)
            {
                Process.Start("https://github.com/DawnFz/Genshin-LauncherDIY");
            }
        }
        //帮助按钮事件
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.bilibili.com/video/BV1hr4y1Q7Qm");
        }
        //启动时事件
        private async void RunLoad()
        {
            string Version = Application.ResourceAssembly.GetName().Version.ToString();
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
                    String notify = Utils.UtilsTools.MiddleText(Utils.UtilsTools.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$notify$]", "[#notify#]");
                    notify = notify.Replace("/n/", Environment.NewLine);
                    String msgtl = Utils.UtilsTools.MiddleText(Utils.UtilsTools.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$msgtl$]", "[#msgtl#]");
                    if ((await this.ShowMessageAsync(msgtl, notify, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "继续使用", NegativeButtonText = "下载更新" })) != MessageDialogResult.Affirmative)
                    {
                        UpdateEXE();
                    }
                }
            }
            {//判断config是否存在
                if (Directory.Exists(Environment.CurrentDirectory + @"\\Config") && Directory.Exists(Environment.CurrentDirectory + @"\\UserData"))
                    Config.setConfig.checkini();
                else
                {
                    Directory.CreateDirectory("Config");
                    Directory.CreateDirectory("UserData");
                    Config.setConfig.checkini();
                }
            }
        }
        //启动更新
        private void UpdateEXE()
        {
            if (HttpFileExist("https://cdn.jsdelivr.net/gh/DawnFz/GenShin-LauncherDIY/Genshin-LauncherDIY/UpdateFile/GenShinLauncher.png") == true)
            {
                Update.Visibility = Visibility.Visible;
                DownloadHttpFile("https://cdn.jsdelivr.net/gh/DawnFz/GenShin-LauncherDIY/Genshin-LauncherDIY/UpdateFile/GenShinLauncher.png", @"UpdateTemp.upd");
            }
            else
            {
                this.ShowMessageAsync("错误提示", "网络更新文件资源不存在或服务器网络错误", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }
        //版本更新-下载文件
        public void DownloadHttpFile(String http_url, String save_url)
        {
            WebResponse response = null;
            WebRequest request = WebRequest.Create(http_url);
            response = request.GetResponse();
            if (response == null) return;
            pbDown.Maximum = response.ContentLength;
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Stream netStream = response.GetResponseStream();
                Stream fileStream = new FileStream(save_url, FileMode.Create);
                byte[] read = new byte[1024];
                long progressBarValue = 0;
                int realReadLen = netStream.Read(read, 0, read.Length);
                while (realReadLen > 0)
                {
                    fileStream.Write(read, 0, realReadLen);
                    progressBarValue += realReadLen;
                    pbDown.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);
                    realReadLen = netStream.Read(read, 0, read.Length);
                }
                netStream.Close();
                fileStream.Close();

                this.Dispatcher.Invoke(new Action(async delegate ()
                {
                    if ((await this.ShowMessageAsync("提示", "下载完成，是否现在进行更新操作", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "取消", NegativeButtonText = "确定" })) != MessageDialogResult.Affirmative)
                    {
                        Process.Start(@"Update.exe");
                        this.Close();
                    }
                }));
            }, null);
        }
        //判断网络资源是否存在
        private bool HttpFileExist(string http_file_url)
        {
            WebResponse response = null;
            bool result = false;
            try
            {
                response = WebRequest.Create(http_file_url).GetResponse();
                result = response == null ? false : true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }
        public delegate void ProgressBarSetter(double value);
        public void SetProgressBar(double value)
        {
            pbDown.Value = value;
            label1.Content = "下载进度:" + Convert.ToInt32((value / pbDown.Maximum) * 100) + "%";
        }
    }
}
