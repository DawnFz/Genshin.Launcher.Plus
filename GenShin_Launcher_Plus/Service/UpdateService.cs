using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.Service.IService;
using GenShin_Launcher_Plus.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace GenShin_Launcher_Plus.Service
{
    public class UpdateService : IUpadteService
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
    }
}
