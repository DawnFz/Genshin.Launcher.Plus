using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.VisualBasic.Devices;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.Service.IService;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// 这个类是SettingsPage的ViewModel 
    /// 集成了SettingsPage所有的操作实现逻辑
    /// 目前这个类有点乱，不要乱动关于IniModel的数据
    /// 避免出现错误，开发者已经开始着手改进该类了
    /// </summary>
    public class SettingsPageViewModel : ObservableObject
    {

        //构造器
        private IDialogCoordinator dialogCoordinator;
        public SettingsPageViewModel(IDialogCoordinator instance)
        {
            //
            SettingsTitle = languages.SettingsTitle;
            ConvertingLog = languages.ConvertingLogStr;
            StateIndicator = languages.StateIndicatorDefault;
            //
            dialogCoordinator = instance;
            SettingsPageCreated();
            CreateDisplaySizeList();
            CreateGamePortList();
            CreateGameWindowModeList();
            ReadUserList();
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            ChooseGamePathCommand = new RelayCommand(ChooseGamePath);
            ChooseUnlockFpsCommand = new RelayCommand(ChooseUnlockFps);
            GameFileConvertCommand = new AsyncRelayCommand(GameFileConvert);
            ThisPageRemoveCommand = new RelayCommand(ThisPageRemove);
        }
        public LanguageModel languages { get => App.Current.Language; }

        //保存状态
        private string _SettingsTitle;
        public string SettingsTitle
        {
            get => _SettingsTitle;
            set => SetProperty(ref _SettingsTitle, value);
        }
        private string _SettingTitleColor = "#FF272727";
        public string SettingTitleColor
        {
            get => _SettingTitleColor;
            set => SetProperty(ref _SettingTitleColor, value);
        }
        private void DelaySaveButtonTitle()
        {
            Task task = new(() =>
            {
                SettingTitleColor = "#FF008C02";
                Thread.Sleep(1500);
                SettingTitleColor = "#FF272727";
            });
            task.Start();
        }
        //设置界面UI刷新绑定数据
        private string _Width;
        public string Width { get => _Width; set => SetProperty(ref _Width, value); }
        private string _Height;
        public string Height { get => _Height; set => SetProperty(ref _Height, value); }
        private bool _isUnFPS;
        public bool isUnFPS { get => _isUnFPS; set => SetProperty(ref _isUnFPS, value); }
        private string _GamePath;
        public string GamePath { get => _GamePath; set => SetProperty(ref _GamePath, value); }
        private string _SwitchUser;
        public string SwitchUser { get => _SwitchUser; set => SetProperty(ref _SwitchUser, value); }
        private int _isMihoyo;
        public int isMihoyo { get => _isMihoyo; set => SetProperty(ref _isMihoyo, value); }

        //选中分辨率的索引
        private int _DisplaySelectedIndex = -1;
        public int DisplaySelectedIndex
        {
            get => _DisplaySelectedIndex;
            set
            {
                switch (value)
                {
                    case 0:
                        Width = "3840";
                        Height = "2160";
                        break;
                    case 1:
                        Width = "2560";
                        Height = "1080";
                        break;
                    case 2:
                        Width = "1920";
                        Height = "1080";
                        break;
                    case 3:
                        Width = "1600";
                        Height = "900";
                        break;
                    case 4:
                        Width = "1360";
                        Height = "768";
                        break;
                    case 5:
                        Width = "1280";
                        Height = "1024";
                        break;
                    case 6:
                        Width = "1280";
                        Height = "720";
                        break;
                    case 7:
                        Width = Convert.ToString(SystemParameters.PrimaryScreenWidth);
                        Height = Convert.ToString(SystemParameters.PrimaryScreenHeight);
                        break;
                    default:
                        break;
                }
                IniModel.Width = Width;
                IniModel.Height = Height;
            }
        }
        //存放设置属性的实体类
        public IniModel IniModel
        {
            get => App.Current.IniModel;
        }

        //转换时的日志列表
        private string _ConvertingLog;
        public string ConvertingLog
        {
            get => _ConvertingLog;
            set => SetProperty(ref _ConvertingLog, value);
        }

        //转换时的控件状态
        private string _PageUiStatus = "true";
        public string PageUiStatus
        {
            get => _PageUiStatus;
            set => SetProperty(ref _PageUiStatus, value);
        }

        //转换状态
        private string _StateIndicator;
        public string StateIndicator
        {
            get => _StateIndicator;
            set => SetProperty(ref _StateIndicator, value);
        }
        public List<DisplaySizeListModel> DisplaySizeLists { get; set; }
        private void CreateDisplaySizeList()
        {
            DisplaySizeLists = new List<DisplaySizeListModel>
            {
                new DisplaySizeListModel { DisplaySize = "3840 × 2160  | 16:9" },
                new DisplaySizeListModel { DisplaySize = "2560 × 1080  | 21:9" },
                new DisplaySizeListModel { DisplaySize = "1920 × 1080  | 16:9" },
                new DisplaySizeListModel { DisplaySize = "1600 × 900    | 16:9" },
                new DisplaySizeListModel { DisplaySize = "1360 × 768    | 16:9" },
                new DisplaySizeListModel { DisplaySize = "1280 × 1024  |  4:3" },
                new DisplaySizeListModel { DisplaySize = "1280 × 720    | 16:9" },
                new DisplaySizeListModel{ DisplaySize = languages.AdaptiveStr },
            };
        }

        public bool ConvertState { get; set; }

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
            GamePortLists = new List<GamePortListModel>
            {
                new GamePortListModel { GamePort = languages.GameClientTypePStr },
                new GamePortListModel { GamePort = languages.GameClientTypeBStr },
                new GamePortListModel { GamePort = languages.GameClientTypeMStr }
            };
        }

        //游戏窗口模式列表
        private List<GameWindowModeListModel> _GameWindowModeList;
        public List<GameWindowModeListModel> GameWindowModeList
        {
            get => _GameWindowModeList;
            set => SetProperty(ref _GameWindowModeList, value);
        }
        private void CreateGameWindowModeList()
        {
            GameWindowModeList = new();
            GameWindowModeList.Add(new GameWindowModeListModel { GameWindowMode = languages.WindowMode });
            GameWindowModeList.Add(new GameWindowModeListModel { GameWindowMode = languages.Fullscreen });
        }

        //选择游戏路径的命令
        public ICommand ChooseGamePathCommand { get; set; }
        private void ChooseGamePath()
        {
            CommonOpenFileDialog dialog = new(languages.GameDirMsg);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                IniModel.GamePath = dialog.FileName;
                GamePath = dialog.FileName;
            }
        }

        //开始转换显示的等待条
        private string _ProgressBar = "Hidden";
        public string ProgressBar
        {
            get => _ProgressBar;
            set => SetProperty(ref _ProgressBar, value);
        }

        //选择解锁FPS的指令
        public ICommand ChooseUnlockFpsCommand { get; set; }
        private async void ChooseUnlockFps()
        {
            if (isUnFPS)
            {
                if ((await dialogCoordinator.ShowMessageAsync(this, languages.SevereWarning, languages.SevereWarningStr, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = languages.Cancel, NegativeButtonText = languages.Determine })) != MessageDialogResult.Affirmative)
                {
                    isUnFPS = true;
                }
                else
                {
                    isUnFPS = false;
                }
                App.Current.IniModel.isUnFPS = isUnFPS;
            }
        }

        //删除账号的命令
        public ICommand DeleteUserCommand { get; set; }
        private async void DeleteUser()
        {
            if (SwitchUser != "" && SwitchUser != null)
            {
                if ((await dialogCoordinator.ShowMessageAsync(this, languages.Warning, $"{languages.WarningDAW}[{SwitchUser}] ? !", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = languages.Cancel, NegativeButtonText = languages.Determine })) != MessageDialogResult.Affirmative)
                {
                    File.Delete(Path.Combine(@"UserData", SwitchUser));
                    ReadUserList();
                }
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.ErrorSA, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
            }
        }

        //保存设置的命令
        public ICommand SaveSettingsCommand { get; set; }
        private async void SaveSettings()
        {
            if (GamePath != "" && File.Exists(Path.Combine(GamePath, "Yuanshen.exe")) || File.Exists(Path.Combine(GamePath, "GenshinImpact.exe")))
            {
                App.Current.IniModel.GamePath = GamePath;
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PathErrorMessageStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                return;
            }
            if (SwitchUser != null && SwitchUser != "")
            {
                App.Current.IniModel.SwitchUser = SwitchUser;
                App.Current.NoticeOverAllBase.SwitchUser = $"{languages.UserNameLab}：{SwitchUser}";
                App.Current.NoticeOverAllBase.IsSwitchUser = "Visible";
                RegistryService registryControl = new();
                registryControl.SetToRegistry(SwitchUser);
            }
            App.Current.IniModel.Width = Width;
            App.Current.IniModel.Height = Height;
            App.Current.IniModel.isUnFPS = isUnFPS;
            if (File.Exists(Path.Combine(App.Current.IniModel.GamePath, "config.ini")))
            {
                switch (isMihoyo)
                {
                    case 0:
                        App.Current.IniModel.Cps = "pcadbdpz";
                        App.Current.IniModel.Channel = 1;
                        App.Current.IniModel.Sub_channel = 1;
                        if (File.Exists(Path.Combine(GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                            File.Delete(Path.Combine(GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                        App.Current.NoticeOverAllBase.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypePStr}";
                        App.Current.NoticeOverAllBase.IsGamePortLists = "Visible";
                        App.Current.NoticeOverAllBase.GamePortListIndex = 0;
                        break;
                    case 1:
                        App.Current.IniModel.Cps = "bilibili";
                        App.Current.IniModel.Channel = 14;
                        App.Current.IniModel.Sub_channel = 0;
                        if (!File.Exists(Path.Combine(GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                        {
                            try
                            {
                                FileHelper.ExtractEmbededAppResource("StaticRes/mihoyosdk.dll", Path.Combine(GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        App.Current.NoticeOverAllBase.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeBStr}";
                        App.Current.NoticeOverAllBase.IsGamePortLists = "Visible";
                        App.Current.NoticeOverAllBase.GamePortListIndex = 1;

                        break;
                    case 2:
                        App.Current.IniModel.Cps = "mihoyo";
                        App.Current.IniModel.Channel = 1;
                        App.Current.IniModel.Sub_channel = 0;
                        if (File.Exists(Path.Combine(GamePath, "GenshinImpact_Data/Plugins/PCGameSDK.dll")))
                            File.Delete(Path.Combine(GamePath, "GenshinImpact_Data/Plugins/PCGameSDK.dll"));
                        App.Current.NoticeOverAllBase.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeMStr}";
                        App.Current.NoticeOverAllBase.IsGamePortLists = "Hidden";
                        App.Current.NoticeOverAllBase.GamePortListIndex = -1;
                        break;
                    default:
                        break;
                }
            }
            DelaySaveButtonTitle();
            App.Current.NoticeOverAllBase.MainPagesIndex = 0;
        }

        //被创建时从Setting.ini文件读取给IniModel对象赋值
        private void SettingsPageCreated()
        {     
            SwitchUser = App.Current.NoticeOverAllBase.SwitchUser;
            GamePath = App.Current.IniModel.GamePath;
            isUnFPS = App.Current.IniModel.isUnFPS;
            Width = App.Current.IniModel.Width ?? "1600";
            Height = App.Current.IniModel.Height ?? "900";
            isMihoyo = App.Current.IniModel.Cps switch
            {
                "pcadbdpz" => 0,
                "bilibili" => 1,
                "mihoyo" => 2,
                _ => 3,
            };
        }

        //关闭设置页面
        public ICommand ThisPageRemoveCommand { get; set; }
        private void ThisPageRemove()
        {
            App.Current.NoticeOverAllBase.MainPagesIndex = 0;
        }

        //转换国际服及转换国服绑定命令
        public ICommand GameFileConvertCommand { get; set; }
        private async Task GameFileConvert()
        {
            PageUiStatus = "false";
            ProgressBar = "Visible";
            FileHelper fileHelper = new();
            if (!fileHelper.IsFileOpen(Path.Combine(App.Current.IniModel.GamePath, "Yuanshen.exe")) && !fileHelper.IsFileOpen(Path.Combine(App.Current.IniModel.GamePath, "GenshinImpact.exe")))
            {
                IGameConvertService gameConvert = new GameConvertService();
                await gameConvert.ConvertGameFileAsync();
                if (ConvertState)
                {
                    await dialogCoordinator.ShowMessageAsync(
                    this, languages.TipsStr,
                    "客户端转换完成，请留意输出日志，记得保存哦~！~\r\n如果转换过程中出现什么问题可以参阅帮助文档网站",
                      MessageDialogStyle.Affirmative,
                     new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });

                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error,
                    "转换失败，请留意输出日志\r\n您也可以参阅帮助文档网站",
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                }
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error,
                    "请先关闭游戏再执行转换操作，如确定游戏已经完全关闭还是弹此提示请重启电脑再试或联系开发者！",
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
            }
            ProgressBar = "Hidden";
            PageUiStatus = "true";
        }
    }
}
