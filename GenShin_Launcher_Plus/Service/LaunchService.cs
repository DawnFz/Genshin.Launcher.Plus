using DGP.Genshin.FPSUnlocking;
using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using GenShin_Launcher_Plus.Service.IService;

namespace GenShin_Launcher_Plus.Service
{
    public class LaunchService : ILaunchService
    {
        private IDialogCoordinator dialogCoordinator;
        public LaunchService(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            ReadUserList();
            CreateGamePortList();
        }

        /// <summary>
        /// 异步启动游戏并且传入游戏参数
        /// </summary>
        /// <returns></returns>
        public async Task RunGameAsync()
        {
            //从Config中读取启动参数
            string gameMain = Path.Combine(App.Current.DataModel.GamePath, "YuanShen.exe");
            string arg = new CommandLineBuilder()
                .AppendIf("-popupwindow", App.Current.DataModel.IsPopup)
                .Append("-screen-fullscreen", App.Current.DataModel.FullSize)
                .Append("-screen-height", App.Current.DataModel.Height)
                .Append("-screen-width", App.Current.DataModel.Width)
                .ToString();

            //判断游戏文件、目录是否存在
            if (!File.Exists(gameMain))
            {
                gameMain = Path.Combine(App.Current.DataModel.GamePath, "GenshinImpact.exe");
                if (!File.Exists(gameMain))
                {
                    await dialogCoordinator.ShowMessageAsync(
                        this, App.Current.Language.Error,
                        App.Current.Language.PathErrorMessageStr,
                        MessageDialogStyle.Affirmative,
                        new MetroDialogSettings()
                        { AffirmativeButtonText = App.Current.Language.Determine });
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
                    WorkingDirectory = App.Current.DataModel.GamePath,
                    Arguments = arg,
                }
            };

            //判断是否启用解锁FPS
            if (App.Current.DataModel.IsUnFPS)
            {
                Unlocker unlocker = int.TryParse(App.Current.DataModel.MaxFps, out int targetFps)
                    ? new Unlocker(game, targetFps)
                    : new Unlocker(game, 144);
                UnlockResult result = await unlocker.StartProcessAndUnlockAsync();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                //判断是否开启启动游戏后关闭原神启动器Plus
                if (App.Current.DataModel.IsRunThenClose)
                {
                    game.Start();
                    Environment.Exit(0);
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

        /// <summary>
        /// 读取用户文件到NoticeOverAllBase中的列表
        /// </summary>
        public void ReadUserList()
        {
            App.Current.NoticeOverAllBase.UserLists = new List<UserListModel>();
            DirectoryInfo TheFolder = new(@"UserData");
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                App.Current.NoticeOverAllBase.UserLists.Add(new UserListModel { UserName = NextFile.Name });
            }
        }
        /// <summary>
        /// 创建NoticeOverAllBase中的客户端列表
        /// </summary>
        public void CreateGamePortList()
        {
            App.Current.NoticeOverAllBase.GamePortLists = new();
            App.Current.NoticeOverAllBase.GamePortLists.Add(
                new GamePortListModel { GamePort = App.Current.Language.GameClientTypePStr });
            App.Current.NoticeOverAllBase.GamePortLists.Add(
                new GamePortListModel { GamePort = App.Current.Language.GameClientTypeBStr });
        }
    }
}
