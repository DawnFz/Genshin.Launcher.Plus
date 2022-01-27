using GenShin_Launcher_Plus.Core;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenShin_Launcher_Plus.Command;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class UpdatePageViewModel : NotificationObject
    {
        private IDialogCoordinator dialogCoordinator;
        public UpdatePageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            UpdateRunCommand = new DelegateCommand { ExecuteAction = new Action<object>(UpdateRun) };
        }
        public string Notify
        {
            get
            {
                string temp = FilesControl.MiddleText(FilesControl.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$notify$]", "[#notify#]");
                string notify = temp.Replace("/n/", Environment.NewLine);
                return notify;
            }
        }
        public string Title
        {
            get
            {
                return FilesControl.MiddleText(FilesControl.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$msgtl$]", "[#msgtl#]");
            }
        }



        private bool _ButtonIsEnabled = true;
        public bool ButtonIsEnabled
        {
            get { return _ButtonIsEnabled;}
            set { _ButtonIsEnabled = value;OnPropChanged("ButtonIsEnabled"); }
        }

        private double _DownloadBarMax;
        public double DownloadBarMax
        {
            get { return _DownloadBarMax; }
            set { _DownloadBarMax = value; OnPropChanged("DownloadBarMax"); }
        }

        private double _DownloadBarValue;
        public double DownloadBarValue
        {
            get { return _DownloadBarValue; }
            set { _DownloadBarValue = value; OnPropChanged("DownloadBarValue"); }
        }

        private string _DownloadLabel;
        public string DownloadLabel
        {
            get { return _DownloadLabel; }
            set { _DownloadLabel = value; OnPropChanged("DownloadLabel"); }
        }


        public DelegateCommand UpdateRunCommand { get; set; }
        private async void UpdateRun(object parameter)
        {
            ButtonIsEnabled = false;
            string downver = FilesControl.MiddleText(FilesControl.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$gitv$]", "[#gitv#]");
            if (await HttpFileExistAsync($"https://cdn.jsdelivr.net/gh/DawnFz/GenShin-LauncherDIY@{ downver }/Genshin-LauncherDIY/UpdateFile/GenShinLauncher.png") == true)
            {
                await DownloadHttpFileAsync($"https://cdn.jsdelivr.net/gh/DawnFz/GenShin-LauncherDIY@{ downver }/Genshin-LauncherDIY/UpdateFile/GenShinLauncher.png", @"UpdateTemp.upd");
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "错误提示", "网络更新文件资源不存在或服务器网络错误", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                ButtonIsEnabled = true;
            }
        }

        private Lazy<HttpClient> LazyClient { get; } = new Lazy<HttpClient>(() => new HttpClient() { Timeout = Timeout.InfiniteTimeSpan });

        public async Task DownloadHttpFileAsync(string url, string fileName)
        {
            HttpResponseMessage response = await LazyClient.Value.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
            DownloadBarMax = (double)response.Content.Headers.ContentLength;

            using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                int bufferSize = 1024;
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true))
                {
                    byte[] buffer = new byte[bufferSize];
                    long progressBarValue = 0;
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        progressBarValue += bytesRead;
                        //fire and forget
                        SetProgressBar(progressBarValue);
                    }
                    while (bytesRead > 0);
                }
            }
            if ((await dialogCoordinator.ShowMessageAsync(this, "提示", "下载完成，是否现在进行更新操作\r\n确定后只需等待5秒将自动启动新版本", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "取消", NegativeButtonText = "确定" })) != MessageDialogResult.Affirmative)
            {
                Process.Start(@"Update.exe");
                Environment.Exit(0);
            }
            else 
            {
                ButtonIsEnabled = true;
            }
        }
        private async Task<bool> HttpFileExistAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await LazyClient.Value.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                return response.IsSuccessStatusCode;
            }
            catch { }
            return false;
        }
        public delegate void ProgressBarSetter(double value);
        public void SetProgressBar(double value)
        {
            DownloadBarValue = value;
            DownloadLabel = $"下载进度:{value / DownloadBarMax:p2}";
        }




    }
}
