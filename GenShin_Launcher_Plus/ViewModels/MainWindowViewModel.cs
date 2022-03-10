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
using GenShin_Launcher_Plus.Models;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private IDialogCoordinator dialogCoordinator;
        public MainWindowViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            languages = MainBase.lang;
            Mainloading();
            OpenImagesDirectoryCommand = new RelayCommand(OpenImagesDirectory);
            OpenAboutCommand = new RelayCommand(OpenAbout);
            OpenQQGroupUrlCommand = new RelayCommand(OpenQQGroupUrl);
            ExitProgramCommand = new RelayCommand(ExitProgram);
            MainMinimizedCommand = new RelayCommand(MainMinimized);
            Title = $"{languages.MainTitle} {Application.ResourceAssembly.GetName().Version}";
            IniControl.EXEname(Path.GetFileName(Environment.ProcessPath));
        }


        public LanguagesModel languages { get; set; }

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
                    string bgurl = languages.MainBackground;
                    if (bgurl != ""&&bgurl!=null)
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
                await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.ScreenPathErr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
            }
        }


        public ICommand OpenAboutCommand { get; set; }
        private async void OpenAbout()
        {
            if ((await dialogCoordinator.ShowMessageAsync(this, languages.AboutTitle, languages.AboutStr, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine, NegativeButtonText = "GitHub" })) != MessageDialogResult.Affirmative)
            {
                ProcessStartInfo info = new()
                {
                    FileName = "https://github.com/DawnFz/Genshin.Launcher.Plus",
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
    }
}
