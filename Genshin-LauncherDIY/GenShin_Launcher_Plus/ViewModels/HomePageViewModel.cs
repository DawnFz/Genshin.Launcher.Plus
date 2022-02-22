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

namespace GenShin_Launcher_Plus.ViewModels
{
    public class HomePageViewModel : ObservableObject

    {
        private IDialogCoordinator dialogCoordinator;
        public HomePageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            RunGameCommand = new RelayCommand(RunGame);

            if (IniControl.SwitchUser != null && IniControl.SwitchUser != "")
            {
                MainBase.noab.IsSwitchUser = "Visible";
                IsSwitchUser = "Visible";
                MainBase.noab.SwitchUser = $"账号：{IniControl.SwitchUser}";
            }
            else
            {
                MainBase.noab.IsSwitchUser = "Hidden";
                IsSwitchUser = "Hidden";
            }
            CreateGamePortList();
            ReadUserList();
            GetGamePort();
        }

        private void GetGamePort()
        {
            if (File.Exists(Path.Combine(IniControl.GamePath, "config.ini")))
            {
                if (IniControl.Cps == "pcadbdpz")
                { MainBase.noab.SwitchPort = "客户端：官方服务器"; }
                else if (IniControl.Cps == "bilibili")
                { MainBase.noab.SwitchPort = "客户端：哔哩哔哩服"; }
                else if (IniControl.Cps == "mihoyo")
                { MainBase.noab.SwitchPort = "客户端：通用国际服"; }
                else
                { MainBase.noab.SwitchPort = "客户端：未知客户端"; }
            }
            else
            { MainBase.noab.SwitchPort = "客户端：未知客户端"; }
        }

        private string _SwitchUserValue;
        public string SwitchUserValue
        {
            get => _SwitchUserValue;
            set
            {
                SetProperty(ref _SwitchUserValue, value);
                if (SwitchUserValue != null && SwitchUserValue != "")
                {
                    MainBase.noab.SwitchUser = $"账号：{SwitchUserValue}";
                    //更改注册表账号状态
                    IniControl.SwitchUser = SwitchUserValue;
                    RegistryControl registryControl = new();
                    registryControl.SetToRegedit(SwitchUserValue);
                }
            }
        }

        //选择账户Combobox控件状态
        private string _IsSwitchUser;
        public string IsSwitchUser
        {
            get => _IsSwitchUser;
            set => SetProperty(ref _IsSwitchUser, value);
        }


        //选择游戏端口Combobox控件状态
        private string _IsGamePortLists;
        public string IsGamePortLists
        {
            get
            {
                if (IniControl.Cps == "mihoyo")
                {
                    _IsGamePortLists = "Hidden";
                }
                else
                {
                    _IsGamePortLists = "Visible";
                }
                return _IsGamePortLists;
            }
            set => SetProperty(ref _IsGamePortLists, value);
        }
        //游戏端口列表
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
        }

        //游戏端口列表索引
        public int GamePortListIndex
        {
            get
            {
                return 0;
            }
            set
            {
                if (File.Exists(Path.Combine(IniControl.GamePath, "config.ini")) == true)
                {
                    if (IniControl.Cps != "mihoyo")
                    {
                        switch (value)
                        {
                            case 0:
                                IniControl.Cps = "pcadbdpz";
                                IniControl.Channel = 1;
                                IniControl.Sub_channel = 1;
                                if (File.Exists(Path.Combine(IniControl.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                                    File.Delete(Path.Combine(IniControl.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                                MainBase.noab.SwitchPort = "客户端：官方服务器";
                                break;
                            case 1:
                                IniControl.Cps = "bilibili";
                                IniControl.Channel = 14;
                                IniControl.Sub_channel = 0;
                                if (!File.Exists(Path.Combine(IniControl.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                                {
                                    FilesControl utils = new();
                                    try
                                    {
                                        utils.FileWriter("StaticRes/mihoyosdk.dll", Path.Combine(IniControl.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                MainBase.noab.SwitchPort = "客户端：哔哩哔哩服";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    dialogCoordinator.ShowMessageAsync(this, "错误", "游戏路径为空或游戏文件不存在\r\n请点击右侧设置按钮进入设置填写游戏目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                }
            }
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

        public ICommand RunGameCommand { get; set; }
        private async void RunGame()
        {
            //判断是否开启UnlockFps
            if (IniControl.isUnFPS)
            {
                Process.Start(@"unlockfps.exe");
            }
            //从Config中读取启动参数
            string gameMain = Path.Combine(IniControl.GamePath, "YuanShen.exe");
            var argBuilder = new CommandLineBuilder();
            argBuilder.AddOption("-screen-fullscreen ", Convert.ToString(IniControl.FullSize));
            argBuilder.AddOption("-screen-height ", IniControl.Height);
            argBuilder.AddOption("-screen-width ", IniControl.Width);
            argBuilder.AddOption("-pop ", IniControl.isPopup ? " -popupwindow " : "");
            //判断游戏文件、目录是否存在
            if (!File.Exists(gameMain))
            {
                gameMain = Path.Combine(IniControl.GamePath, "GenshinImpact.exe");
                if (!File.Exists(gameMain))
                {
                    await dialogCoordinator.ShowMessageAsync(this, "错误", "游戏路径为空或游戏文件不存在\r\n请点击右侧设置按钮进入设置填写游戏目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                    return;
                }
            }
            //创建Task线程启动游戏
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
