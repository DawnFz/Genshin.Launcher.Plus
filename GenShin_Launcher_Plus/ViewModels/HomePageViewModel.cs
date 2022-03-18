using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;
using System.Collections.Generic;
using DGP.Genshin.FPSUnlocking;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// 这个类为启动页的ViewModel，目前正在改进
    /// </summary>
    public class HomePageViewModel : ObservableObject
    {
        private IDialogCoordinator dialogCoordinator;
        public HomePageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            RunGameCommand = new AsyncRelayCommand(RunGameAsync);
            if (MainBase.IniModel.SwitchUser != null && MainBase.IniModel.SwitchUser != "")
            {
                MainBase.noab.IsSwitchUser = "Visible";
                MainBase.noab.SwitchUser = $"{languages.UserNameLab} : {MainBase.IniModel.SwitchUser}";
            }
            else
            {
                MainBase.noab.IsSwitchUser = "Hidden";
            }
            CreateGamePortList();
            ReadUserList();
        }
        public LanguagesModel languages { get => MainBase.lang; }

        private void CreateGamePortList()
        {
            MainBase.noab.GamePortLists = new List<GamePortListModel>();
            MainBase.noab.GamePortLists.Add(new GamePortListModel { GamePort = languages.GameClientTypePStr });
            MainBase.noab.GamePortLists.Add(new GamePortListModel { GamePort = languages.GameClientTypeBStr });
        }

        private void ReadUserList()
        {
            MainBase.noab.UserLists = new List<UserListModel>();
            DirectoryInfo TheFolder = new(@"UserData");
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                MainBase.noab.UserLists.Add(new UserListModel { UserName = NextFile.Name });
            }
        }

        public ICommand RunGameCommand { get; set; }
        private async Task RunGameAsync()
        {
            //从Config中读取启动参数
            string gameMain = Path.Combine(MainBase.IniModel.GamePath, "YuanShen.exe");
            string arg = new CommandLineBuilder()
                .AppendIf("-popupwindow", MainBase.IniModel.isPopup)
                .Append("-screen-fullscreen", MainBase.IniModel.FullSize)
                .Append("-screen-height", MainBase.IniModel.Height)
                .Append("-screen-width", MainBase.IniModel.Width)
                .ToString();
            
            //判断游戏文件、目录是否存在
            if (!File.Exists(gameMain))
            {
                gameMain = Path.Combine(MainBase.IniModel.GamePath, "GenshinImpact.exe");
                if (!File.Exists(gameMain))
                {
                    await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PathErrorMessageStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                    return;
                }
            }
            Application.Current.MainWindow.WindowState = WindowState.Minimized;

            Process game = new()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = gameMain,
                    Verb = "runas",
                    UseShellExecute = true,
                    WorkingDirectory = MainBase.IniModel.GamePath,
                    Arguments = arg,
                }
            };

            if (MainBase.IniModel.isUnFPS)
            {
                Unlocker unlocker = int.TryParse(MainBase.IniModel.MaxFps, out int targetFps) 
                    ? new Unlocker(game, targetFps) 
                    : new Unlocker(game, 144);
                UnlockResult result = await unlocker.StartProcessAndUnlockAsync();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                if (game.Start())
                {
                    await game.WaitForExitAsync();
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }
            }
        }
    }
}
