using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.Service.IService;
using GenShin_Launcher_Plus.ViewModels;

namespace GenShin_Launcher_Plus.Service
{
    public class MainService : IMainWindowService
    {

        public MainService(MainWindow main, MainWindowViewModel vm)
        {
            CheckConfig(main);
            MainBackgroundLoad(vm);
        }

        public async void MainBackgroundLoad(MainWindowViewModel vm)
        {
            vm.Background = new ImageBrush();
            if (App.Current.IniModel.UseXunkongWallpaper)
            {
                vm.Background.Stretch = Stretch.UniformToFill;
                var uri = new Uri("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                try
                {
                    var file = Path.Combine(AppContext.BaseDirectory, "Config/XunkongWallpaper.webp");
                    if (File.Exists(file))
                    {
                        uri = new Uri(file);
                        using var fs = File.OpenRead(file);
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = fs;
                        bitmap.EndInit();
                        vm.Background.ImageSource = bitmap;
                    }
                    else
                    {
                        vm.Background.ImageSource = new BitmapImage(uri);
                    }
                    const string url = "https://api.xunkong.cc/v0.1/wallpaper/recommend/redirect";
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
                    vm.Background.ImageSource = newBitmap;
                    await File.WriteAllBytesAsync(file, bytes);
                }
                catch
                {
                    vm.Background.ImageSource = new BitmapImage(uri);
                }
            }
            else
            {
                Uri uri = new("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                if (File.Exists(@"Config/Bg.png"))
                {
                    uri = new(Path.Combine(Environment.CurrentDirectory, "Config/Bg.png"), UriKind.Absolute);
                }
                else if (File.Exists(@"Config/Bg.jpg"))
                {
                    uri = new(Path.Combine(Environment.CurrentDirectory, "Config/Bg.jpg"), UriKind.Absolute);
                }
                else if (App.Current.IniModel.isWebBg)
                {
                    uri = new("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                }
                else
                {
                    /*新的背景解决方案
                     * 
                     * 读取json获得背景列表并判断日期进行轮播
                     * 将在下个版本加入
                     * 
                    string bgurl = App.Current.UpdateObject.BgUrl;
                    if (bgurl != "" && bgurl != null)
                    {
                        uri = new(bgurl, UriKind.Absolute);
                    }*/
                    string bgurl = await HtmlHelper.GetInfoFromHtmlAsync("bg");
                    if (bgurl != "" && bgurl != null)
                    {
                        uri = new(bgurl, UriKind.Absolute);
                    }                    
                }
                vm.Background.ImageSource = new BitmapImage(uri);
                vm.Background.Stretch = Stretch.UniformToFill;
            }
        }

        public void CheckConfig(MainWindow main)
        {
            if (!Directory.Exists(@"UserData"))
            {
                Directory.CreateDirectory("UserData");
            }
            if (!File.Exists(Path.Combine(App.Current.IniModel.GamePath ?? "", "Yuanshen.exe")) &&
                !File.Exists(Path.Combine(App.Current.IniModel.GamePath ?? "", "GenshinImpact.exe")))
            {
                main.MainGrid.Children.Add(new Views.GuidePage());
            }
        }
    }
}
