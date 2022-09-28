using System;
using System.Net.Http;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GenShin_Launcher_Plus.ViewModels;

namespace GenShin_Launcher_Plus.Helper
{
    /// <summary>
    /// 用于处理文件下载
    /// </summary>
    public class DownloadHelper : ObservableObject
    {
        private double _DownloadBarMax;
        public double DownloadBarMax
        {
            get => _DownloadBarMax;
            set => SetProperty(ref _DownloadBarMax, value);
        }

        private double _DownloadBarValue;
        public double DownloadBarValue
        {
            get => _DownloadBarValue;
            set => SetProperty(ref _DownloadBarValue, value);
        }

        private string _DownloadLabel;
        public string DownloadLabel
        {
            get => _DownloadLabel;
            set => SetProperty(ref _DownloadLabel, value);
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
                int bufferSize = 2048;
                using (FileStream fileStream = new(fileName, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true))
                {
                    byte[] buffer = new byte[bufferSize];
                    long progressBarValue = 0;
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        progressBarValue += bytesRead;
                        DownloadBarValue = progressBarValue;
                        //fire and forget
                        SetProgressBar();
                    }
                    while (bytesRead > 0);
                }
            }
        }

        /// <summary>
        /// 检查网络文件是否存在
        /// </summary>
        public async Task<bool> HttpFileExistAsync(string url)
        {
            try
            {
                return (await LazyClient.Value.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)).IsSuccessStatusCode;
            }
            catch 
            { 
                return false;
            }
        }

        public delegate void ProgressBarSetter(double value);
        public void SetProgressBar()
        {
            DownloadLabel = $"{App.Current.Language.DownProgress}:{DownloadBarValue / DownloadBarMax:p2}";
        }
    }
}
