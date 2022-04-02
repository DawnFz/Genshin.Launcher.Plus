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

            App.Current.LoadProgramCore.LoadUpdateCoreAsync();
            string newver = App.Current.UpdateObject.Version;
            string version = Application.ResourceAssembly.GetName().Version.ToString();
            if (version != newver && !App.Current.IsLoading)
            {
                main.MainGrid.Children.Add(new Views.UpdatePage());
                App.Current.IsLoading = true;
            }
        }
    }
}
