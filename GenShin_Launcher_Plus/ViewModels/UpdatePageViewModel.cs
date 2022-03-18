using GenShin_Launcher_Plus.Core;
using MahApps.Metro.Controls.Dialogs;
using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using GenShin_Launcher_Plus.Models;
using System.Windows;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// 更新页面的ViewModel 
    /// 集成了更新页面的操作实现逻辑和UI更新绑定
    /// </summary>
    
    public class UpdatePageViewModel : ObservableObject
    {
        private IDialogCoordinator dialogCoordinator;
        public UpdatePageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            DFC = new();
            UpdateRunCommand = new RelayCommand(UpdateRun);
            ViewControlVisibility = "Hidden";
        }
        private FileDownloader _DFC;
        public FileDownloader DFC
        {
            get=> _DFC;
            set=> SetProperty(ref _DFC, value);
        }
        public LanguagesModel languages { get => MainBase.lang; }
        public string Notify
        {
            get => MainBase.update.Content;
        }
        public string Title
        {
            get => MainBase.update.Title;
        }

        private bool _ButtonIsEnabled = true;
        public bool ButtonIsEnabled
        {
            get => _ButtonIsEnabled;
            set => SetProperty(ref _ButtonIsEnabled, value);
        }

        private string _ViewControlVisibility;
        public string ViewControlVisibility
        {
            get => _ViewControlVisibility;
            set => SetProperty(ref _ViewControlVisibility, value);
        }

        private bool _UseGlobalUrlCheck;
        public bool UseGlobalUrlCheck
        {
            get => _UseGlobalUrlCheck;
            set => SetProperty(ref _UseGlobalUrlCheck, value);
        }

        //开始更新命令
        public ICommand UpdateRunCommand { get; set; }
        private async void UpdateRun()
        {
            if (ButtonIsEnabled)
            {
                ButtonIsEnabled = false;
                ViewControlVisibility = "Visibility";
                string updatefile = UseGlobalUrlCheck ? MainBase.update.GlobalDownloadUrl : MainBase.update.DownloadUrl;
                if (await DFC.HttpFileExistAsync(updatefile) == true)
                {
                    await DFC.DownloadHttpFileAsync(updatefile, @"UpdateTemp.zip");
                    if ((await dialogCoordinator.ShowMessageAsync(this, languages.TipsStr, languages.DownloadComStr, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = languages.Cancel, NegativeButtonText = languages.Determine })) != MessageDialogResult.Affirmative)
                    {
                        //解压更新了的ZIP文件
                        if (FileHelper.UnZip("UpdateTemp.zip", @""))
                        {
                            File.Delete(@"UpdateTemp.zip");
                            Process.Start(@"Update.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            MessageBox.Show("解压更新文件失败，可能是文件已损坏 !");
                            File.Delete(@"UpdateTemp.zip");
                        }
                    }
                    else
                    {
                        ButtonIsEnabled = true;
                        ViewControlVisibility = "Hidden";
                    }
                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.DownFailedStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                    ViewControlVisibility = "Hidden";
                    ButtonIsEnabled = true;
                }
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, languages.TipsStr, languages.RepWarnStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
            }
        }
    }
}
