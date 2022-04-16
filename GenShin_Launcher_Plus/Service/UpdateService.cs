using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.Service.IService;
using GenShin_Launcher_Plus.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using Newtonsoft.Json;

namespace GenShin_Launcher_Plus.Service
{
    public class UpdateService : IUpdateService
    {
        public async void UpdateRun(UpdatePageViewModel vm)
        {
            if (vm.ButtonIsEnabled)
            {
                vm.ButtonIsEnabled = false;
                vm.ViewControlVisibility = "Visibility";
                string updatefile = vm.UseGlobalUrlCheck ? App.Current.UpdateObject.GlobalDownloadUrl : App.Current.UpdateObject.DownloadUrl;
                if (await vm.DFC.HttpFileExistAsync(updatefile) == true)
                {
                    //等待文件下载
                    await vm.DFC.DownloadHttpFileAsync(updatefile, @"UpdateTemp.zip");
                    if ((await vm.dialogCoordinator.ShowMessageAsync(
                        vm, App.Current.Language.TipsStr,
                        App.Current.Language.DownloadComStr,
                        MessageDialogStyle.AffirmativeAndNegative,
                        new MetroDialogSettings()
                        {
                            AffirmativeButtonText = App.Current.Language.Cancel,
                            NegativeButtonText = App.Current.Language.Determine
                        })) != MessageDialogResult.Affirmative)
                    {
                        //执行更新操作
                        if (FileHelper.UnZip("UpdateTemp.zip"))
                        {
                            File.Delete(@"UpdateTemp.zip");
                            Process.Start(@"Update.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            File.Move(@"UpdateTemp.zip", @"UpdateTemp.upd");
                            Process.Start(@"Update.exe");
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        vm.ButtonIsEnabled = true;
                        vm.ViewControlVisibility = "Hidden";
                    }
                }
                else
                {
                    await vm.dialogCoordinator.ShowMessageAsync(
                        vm, App.Current.Language.Error,
                        App.Current.Language.DownFailedStr,
                        MessageDialogStyle.Affirmative,
                        new MetroDialogSettings()
                        { AffirmativeButtonText = App.Current.Language.Determine });
                    vm.ViewControlVisibility = "Hidden";
                    vm.ButtonIsEnabled = true;
                }
            }
            else
            {
                await vm.dialogCoordinator.ShowMessageAsync(
                    vm, App.Current.Language.TipsStr,
                    App.Current.Language.RepWarnStr,
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings()
                    { AffirmativeButtonText = App.Current.Language.Determine });
            }
        }

        /// <summary>
        /// 检查是否存在更新信息
        /// 初始化更新页面的内容
        /// </summary>
        public async void CheckUpdate(MainWindow main)
        {
            try
            {
                FileHelper.ExtractEmbededAppResource("StaticRes/Update.dll", @"Update.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (App.Current.IniModel.ReadLang == "Lang_CN" ||
                App.Current.IniModel.ReadLang == null ||
                App.Current.IniModel.ReadLang == string.Empty)
            {
                string json = await HtmlHelper.GetInfoFromHtmlAsync("UpdateCN");
                App.Current.UpdateObject = JsonConvert.DeserializeObject<UpdateModel>(json) ?? new();
            }
            else
            {
                string json = await HtmlHelper.GetInfoFromHtmlAsync("UpdateGlobal");
                App.Current.UpdateObject = JsonConvert.DeserializeObject<UpdateModel>(json) ?? new();
            }

            string newver = App.Current.UpdateObject.Version;
            string version = Application.ResourceAssembly.GetName().Version.ToString();
            if (version != newver &&
                newver != null &&
                newver != String.Empty &&
                !App.Current.IsLoading)
            {
                main.MainGrid.Children.Add(new Views.UpdatePage());
                App.Current.IsLoading = true;
            }
        }
    }
}
