using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.Service.IService;
using System.Windows.Threading;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// SettingsPage的ViewModel 
    /// </summary>
    public class SettingsPageViewModel : ObservableObject
    {

        //构造器
        private IDialogCoordinator dialogCoordinator;
        public SettingsPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;

            _gameConvert = new GameConvertService();
            _userDataService = new UserDataService();
            _registryService = new RegistryService();
            _settingService = new SettingService(this);

            DeleteUserCommand = new RelayCommand(DeleteUser);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ThisPageRemoveCommand = new RelayCommand(ThisPageRemove);
            ChooseGamePathCommand = new RelayCommand(ChooseGamePath);
            ChooseUnlockFpsCommand = new RelayCommand(ChooseUnlockFps);
            GameFileConvertCommand = new AsyncRelayCommand(GameFileConvert);

            _UserLists = UserDataService.ReadUserList();
            _GamePortLists = SettingService.CreateGamePortList();
            _DisplaySizeLists = SettingService.CreateDisplaySizeList();
            _GameWindowModeList = SettingService.CreateGameWindowModeList();
        }


        private IGameConvertService _gameConvert;
        public IGameConvertService GameConvert { get => _gameConvert; }

        private ISettingService _settingService;
        public ISettingService SettingService { get => _settingService; }

        private IUserDataService _userDataService;
        public IUserDataService UserDataService { get => _userDataService; }

        private IRegistryService _registryService;
        public IRegistryService RegistryService { get => _registryService; }

        public IniModel IniModel { get => App.Current.IniModel; }
        public LanguageModel languages { get => App.Current.Language; }



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


        public bool ConvertState { get; set; }
        public string SettingsTitle { get => languages.SettingsTitle; }


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
            set => SettingService.SetDisplaySelectedIndex(value, this);
        }


        //开始转换显示的等待条
        private string _ProgressBar = "Hidden";
        public string ProgressBar
        {
            get => _ProgressBar;
            set => SetProperty(ref _ProgressBar, value);
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


        //用户列表
        private List<UserListModel> _UserLists;
        public List<UserListModel> UserLists
        {
            get => _UserLists;
            set => SetProperty(ref _UserLists, value);
        }

        //游戏客户端列表
        private List<GamePortListModel> _GamePortLists;
        public List<GamePortListModel> GamePortLists { get => _GamePortLists; }

        private List<DisplaySizeListModel> _DisplaySizeLists;
        public List<DisplaySizeListModel> DisplaySizeLists { get => _DisplaySizeLists; }

        //游戏窗口模式列表
        private List<GameWindowModeListModel> _GameWindowModeList;
        public List<GameWindowModeListModel> GameWindowModeList { get => _GameWindowModeList; }


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

        //选择解锁FPS的指令
        public ICommand ChooseUnlockFpsCommand { get; set; }
        private async void ChooseUnlockFps()
        {
            if (isUnFPS)
            {
                if ((await dialogCoordinator.ShowMessageAsync(
                    this, languages.SevereWarning,
                    languages.SevereWarningStr,
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings()
                    {
                        AffirmativeButtonText = languages.Cancel,
                        NegativeButtonText = languages.Determine
                    })) != MessageDialogResult.Affirmative)
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
                if ((await dialogCoordinator.ShowMessageAsync(
                    this, languages.Warning,
                    $"{languages.WarningDAW}[{SwitchUser}] ? !",
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings()
                    {
                        AffirmativeButtonText = languages.Cancel,
                        NegativeButtonText = languages.Determine
                    })) != MessageDialogResult.Affirmative)
                {
                    File.Delete(Path.Combine(@"UserData", SwitchUser));
                    UserLists = UserDataService.ReadUserList();
                    App.Current.NoticeOverAllBase.UserLists = UserLists;
                }
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error, languages.ErrorSA,
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings()
                    { AffirmativeButtonText = languages.Determine });
            }
        }

        //保存设置的命令
        public ICommand SaveSettingsCommand { get; set; }
        private async void SaveSettings()
        {
            string CnGamePath = Path.Combine(GamePath, "Yuanshen.exe");
            string GlobalGamePath = Path.Combine(GamePath, "GenshinImpact.exe");
            if (GamePath != "" && File.Exists(CnGamePath) || File.Exists(GlobalGamePath))
            {
                App.Current.IniModel.GamePath = GamePath;
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error, languages.PathErrorMessageStr,
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings()
                    { AffirmativeButtonText = languages.Determine });
                return;
            }
            if (SwitchUser != null && SwitchUser != "")
            {
                App.Current.IniModel.SwitchUser = SwitchUser;
                App.Current.NoticeOverAllBase.SwitchUser = $"{languages.UserNameLab}：{SwitchUser}";
                App.Current.NoticeOverAllBase.IsSwitchUser = "Visible";
                RegistryService.SetToRegistry(SwitchUser);
            }
            App.Current.IniModel.Width = Width;
            App.Current.IniModel.Height = Height;
            App.Current.IniModel.isUnFPS = isUnFPS;
            GameConvert.SaveGameConfig(this);
            DelaySaveButtonTitle();
            ThisPageRemove();
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
            if (!fileHelper.IsFileOpen(Path.Combine(App.Current.IniModel.GamePath, "Yuanshen.exe")) &&
                !fileHelper.IsFileOpen(Path.Combine(App.Current.IniModel.GamePath, "GenshinImpact.exe")))
            {
                await GameConvert.ConvertGameFileAsync(this);
                if (ConvertState)
                {
                    await dialogCoordinator.ShowMessageAsync(
                    this, languages.TipsStr,
                    "客户端转换完成，请留意输出日志，记得保存哦~！~\r\n如果转换过程中出现什么问题可以参阅帮助文档网站",
                      MessageDialogStyle.Affirmative,
                     new MetroDialogSettings()
                     { AffirmativeButtonText = languages.Determine });

                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error,
                    "转换失败，请留意输出日志\r\n您也可以参阅帮助文档网站",
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings()
                    { AffirmativeButtonText = languages.Determine });
                }
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error,
                    "请先关闭游戏再执行转换操作，如确定游戏已经完全关闭还是弹此提示请重启电脑再试或联系开发者！",
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings()
                    { AffirmativeButtonText = languages.Determine });
            }
            ProgressBar = "Hidden";
            PageUiStatus = "true";
        }
    }
}
