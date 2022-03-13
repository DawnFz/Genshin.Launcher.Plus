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
            languages = MainBase.lang;
            RunGameCommand = new AsyncRelayCommand(RunGameAsync);

            if (MainBase.IniModel.SwitchUser != null && MainBase.IniModel.SwitchUser != "")
            {
                MainBase.noab.IsSwitchUser = "Visible";
                IsSwitchUser = "Visible";
                MainBase.noab.SwitchUser = $"{languages.UserNameLab} : {MainBase.IniModel.SwitchUser}";
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
        public LanguagesModel languages { get; set; }
        /// <summary>
        /// 此方法非常稳健，请不要尝试优化此代码
        /// </summary>
        private void GetGamePort()
        {
            if (File.Exists(Path.Combine(MainBase.IniModel.GamePath == null ? "" : MainBase.IniModel.GamePath, "config.ini")))
            {
                if (MainBase.IniModel.Cps == "pcadbdpz")
                { MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypePStr}"; }
                else if (MainBase.IniModel.Cps == "bilibili")
                { MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeBStr}"; }
                else if (MainBase.IniModel.Cps == "mihoyo")
                { MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeMStr}"; }
                else
                { MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeNullStr}"; }
            }
            else
            { MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeNullStr}"; }
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
                    MainBase.noab.SwitchUser = $"{languages.UserNameLab} : {SwitchUserValue}";
                    //更改注册表账号状态
                    MainBase.IniModel.SwitchUser = SwitchUserValue;
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
                if (File.Exists(Path.Combine(MainBase.IniModel.GamePath, "config.ini")))
                {
                    if (MainBase.IniModel.Cps == "mihoyo")
                    {
                        _IsGamePortLists = "Hidden";
                    }
                    else
                    {
                        _IsGamePortLists = "Visible";
                    }
                }
                else
                {
                    _IsGamePortLists = "Hidden";
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
            GamePortLists.Add(new GamePortListModel { GamePort = languages.GameClientTypePStr });
            GamePortLists.Add(new GamePortListModel { GamePort = languages.GameClientTypeBStr });
        }

        //游戏端口列表索引
        public int GamePortListIndex
        {
            get
            {
                if (File.Exists(Path.Combine(MainBase.IniModel.GamePath, "config.ini")))
                {
                    if (MainBase.IniModel.Cps == "pcadbdpz")
                    {
                        MainBase.noab.SwitchPort = $"{MainBase.lang.GameClientStr} : {MainBase.lang.GameClientTypePStr}";
                        return 0;
                    }
                    else if (MainBase.IniModel.Cps == "bilibili")
                    {
                        MainBase.noab.SwitchPort = $"{MainBase.lang.GameClientStr} : {MainBase.lang.GameClientTypeBStr}";
                        return 1;
                    }
                    else if (MainBase.IniModel.Cps == "mihoyo")
                    {
                        MainBase.noab.SwitchPort = $"{MainBase.lang.GameClientStr} : {MainBase.lang.GameClientTypeMStr}";
                        return -1;
                    }
                    else
                    {
                        MainBase.noab.SwitchPort = $"{MainBase.lang.GameClientStr} : {MainBase.lang.GameClientTypeNullStr}";
                        return -1;
                    }
                }
                else
                {
                    MainBase.noab.SwitchPort = $"{MainBase.lang.GameClientStr} : {MainBase.lang.GameClientTypeNullStr}";
                    return -1;
                }
            }
            set
            {
                if (File.Exists(Path.Combine(MainBase.IniModel.GamePath, "config.ini")) == true)
                {
                    if (MainBase.IniModel.Cps != "mihoyo")
                    {
                        switch (value)
                        {
                            case 0:
                                MainBase.IniModel.Cps = "pcadbdpz";
                                MainBase.IniModel.Channel = 1;
                                MainBase.IniModel.Sub_channel = 1;
                                if (File.Exists(Path.Combine(MainBase.IniModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                                    File.Delete(Path.Combine(MainBase.IniModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                                MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypePStr}";
                                break;
                            case 1:
                                MainBase.IniModel.Cps = "bilibili";
                                MainBase.IniModel.Channel = 14;
                                MainBase.IniModel.Sub_channel = 0;
                                if (!File.Exists(Path.Combine(MainBase.IniModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                                {
                                    FilesControl utils = new();
                                    try
                                    {
                                        utils.FileWriter("StaticRes/mihoyosdk.dll", Path.Combine(MainBase.IniModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeBStr}";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PathErrorMessageStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
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
        private async Task RunGameAsync()
        {
            //从Config中读取启动参数
            string gameMain = Path.Combine(MainBase.IniModel.GamePath, "YuanShen.exe");
            var argBuilder = new CommandLineBuilder();
            argBuilder.AddOption("-screen-fullscreen ", Convert.ToString(MainBase.IniModel.FullSize));
            argBuilder.AddOption("-screen-height ", MainBase.IniModel.Height);
            argBuilder.AddOption("-screen-width ", MainBase.IniModel.Width);
            argBuilder.AddOption("-pop ", MainBase.IniModel.isPopup ? " -popupwindow " : "");
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
            //创建Task线程启动游戏

            Process game = new()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = gameMain,
                    Verb = "runas",
                    UseShellExecute = true,
                    WorkingDirectory = MainBase.IniModel.GamePath,
                    Arguments = argBuilder.ToString(),
                }
            };

            if (MainBase.IniModel.isUnFPS)
            {
                Unlocker unlocker;
                if (int.TryParse(MainBase.IniModel.MaxFps, out int targetFps))
                {
                    unlocker = new Unlocker(game, targetFps);
                }
                else
                {
                    unlocker = new Unlocker(game, 144);
                }
                var result = await unlocker.StartProcessAndUnlockAsync();
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
