using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;
using System.Collections.Generic;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class HomePageViewModel : ObservableObject

    {
        private IDialogCoordinator dialogCoordinator;
        public HomePageViewModel(IDialogCoordinator instance)
        {
            IniModel = new SettingsIniModel();
            dialogCoordinator = instance;
            RunGameCommand = new RelayCommand(RunGame);
            IniModel.SwitchUser = $"账号：{IniControl.SwitchUser}";

            if (IniControl.SwitchUser != null && IniControl.SwitchUser != "")
            { IniModel.IsSwitchUser = "Visible"; }
            else
            { IniModel.IsSwitchUser = "Hidden"; }

            CreateGamePortList();
            GetGamePort();
        }

        private SettingsIniModel _IniModel;
        public SettingsIniModel IniModel
        {
            get => _IniModel;
            set => SetProperty(ref _IniModel, value);
        }

        private void GetGamePort()
        {
            if (File.Exists(Path.Combine(IniControl.GamePath, "config.ini")))
            {
                if (IniControl.Cps == "pcadbdpz")
                { IniModel.SwitchPort = "客户端：官方服务器"; }
                else if (IniControl.Cps == "bilibili")
                { IniModel.SwitchPort = "客户端：哔哩哔哩服"; }
                else if (IniControl.Cps == "mihoyo")
                { IniModel.SwitchPort = "客户端：通用国际服"; }
                else
                { IniModel.SwitchPort = "客户端：未知客户端"; }
            }
            else
            { IniModel.SwitchPort = "客户端：未知客户端"; }
        }

        //用户列表
        public List<UserListModel> _UserLists;
        public List<UserListModel> UserLists
        {
            get => _UserLists;
            set => SetProperty(ref _UserLists, value);
        }
        private void ReadUserList()
        {
            UserLists = new List<UserListModel>();
            DirectoryInfo TheFolder = new(@"UserData");
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                UserLists.Add(new UserListModel { UserName = NextFile.Name });
            }
        }

        //游戏客户端列表
        private List<GamePortListModel> _GamePortLists;
        public List<GamePortListModel> GamePortLists
        {
            get => _GamePortLists;
            set => SetProperty(ref _GamePortLists, value);
        }
        private void CreateGamePortList()
        {
            GamePortLists = new List<GamePortListModel>();
            GamePortLists.Add(new GamePortListModel { GamePort = "官方" });
            GamePortLists.Add(new GamePortListModel { GamePort = "哔哩" });
            GamePortLists.Add(new GamePortListModel { GamePort = "国际" });
        }

        public ICommand RunGameCommand { get; set; }
        private async void RunGame()
        {
            if (IniControl.isUnFPS)
            {
                Process.Start(@"unlockfps.exe");
            }
            string gameMain = Path.Combine(IniControl.GamePath, "YuanShen.exe");
            var argBuilder = new CommandLineBuilder();
            argBuilder.AddOption("-screen-fullscreen ", Convert.ToString(IniControl.FullSize));
            argBuilder.AddOption("-screen-height ", IniControl.Height);
            argBuilder.AddOption("-screen-width ", IniControl.Width);
            argBuilder.AddOption("-pop ", IniControl.isPopup ? " -popupwindow " : "");

            if (!File.Exists(gameMain))
            {
                gameMain = Path.Combine(IniControl.GamePath, "GenshinImpact.exe");
                if (!File.Exists(gameMain))
                {
                    await dialogCoordinator.ShowMessageAsync(this, "错误", "游戏路径为空或游戏文件不存在\r\n请点击右侧设置按钮进入设置填写游戏目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                    return;
                }
            }

            Task StartGame = new(() =>
            {
                ProcessStartInfo info = new()
                {
                    FileName = gameMain,
                    Verb = "runas",
                    UseShellExecute = true,
                    WorkingDirectory = IniControl.GamePath,
                    Arguments = argBuilder.ToString()
                };
                Process.Start(info);
            });
            StartGame.Start();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
        }
    }
}
