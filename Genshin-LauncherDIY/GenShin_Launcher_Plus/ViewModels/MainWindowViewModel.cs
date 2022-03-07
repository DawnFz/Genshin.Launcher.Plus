using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GenShin_Launcher_Plus.Core;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Security.Cryptography;
using System.Text;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private IDialogCoordinator dialogCoordinator;
        public MainWindowViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            Mainloading();
            OpenImagesDirectoryCommand = new RelayCommand(OpenImagesDirectory);
            OpenAboutCommand = new RelayCommand(OpenAbout);
            OpenQQGroupUrlCommand = new RelayCommand(OpenQQGroupUrl);
            ExitProgramCommand = new RelayCommand(ExitProgram);
            MainMinimizedCommand = new RelayCommand(MainMinimized);
            Title = $"原神启动器Plus {Application.ResourceAssembly.GetName().Version}";
            IniControl.EXEname(Path.GetFileName(Environment.ProcessPath));
        }

        public string Title { get; set; }

        private ImageBrush _Background;
        public ImageBrush Background
        {
            get => _Background;
            set => SetProperty(ref _Background, value);
        }

        private async void Mainloading()
        {
            //
            Background = new();
            if (IniControl.UserXunkongWallpaper)
            {
                Background.Stretch = Stretch.UniformToFill;
                var uri = new Uri("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                try
                {
                    var file = Path.Combine(AppContext.BaseDirectory, "Config/XunkongWallpaper.jpg");
                    if (File.Exists(file))
                    {
                        uri = new Uri(file);
                        using var fs = File.OpenRead(file);
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = fs;
                        bitmap.EndInit();
                        Background.ImageSource = bitmap;
                    }
                    else
                    {
                        Background.ImageSource = new BitmapImage(uri);
                    }
                    const string url = "https://api.xunkong.cc/v0.1/genshin/wallpaper/redirect/recommend";
                    var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All });
                    client.DefaultRequestHeaders.Add("User-Agent", $"GenShinLauncher/{Application.ResourceAssembly.GetName().Version}");
                    try
                    {
                        var UserName = Environment.UserName;
                        var MachineGuid = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\", "MachineGuid", UserName);
                        var UserBytes = Encoding.UTF8.GetBytes(UserName + MachineGuid);
                        var hash = MD5.HashData(UserBytes);
                        var deviceId = Convert.ToHexString(hash);
                        client.DefaultRequestHeaders.Add("X-Device-Id", deviceId);
                    }
                    catch { }
                    var bytes = await client.GetByteArrayAsync(url);
                    var ms = new MemoryStream(bytes);
                    var newBitmap = new BitmapImage();
                    newBitmap.BeginInit();
                    newBitmap.CacheOption = BitmapCacheOption.OnLoad;
                    newBitmap.StreamSource = ms;
                    newBitmap.EndInit();
                    Background.ImageSource = newBitmap;
                    await File.WriteAllBytesAsync(file, bytes);
                }
                catch
                {
                    Background.ImageSource = new BitmapImage(uri);
                }
            }
            else
            {
                Uri uri;
                if (File.Exists(@"Config\Bg.png"))
                {
                    uri = new(Path.Combine(Environment.CurrentDirectory, "Config/Bg.png"), UriKind.Absolute);
                }
                else if (File.Exists(@"Config\Bg.jpg"))
                {
                    uri = new(Path.Combine(Environment.CurrentDirectory, "Config/Bg.jpg"), UriKind.Absolute);
                }
                else if (IniControl.isWebBg == true)
                {
                    uri = new("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                }
                else
                {
                    string bgurl = FilesControl.MiddleText(FilesControl.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$bg$]", "[#bg#]");
                    if (bgurl != "读取错误，请检查网络后再试！")
                    {
                        uri = new(bgurl, UriKind.Absolute);
                    }
                    else
                    {
                        uri = new("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                    }
                }
                Background.ImageSource = new BitmapImage(uri);
                Background.Stretch = Stretch.UniformToFill;
            }

            //
            FilesControl utils = new();
            try
            {
                utils.FileWriter("StaticRes/unlockfps.dll", @"unlockfps.exe");
                utils.FileWriter("StaticRes/Update.dll", @"Update.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand OpenImagesDirectoryCommand { get; set; }
        private async void OpenImagesDirectory()
        {
            if (Directory.Exists(Path.Combine(IniControl.GamePath, "ScreenShot")))
            {
                ProcessStartInfo info = new()
                {
                    FileName = Path.Combine(IniControl.GamePath, "ScreenShot"),
                    UseShellExecute = true,
                };
                Process.Start(info);
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "错误提示", "本功能为打开游戏内截图照相保存目录\r\n没有检测到照相文件或者请先输入正确的游戏路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }


        public ICommand OpenAboutCommand { get; set; }
        private async void OpenAbout()
        {
            if ((await dialogCoordinator.ShowMessageAsync(this, "关于", aboutthis, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定", NegativeButtonText = "GitHub" })) != MessageDialogResult.Affirmative)
            {
                ProcessStartInfo info = new()
                {
                    FileName = "https://github.com/DawnFz/Genshin-LauncherDIY",
                    UseShellExecute = true,
                };
                Process.Start(info);
            }
        }

        public ICommand OpenQQGroupUrlCommand { get; set; }
        private void OpenQQGroupUrl()
        {
            ProcessStartInfo info = new()
            {
                FileName = "https://jq.qq.com/?_wv=1027&k=Kxt00f0Y",
                UseShellExecute = true,
            };
            Process.Start(info);
        }

        public ICommand ExitProgramCommand { get; set; }
        private void ExitProgram()
        {
            Environment.Exit(0);
        }


        public ICommand MainMinimizedCommand { get; set; }
        private void MainMinimized()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
        }

        private string aboutthis =
            (
            "注意，启动器涉及到注册表修改和文件替换，部分杀毒软\r\n" +
            "件可能会报毒，为了客户端数据完整建议关闭杀软后再运行\r\n" +
            "本程序完全开源，并不会将用户数据公布到网络，\r\n" +
            "本启动器需要联网部分的代码仅为版本检测和公告获取\r\n\r\n\r\n" +
            "编写：DawnFz (ねねだん)\r\n" +
            "联系邮箱：admin@dawnfz.com\r\n" +
            "技术支持：Lightczx（Github）【Snap.Genshin作者】\r\n" +
            "您可以跳转到Github以获取本项目源代码\r\n\r\n\r\n" +
            "————————本程序用到的代码及参考————————\r\n" +
            "[Snap.Genshin]\r\n" +
            "项目地址：https://github.com/DGP-Studio/Snap.Genshin \r\n" +
            "————————————————————————————\r\n" +
            "[genshin-fps-unlock]\r\n" +
            "项目地址：https://gitee.com/Euphony_Facetious/genshin-fps-unlock \r\n");
    }
}
