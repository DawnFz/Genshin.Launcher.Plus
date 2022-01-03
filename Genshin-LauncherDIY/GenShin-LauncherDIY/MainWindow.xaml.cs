using Hardcodet.Wpf.TaskbarNotification;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            AddConfig.CheckIni();

            UtilsTools utils = new UtilsTools();
            utils.FileWriter("Res/Update.dll", @"Update.exe");
            if (!File.Exists(@"unlockfps.exe"))
                utils.FileWriter("Res/unlockfps.dll", @"unlockfps.exe");
    
            IniControl.EXEname(Path.GetFileName(Assembly.GetEntryAssembly().Location));
            if (IniControl.isMihoyo == 1)
                NowPort.Content = "当前客户端：官方服";
            else if (IniControl.isMihoyo == 2)
                NowPort.Content = "当前客户端：哔哩服";
            else if (IniControl.isMihoyo == 3)
                NowPort.Content = "当前客户端：国际服";
            else
                NowPort.Content = "当前客户端：未知";
        }
        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            DragMove();
        }

        private void RunGame_Click(object sender, RoutedEventArgs e)
        {
            if (IniControl.isUnFPS)
                Process.Start(@"unlockfps.exe");

            var argBuilder = new CommandLineBuilder();
            argBuilder.AddOption("-screen-fullscreen ", IniControl.isAutoSize ? "1" : "0");
            argBuilder.AddOption("-screen-height ", IniControl.Height);
            argBuilder.AddOption("-screen-width ", IniControl.Width);
            argBuilder.AddOption("-pop ", IniControl.isPopup ? " -popupwindow " : "");

            if (!File.Exists(Settings.gameMain))
            {
                Settings.gameMain= Path.Combine(IniControl.GamePath, "Genshin Impact Game/GenshinImpact.exe");
                if (!File.Exists(Settings.gameMain))
                {
                    this.ShowMessageAsync("错误", "游戏路径为空或游戏文件不存在\r\n请点击右侧设置按钮进入设置填写游戏目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                    return;
                }
            }
            
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = Settings.gameMain,
                Verb = "runas",
                UseShellExecute = true,
                WorkingDirectory = Path.Combine(Settings.launcherPath, "Genshin Impact Game"),
                Arguments = argBuilder.ToString()
            };
            Process.Start(info);
            
            if (IniControl.isClose == true)
            {
                TaskbarIcon = (TaskbarIcon)FindResource("Taskbar");
                Close();
            }
            else
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void CloseW_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public static TaskbarIcon TaskbarIcon;
        private void Min_Click(object sender, RoutedEventArgs e)
        {
            if (IniControl.isClose == true)
            {
                TaskbarIcon = (TaskbarIcon)FindResource("Taskbar");
                Close();
            }
            else
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void ScreenSrc_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Path.Combine(IniControl.GamePath, "Genshin Impact Game/ScreenShot")) == true)
            {
                Process.Start(Path.Combine(IniControl.GamePath, "Genshin Impact Game/ScreenShot"));
            }
            else
            {
                this.ShowMessageAsync("错误提示", "本功能为打开游戏内截图照相保存目录\r\n没有检测到照相文件或者请先输入正确的游戏路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Window set = new SetWindow();
            HwndSource winformWindow = (PresentationSource.FromDependencyObject(this) as HwndSource);
            if (winformWindow != null)
                new WindowInteropHelper(set) { Owner = winformWindow.Handle };
            set.Owner = this;
            set.ShowDialog();
        }

        private void QqG_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Settings.htmlUrl[1]);
        }

        private void SaveAcc_Click(object sender, RoutedEventArgs e)
        {
            Window save = new SaveAccIni();
            HwndSource winformWindow = (PresentationSource.FromDependencyObject(this) as HwndSource);
            if (winformWindow != null)
                new WindowInteropHelper(save) { Owner = winformWindow.Handle };
            save.ShowDialog();
        }

        private async void About_Click(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("关于", Settings.aboutthis, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定", NegativeButtonText = "GitHub" })) != MessageDialogResult.Affirmative)
                Process.Start(Settings.htmlUrl[2]);
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Settings.htmlUrl[3]);
        }

        private void UpdateEXE()
        {
            string downver = UtilsTools.MiddleText(UtilsTools.ReadHTML(Settings.htmlUrl[0], "UTF-8"), "[$gitv$]", "[#gitv#]");
            
            if (HttpFileExist($"https://cdn.jsdelivr.net/gh/DawnFz/GenShin-LauncherDIY@{ downver }/Genshin-LauncherDIY/UpdateFile/GenShinLauncher.png") == true)
            {
                Update.Visibility = Visibility.Visible;
                DownloadHttpFile($"https://cdn.jsdelivr.net/gh/DawnFz/GenShin-LauncherDIY@{ downver }/Genshin-LauncherDIY/UpdateFile/GenShinLauncher.png", @"UpdateTemp.upd");
            }
            else
            {
                this.ShowMessageAsync("错误提示", "网络更新文件资源不存在或服务器网络错误", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }

        public void DownloadHttpFile(string http_url, string save_url)
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

                Dispatcher.Invoke(new Action(async delegate ()
                {
                    if ((await this.ShowMessageAsync("提示", "下载完成，是否现在进行更新操作\r\n确定后只需等待5秒将自动启动新版本", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "取消", NegativeButtonText = "确定" })) != MessageDialogResult.Affirmative)
                    {
                        Process.Start(@"Update.exe");
                        Environment.Exit(0);
                    }
                    else
                        Update.Visibility = Visibility.Hidden;
                }));
            }, null);
        }
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

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {           
            string version = Application.ResourceAssembly.GetName().Version.ToString();
            TitleMain.Content = "原神启动器Plus  " + version;
            if (File.Exists(@"Config\Bg.png"))
            {
                ImageBrush b = new ImageBrush();
                Uri uri = new Uri(Path.Combine(Environment.CurrentDirectory, "Config/Bg.png"), UriKind.Absolute);
                b.ImageSource = new BitmapImage(uri);
                b.Stretch = Stretch.UniformToFill;
                BGW.Background = b;
            }

            if(IniControl.isMainGridHide==true)
                MainGrid.Visibility = Visibility.Hidden;

            string JsonFile = UtilsTools.ReadHTML(Settings.htmlUrl[4], "UTF-8");
            Stream s = new MemoryStream(Encoding.UTF8.GetBytes(JsonFile));
            StreamReader file = new StreamReader(s);
            JsonTextReader reader = new JsonTextReader(file);
            JObject jsonObject = (JObject)JToken.ReadFrom(reader);

            Page1.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[0]["title"].ToString();
            Page2.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[1]["title"].ToString();
            Page3.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[2]["title"].ToString();
            Page4.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[3]["title"].ToString();
            Page5.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[4]["title"].ToString();

            Page6.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[0]["end_time"].ToString().Substring(0, 10);
            Page7.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[1]["end_time"].ToString().Substring(0, 10);
            Page8.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[2]["end_time"].ToString().Substring(0, 10);
            Page9.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[3]["end_time"].ToString().Substring(0, 10);
            Page10.Content = (jsonObject["data"]["list"]).ToList()[0]["list"].ToList()[4]["end_time"].ToString().Substring(0, 10);
            file.Close();

            string newver = UtilsTools.MiddleText(UtilsTools.ReadHTML(Settings.htmlUrl[0], "UTF-8"), "[$ver$]", "[#ver#]");
            if (version != newver)
            {
                string notify = UtilsTools.MiddleText(UtilsTools.ReadHTML(Settings.htmlUrl[0], "UTF-8"), "[$notify$]", "[#notify#]");
                notify = notify.Replace("/n/", Environment.NewLine);
                string msgtl = UtilsTools.MiddleText(UtilsTools.ReadHTML(Settings.htmlUrl[0], "UTF-8"), "[$msgtl$]", "[#msgtl#]");
                if ((await this.ShowMessageAsync(msgtl, notify, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "继续使用", NegativeButtonText = "下载更新" })) != MessageDialogResult.Affirmative)
                {
                    UpdateEXE();
                }
            }
        }
    }
}
