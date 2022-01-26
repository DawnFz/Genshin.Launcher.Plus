using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GenShin_Launcher_Plus.Command;
using GenShin_Launcher_Plus.Core;
using MahApps.Metro.Controls.Dialogs;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class MainWindowViewModel : NotificationObject
    {
        private IDialogCoordinator dialogCoordinator;
        public MainWindowViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            MainWindowLoaded();
            OpenImagesDirectoryCommand = new DelegateCommand { ExecuteAction = new Action<object>(OpenImagesDirectory) };
            OpenAboutCommand = new DelegateCommand { ExecuteAction = new Action<object>(OpenAbout) };
            OpenQQGroupUrlCommand = new DelegateCommand { ExecuteAction = new Action<object>(OpenQQGroupUrl) };
            ExitProgramCommand = new DelegateCommand { ExecuteAction = new Action<object>(ExitProgram) };
            MainMinimizedCommand = new DelegateCommand { ExecuteAction = new Action<object>(MainMinimized) };
            Title = $"原神启动器Plus {Application.ResourceAssembly.GetName().Version}";
            IniControl.EXEname(Path.GetFileName(Environment.ProcessPath));
        }

        public string Title { get; set; }

        private ImageBrush _Background;
        public ImageBrush Background
        {
            get { return _Background; }
            set
            {
                _Background = value;
                OnPropChanged("Background");
            }
        }



        public DelegateCommand OpenImagesDirectoryCommand { get; set; }
        private async void OpenImagesDirectory(object parameter)
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


        public DelegateCommand OpenAboutCommand { get; set; }
        private async void OpenAbout(object parameter)
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

        public DelegateCommand OpenQQGroupUrlCommand { get; set; }
        private void OpenQQGroupUrl(object parameter)
        {
            ProcessStartInfo info = new()
            {
                FileName = "https://jq.qq.com/?_wv=1027&k=Kxt00f0Y",
                UseShellExecute = true,
            };
            Process.Start(info);
        }

        public DelegateCommand ExitProgramCommand { get; set; }
        private void ExitProgram(object parameter)
        {
            Environment.Exit(0);
        }


        public DelegateCommand MainMinimizedCommand { get; set; }
        private void MainMinimized(object parameter)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
        }


        private void MainWindowLoaded()
        {
            Background = new();
            if (File.Exists(@"Config\Bg.png"))
            {
                Uri uri = new Uri(Path.Combine(Environment.CurrentDirectory, "Config/Bg.png"), UriKind.Absolute);
                Background.ImageSource = new BitmapImage(uri);
                Background.Stretch = Stretch.UniformToFill;
            }
            else if (IniControl.isWebBg == true)
            {
                Uri uri = new Uri(FilesControl.MiddleText(FilesControl.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$bg$]", "[#bg#]"), UriKind.Absolute);
                Background.ImageSource = new BitmapImage(uri);
                Background.Stretch = Stretch.UniformToFill;
            }
            else
            {
                Uri uri = new Uri("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                Background.ImageSource = new BitmapImage(uri);
                Background.Stretch = Stretch.UniformToFill;
            }

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
