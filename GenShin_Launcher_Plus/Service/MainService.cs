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
using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.Service.IService;
using GenShin_Launcher_Plus.ViewModels;
using Newtonsoft.Json;

namespace GenShin_Launcher_Plus.Service
{
    public class MainService : IMainWindowService
    {

        public MainService(MainWindow main, MainWindowViewModel vm)
        {
            CheckConfig(main);
            MainBackgroundLoad(vm);
        }

        /// <summary>
        /// 异步实现Main中的背景调用
        /// </summary>
        /// <param name="vm"></param>
        public async void MainBackgroundLoad(MainWindowViewModel vm)
        {
            /*
             * 利用 Lolicon Api 搜索Pixiv中的原神板块随机获取图片并返回Json信息
             * 使用 Pixiv.re 反代从网络获得该图片并应用到背景
             * 自己会编译的可以自己使用这段注释的代码，Api里的键r18对应的值0为假1为真，懂得都懂
             * 
            string json = await HtmlHelper.ReadHTMLAsTextAsync("https://api.lolicon.app/setu/v2?r18=0&proxy=null&tag=%E5%8E%9F%E7%A5%9E");
            ImageModel imageModel = JsonConvert.DeserializeObject<ImageModel>(json);

            vm.Background = new ImageBrush();
            if (App.Current.DataModel.UseXunkongWallpaper)
            {
                vm.Background.Stretch = Stretch.UniformToFill;
                var uri = new Uri("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                vm.Background.ImageSource = new BitmapImage(uri);

                string urlEnd = $"{imageModel.data[0].pid}_p{imageModel.data[0].p}.{imageModel.data[0].ext}";
                string uploadDate = HtmlHelper.GetDateFromUrl(imageModel.data[0].urls.original, urlEnd);
                string url = $"https://i.pixiv.re/img-original/img/{uploadDate}/{urlEnd}";
                MessageBox.Show(url);
                var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All });

                var bytes = await client.GetByteArrayAsync(url);
                var ms = new MemoryStream(bytes);
                var newBitmap = new BitmapImage();
                newBitmap.BeginInit();
                newBitmap.CacheOption = BitmapCacheOption.OnLoad;
                newBitmap.StreamSource = ms;
                newBitmap.EndInit();
                vm.Background.ImageSource = newBitmap;

            }
            */
            vm.Background = new ImageBrush();
            if (App.Current.DataModel.UseXunkongWallpaper)
            {
                vm.Background.Stretch = Stretch.UniformToFill;
                var uri = new Uri("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                var file = Path.Combine(AppContext.BaseDirectory, "Config/Wallpaper.jpg");
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

                string imageJson = await HtmlHelper.GetInfoFromHtmlAsync("Image");
                DailyImageModel dailyImage = JsonConvert.DeserializeObject<DailyImageModel>(imageJson);
                DateTime date = DateTime.Now;
                int week = (int)date.DayOfWeek;
                if (dailyImage.ImageInfo[week].ImageDate != App.Current.DataModel.ImageDate||!File.Exists(file))
                {
                    try
                    {
                        string url = $"https://pixiv.re/{dailyImage.ImageInfo[week].ImagePid}.jpg";
                        var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All });
                        var bytes = await client.GetByteArrayAsync(url);
                        var ms = new MemoryStream(bytes);
                        var newBitmap = new BitmapImage();
                        newBitmap.BeginInit();
                        newBitmap.CacheOption = BitmapCacheOption.OnLoad;
                        newBitmap.StreamSource = ms;
                        newBitmap.EndInit();
                        vm.Background.ImageSource = newBitmap;
                        await File.WriteAllBytesAsync(file, bytes);
                        App.Current.DataModel.ImageDate = dailyImage.ImageInfo[week].ImageDate;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                Uri uri = new("pack://application:,,,/Images/MainBackground.jpg", UriKind.Absolute);
                if (!App.Current.DataModel.IsWebBg)
                {
                    vm.Background.ImageSource = new BitmapImage(uri);
                    string bgurl = await HtmlHelper.GetInfoFromHtmlAsync("bg");
                    var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All });
                    if (bgurl != null)
                    {
                        var bytes = await client.GetByteArrayAsync(bgurl);
                        var ms = new MemoryStream(bytes);
                        var newBitmap = new BitmapImage();
                        newBitmap.BeginInit();
                        newBitmap.CacheOption = BitmapCacheOption.OnLoad;
                        newBitmap.StreamSource = ms;
                        newBitmap.EndInit();
                        vm.Background.ImageSource = newBitmap;
                        vm.Background.Stretch = Stretch.UniformToFill;
                    }
                    else
                    {
                        vm.Background.ImageSource = new BitmapImage(uri);
                        vm.Background.Stretch = Stretch.UniformToFill;
                    }
                    return;
                }
                vm.Background.ImageSource = new BitmapImage(uri);
                vm.Background.Stretch = Stretch.UniformToFill;

                var bg = App.Current.DataModel.BackgroundPath;
                if (File.Exists(bg))
                {
                    using var fs = File.OpenRead(bg);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = fs;
                    bitmap.EndInit();
                    vm.Background.ImageSource = bitmap;
                }
            }
        }

        /// <summary>
        /// 检查程序所需要的配置文件
        /// </summary>
        /// <param name="main"></param>
        public void CheckConfig(MainWindow main)
        {
            if (!Directory.Exists(@"UserData"))
            {
                Directory.CreateDirectory("UserData");
            }
            if (!File.Exists(Path.Combine(App.Current.DataModel.GamePath ?? "", "Yuanshen.exe")) &&
                !File.Exists(Path.Combine(App.Current.DataModel.GamePath ?? "", "GenshinImpact.exe")))
            {
                main.MainGrid.Children.Add(new Views.GuidePage());
            }
        }
    }
}
