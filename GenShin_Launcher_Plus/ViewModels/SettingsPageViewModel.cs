using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.Service.IService;

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

            SwitchAccountCommand = new RelayCommand(SwitchAccount);
            SwitchGameSettingsCommand = new RelayCommand(SwitchGameSettings);
            SwitchConvertClientCommand = new RelayCommand(SwitchConvertClient);
            SwitchProgarmSettingCommand = new RelayCommand(SwitchProgarmSetting);

            CheckUpdateCommand = new RelayCommand(CheckUpdate);
            SaveDisPlaySizeCommand = new RelayCommand(SaveDisPlaySize);
            SetMainBackgroundCommand = new RelayCommand(SetMainBackground);
            SwitchLanguagePageCommand = new RelayCommand(SwitchLanguagePage);
            OpenPkgDownloadUrlCommand = new RelayCommand(OpenPkgDownloadUrl);
            OpenApplicationFolderCommand = new RelayCommand(OpenApplicationFolder);

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
        public string Width 
        { 
            get => _Width;
            set
            {
                App.Current.DataModel.Width = value;
                SetProperty(ref _Width, value);
            }
        }

        private string _Height;
        public string Height
        { 
            get => _Height;
            set 
            { 
                App.Current.DataModel.Height = value;
                SetProperty(ref _Height, value);
            }
        }

        private bool _IsUnFPS;
        public bool IsUnFPS
        {
            get => _IsUnFPS;
            set
            { 
                App.Current.DataModel.IsUnFPS = value;
                SetProperty(ref _IsUnFPS, value); 
            }
        }

        private string _GamePath;
        public string GamePath 
        { 
            get => _GamePath;
            set 
            { 
                App.Current.DataModel.GamePath = value;
                SetProperty(ref _GamePath, value); 
            }
        }

        private string _SwitchUser;
        public string SwitchUser 
        { 
            get => _SwitchUser;
            set
            {
                App.Current.DataModel.SwitchUser = value;
                SetProperty(ref _SwitchUser, value);
            }
        }

        private int _IsMihoyo;
        public int IsMihoyo 
        { 
            get => _IsMihoyo;
            set
            {
                App.Current.DataModel.IsMihoyo = value;
                SetProperty(ref _IsMihoyo, value);
            }
        }

        private bool _IsPopup;
        public bool IsPopup
        {
            get => _IsPopup;
            set
            {
                App.Current.DataModel.IsPopup = value;
                SetProperty(ref _IsPopup, value);
            }
        }

        private ushort _FullSize;
        public ushort FullSize
        {
            get => _FullSize;
            set
            {
                App.Current.DataModel.FullSize = value;
                SetProperty(ref _FullSize, value);
            }
        }

        private string _MaxFps;
        public string MaxFps
        {
            get => _MaxFps;
            set
            {
                App.Current.DataModel.MaxFps = value;
                SetProperty(ref _MaxFps, value);
            }
        }

        private bool _IsWebBg;
        public bool IsWebBg
        {
            get => _IsWebBg;
            set
            {
                App.Current.DataModel.IsWebBg = value;
                SetProperty(ref _IsWebBg, value);
            }
        }

        private bool _UseXunkongWallpaper;
        public bool UseXunkongWallpaper
        {
            get => _UseXunkongWallpaper;
            set
            {
                App.Current.DataModel.UseXunkongWallpaper = value;
                SetProperty(ref _UseXunkongWallpaper, value);
            }
        }

        private bool _IsRunThenClose;
        public bool IsRunThenClose
        {
            get => _IsRunThenClose;
            set
            {
                App.Current.DataModel.IsRunThenClose = value;
                SetProperty(ref _IsRunThenClose, value);
            }
        }

        private bool _IsCloseUpdate;
        public bool IsCloseUpdate
        {
            get => _IsCloseUpdate;
            set
            {
                App.Current.DataModel.IsCloseUpdate = value;
                SetProperty(ref _IsCloseUpdate, value);
            }
        }

        private int _FlipViewSelectedIndex;
        public int FlipViewSelectedIndex 
        { 
            get => _FlipViewSelectedIndex;
            set
            {
                SetProperty(ref _FlipViewSelectedIndex, value);
            }
        }


        //选中分辨率进行调整
        public string SwitchSize { set => SettingService.SetDisplaySelectedValue(value, this); }


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

        //预设分辨率列表
        private List<DisplaySizeListModel> _DisplaySizeLists;
        public List<DisplaySizeListModel> DisplaySizeLists
        {
            get
            {
                if (_DisplaySizeLists == null)
                { 
                    return new List<DisplaySizeListModel>()
                    {
                        new DisplaySizeListModel
                        { 
                            SizeName = "没有已保存的预设选项",
                            IsNull = true,
                        } 
                    }; 
                }
                else { return _DisplaySizeLists; }
            }
            set => SetProperty(ref _DisplaySizeLists, value);
        }

        //游戏窗口模式列表
        private List<GameWindowModeListModel> _GameWindowModeList;
        public List<GameWindowModeListModel> GameWindowModeList { get => _GameWindowModeList; }


        /// <summary>
        /// 切换FlipView的SelectedIndex方法集
        /// </summary>
        /// 
        public ICommand SwitchGameSettingsCommand { get; set; }
        private void SwitchGameSettings()
        {
            FlipViewSelectedIndex = 0;
        }

        public ICommand SwitchConvertClientCommand { get; set; }
        private void SwitchConvertClient()
        {
            FlipViewSelectedIndex = 1;
        }

        public ICommand SwitchAccountCommand { get; set; }
        private void SwitchAccount()
        {
            FlipViewSelectedIndex = 2;
        }

        public ICommand SwitchProgarmSettingCommand { get; set; }
        private void SwitchProgarmSetting()
        {
            FlipViewSelectedIndex = 3;
        }

        //保存设置的分辨率到列表
        public ICommand SaveDisPlaySizeCommand { get; set; }
        private async void SaveDisPlaySize()
        {
            if (FileHelper.IsInt(Width) && FileHelper.IsInt(Height))
            {
                SettingService.SaveDisplaySizeToList(this, Width, Height);
                await dialogCoordinator.ShowMessageAsync(
                   this, languages.TipsStr,
                   "添加自定义分辨率到预设列表成功！",
                   MessageDialogStyle.Affirmative,
                   new MetroDialogSettings()
                   { AffirmativeButtonText = languages.Determine });
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(
                   this, languages.Error,
                   "请输入正确的分辨率宽高数值！",
                   MessageDialogStyle.Affirmative,
                   new MetroDialogSettings()
                   { AffirmativeButtonText = languages.Determine });
            }
        }

        //选择游戏路径的命令
        public ICommand ChooseGamePathCommand { get; set; }
        private void ChooseGamePath()
        {
            CommonOpenFileDialog dialog = new(languages.GameDirMsg);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                GamePath = dialog.FileName;
            }
        }

        //跳转到下载Pkg文件的链接命令
        public ICommand OpenPkgDownloadUrlCommand { get; set; }
        private void OpenPkgDownloadUrl()
        {
            FileHelper.OpenUrl("https://resource.snapgenshin.com/Plugins/Genshin.Launcher.Plus.SE.Plugin/");
        }

        //设置页面手动检查更新命令
        public ICommand CheckUpdateCommand { get; set; }
        private async void CheckUpdate()
        {
            App.Current.IsLoading = false;
            App.Current.DataModel.IsCloseUpdate = false;
            new UpdateService().CheckUpdate(App.Current.ThisMainWindow);
        }

        //跳转到设置程序语言界面
        public ICommand SwitchLanguagePageCommand { get; set; }
        private void SwitchLanguagePage()
        {
            App.Current.ThisMainWindow.SwitchLanguages.Children.Clear();
            App.Current.ThisMainWindow.SwitchLanguages.Children.Add(new Views.LanguagesPage());
            App.Current.ThisMainWindow.MainFlipView.SelectedIndex = 3;
        }

        //打开本程序目录的命令
        public ICommand OpenApplicationFolderCommand { get; set; }
        private void OpenApplicationFolder()
        {
            FileHelper.OpenUrl(Environment.CurrentDirectory);
        }

        //设置背景图片的命令
        public ICommand SetMainBackgroundCommand { get; set; }
        private async void SetMainBackground()
        {
            CommonOpenFileDialog dialog = new();
            dialog.IsFolderPicker = false;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string extensionName = Path.GetExtension(dialog.FileName);
                if (extensionName.ToLower() == ".png" ||
                    extensionName.ToLower() == ".jpg" ||
                    extensionName.ToLower() == ".webp")
                {
                    App.Current.DataModel.BackgroundPath = dialog.FileName;
                    IsWebBg = true;
                    UseXunkongWallpaper = false;
                    _ = new MainService(App.Current.ThisMainWindow, App.Current.ThisMainWindow.ViewModel);
                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(
                        this, languages.Error,
                        "仅支持png、jpg或webp文件，请选择支持的格式",
                        MessageDialogStyle.Affirmative,
                        new MetroDialogSettings()
                        { AffirmativeButtonText = languages.Determine });
                }
            }
        }

        //选择解锁FPS的指令
        public ICommand ChooseUnlockFpsCommand { get; set; }
        private async void ChooseUnlockFps()
        {
            if (IsUnFPS)
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
                    IsUnFPS = true;
                }
                else
                {
                    IsUnFPS = false;
                }
                App.Current.DataModel.IsUnFPS = IsUnFPS;
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
                App.Current.DataModel.GamePath = GamePath;
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
                App.Current.NoticeOverAllBase.SwitchUser = $"{languages.UserNameLab}：{SwitchUser}";
                App.Current.NoticeOverAllBase.IsSwitchUser = "Visible";
                RegistryService.SetToRegistry(SwitchUser);
            }
            GameConvert.SaveGameConfig(this);
            DelaySaveButtonTitle();
            App.Current.DataModel.SaveDataToFile();
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
            if (!fileHelper.IsFileOpen(Path.Combine(App.Current.DataModel.GamePath, "Yuanshen.exe")) &&
                !fileHelper.IsFileOpen(Path.Combine(App.Current.DataModel.GamePath, "GenshinImpact.exe")))
            {
                await GameConvert.ConvertGameFileAsync(this);
                if (ConvertState)
                {
                    await dialogCoordinator.ShowMessageAsync(
                    this, languages.TipsStr,
                    App.Current.Language.SwitchSucessStr,
                      MessageDialogStyle.Affirmative,
                     new MetroDialogSettings()
                     { AffirmativeButtonText = languages.Determine });

                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error,
                    App.Current.Language.ConvertError,
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings()
                    { AffirmativeButtonText = languages.Determine });
                }
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error,
                    App.Current.Language.CloseGameWaring,
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings()
                    { AffirmativeButtonText = languages.Determine });
            }
            ProgressBar = "Hidden";
            PageUiStatus = "true";
        }
    }
}
