using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.VisualBasic.Devices;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GenShin_Launcher_Plus.ViewModels
{
    internal class SettingsPageViewModel : ObservableObject
    {
        //转换文件列表
        private string[] globalfiles = new string[]
        { "GenshinImpact_Data/app.info",
          "GenshinImpact_Data/globalgamemanagers",
          "GenshinImpact_Data/globalgamemanagers.assets",
          "GenshinImpact_Data/globalgamemanagers.assets.resS",
          "GenshinImpact_Data/upload_crash.exe",
          "GenshinImpact_Data/Managed/Metadata/global-metadata.dat" ,
          "GenshinImpact_Data/Native/Data/Metadata/global-metadata.dat",
          "GenshinImpact_Data/Native/UserAssembly.dll",
          "GenshinImpact_Data/Native/UserAssembly.exp",
          "GenshinImpact_Data/Native/UserAssembly.lib",
          "GenshinImpact_Data/Plugins/cri_mana_vpx.dll",
          "GenshinImpact_Data/Plugins/cri_vip_unity_pc.dll",
          "GenshinImpact_Data/Plugins/cri_ware_unity.dll",
          "GenshinImpact_Data/Plugins/d3dcompiler_43.dll",
          "GenshinImpact_Data/Plugins/d3dcompiler_47.dll",
          "GenshinImpact_Data/Plugins/hdiffz.dll",
          "GenshinImpact_Data/Plugins/hpatchz.dll",
          "GenshinImpact_Data/Plugins/mihoyonet.dll",
          "GenshinImpact_Data/Plugins/Mmoron.dll",
          "GenshinImpact_Data/Plugins/MTBenchmark_Windows.dll",
          "GenshinImpact_Data/Plugins/NamedPipeClient.dll",
          "GenshinImpact_Data/Plugins/UnityNativeChromaSDK.dll",
          "GenshinImpact_Data/Plugins/UnityNativeChromaSDK3.dll",
          "GenshinImpact_Data/Plugins/xlua.dll",
          "GenshinImpact_Data/StreamingAssets/20527480.blk",
          "Audio_Chinese_pkg_version",
          "pkg_version",
          "UnityPlayer.dll",
          "GenshinImpact.exe"
        };

        private string[] cnfiles = new string[]
        { "YuanShen_Data/app.info",
          "YuanShen_Data/globalgamemanagers",
          "YuanShen_Data/globalgamemanagers.assets",
          "YuanShen_Data/globalgamemanagers.assets.resS",
          "YuanShen_Data/upload_crash.exe",
          "YuanShen_Data/Managed/Metadata/global-metadata.dat" ,
          "YuanShen_Data/Native/Data/Metadata/global-metadata.dat",
          "YuanShen_Data/Native/UserAssembly.dll",
          "YuanShen_Data/Native/UserAssembly.exp",
          "YuanShen_Data/Native/UserAssembly.lib",
          "YuanShen_Data/Plugins/cri_mana_vpx.dll",
          "YuanShen_Data/Plugins/cri_vip_unity_pc.dll",
          "YuanShen_Data/Plugins/cri_ware_unity.dll",
          "YuanShen_Data/Plugins/d3dcompiler_43.dll",
          "YuanShen_Data/Plugins/d3dcompiler_47.dll",
          "YuanShen_Data/Plugins/hdiffz.dll",
          "YuanShen_Data/Plugins/hpatchz.dll",
          "YuanShen_Data/Plugins/mihoyonet.dll",
          "YuanShen_Data/Plugins/Mmoron.dll",
          "YuanShen_Data/Plugins/MTBenchmark_Windows.dll",
          "YuanShen_Data/Plugins/NamedPipeClient.dll",
          "YuanShen_Data/Plugins/UnityNativeChromaSDK.dll",
          "YuanShen_Data/Plugins/UnityNativeChromaSDK3.dll",
          "YuanShen_Data/Plugins/xlua.dll",
          "YuanShen_Data/StreamingAssets/20527480.blk",
          "Audio_Chinese_pkg_version",
          "pkg_version",
          "UnityPlayer.dll",
          "YuanShen.exe"
        };

        //构造器
        private IDialogCoordinator dialogCoordinator;
        public SettingsPageViewModel(IDialogCoordinator instance)
        {
            IniModel = new SettingsIniModel();
            //
            languages = MainBase.lang;
            SettingsTitle = languages.SettingsTitle;
            GameSwitchLog = languages.GameSwitchLogStr;
            TimeStatus = languages.TimeStatusDefault;
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
            GameFileConvertCommand = new RelayCommand(GameFileConvert);
            Auto21x9Command = new RelayCommand(Auto21x9);
            ThisPageRemoveCommand = new RelayCommand(ThisPageRemove);
        }
        public LanguagesModel languages { get; set; }

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
        private SettingsIniModel _IniModel;
        public SettingsIniModel IniModel
        {
            get => _IniModel;
            set => SetProperty(ref _IniModel, value);

        }

        //转换时的日志列表
        private string _GameSwitchLog;
        public string GameSwitchLog
        {
            get => _GameSwitchLog;
            set => SetProperty(ref _GameSwitchLog, value);
        }

        //转换时的控件状态
        private string _PageUiStatus = "true";
        public string PageUiStatus
        {
            get => _PageUiStatus;
            set => SetProperty(ref _PageUiStatus, value);
        }

        //转换状态
        private string _TimeStatus;
        public string TimeStatus
        {
            get => _TimeStatus;
            set => SetProperty(ref _TimeStatus, value);
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
                    IniModel.isUnFPS = isUnFPS;
                }
                else
                {
                    isUnFPS = false;
                    IniModel.isUnFPS = isUnFPS;
                }
            }
        }

        //删除账号的命令
        public ICommand DeleteUserCommand { get; set; }
        private async void DeleteUser()
        {

            if (IniModel.SwitchUser != "" && IniModel.SwitchUser != null)
            {
                if ((await dialogCoordinator.ShowMessageAsync(this, languages.Warning, $"{languages.WarningDAW}[{IniModel.SwitchUser}] ? !", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = languages.Cancel, NegativeButtonText = languages.Determine })) != MessageDialogResult.Affirmative)
                {
                    File.Delete(Path.Combine(@"UserData", IniModel.SwitchUser));
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
            if (IniModel.SwitchUser != null && IniModel.SwitchUser != "")
            {
                IniControl.SwitchUser = IniModel.SwitchUser;
                MainBase.noab.SwitchUser = $"{languages.UserNameLab}：{IniModel.SwitchUser}";
                MainBase.noab.IsSwitchUser = "Visible";
                RegistryControl registryControl = new();
                registryControl.SetToRegedit(IniModel.SwitchUser);
            }
            if (IniModel.GamePath != "" && File.Exists(Path.Combine(IniModel.GamePath, "Yuanshen.exe")) || File.Exists(Path.Combine(IniModel.GamePath, "GenshinImpact.exe")))
            {
                IniControl.GamePath = IniModel.GamePath;
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PathErrorMessageStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                return;
            }
            IniControl.Width = Width;
            IniControl.Height = Height;
            IniControl.isUnFPS =isUnFPS;
            IniControl.MaxFps = IniModel.MaxFps;
            IniControl.isPopup = IniModel.isPopup;
            IniControl.isMainGridHide = IniModel.isMainGridHide;
            IniControl.isWebBg = IniModel.isWebBg;
            IniControl.FullSize = IniModel.FullSize;
            IniControl.UserXunkongWallpaper = IniModel.UseXunkongWallpaper;
            if (File.Exists(Path.Combine(IniControl.GamePath, "config.ini")) == true)
            {
                switch (IniModel.isMihoyo)
                {
                    case 0:
                        IniControl.Cps = "pcadbdpz";
                        IniControl.Channel = 1;
                        IniControl.Sub_channel = 1;
                        if (File.Exists(Path.Combine(IniModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                            File.Delete(Path.Combine(IniModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                        MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypePStr}";
                        break;
                    case 1:
                        IniControl.Cps = "bilibili";
                        IniControl.Channel = 14;
                        IniControl.Sub_channel = 0;
                        if (!File.Exists(Path.Combine(IniModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                        {
                            FilesControl utils = new();
                            try
                            {
                                utils.FileWriter("StaticRes/mihoyosdk.dll", Path.Combine(IniModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeBStr}";
                        break;
                    case 2:
                        IniControl.Cps = "mihoyo";
                        IniControl.Channel = 1;
                        IniControl.Sub_channel = 0;
                        if (File.Exists(Path.Combine(IniModel.GamePath, "GenshinImpact_Data/Plugins/PCGameSDK.dll")))
                            File.Delete(Path.Combine(IniModel.GamePath, "GenshinImpact_Data/Plugins/PCGameSDK.dll"));
                        MainBase.noab.SwitchPort = $"{languages.GameClientStr} : {languages.GameClientTypeMStr}";
                        break;
                    default:
                        break;
                }
            }
            DelaySaveButtonTitle();
            MainBase.noab.MainPagesIndex = 0;
        }

        //读取判断游戏客户端信息
        private void ReadGameConfig()
        {
            if (File.Exists(Path.Combine(IniControl.GamePath, "config.ini")))
            {
                if (IniControl.Cps == "pcadbdpz")
                { IniModel.isMihoyo = 0; }
                else if (IniControl.Cps == "bilibili")
                { IniModel.isMihoyo = 1; }
                else if (IniControl.Cps == "mihoyo")
                { IniModel.isMihoyo = 2; }
                else
                { IniModel.isMihoyo = 3; }
            }
            else
            { IniModel.isMihoyo = 3; }
        }

        //被创建时从Setting.ini文件读取给IniModel对象赋值
        private void SettingsPageCreated()
        {
            IniModel.GamePath = IniControl.GamePath;
            IniModel.Width = IniControl.Width;
            IniModel.Height = IniControl.Height;
            IniModel.isUnFPS = IniControl.isUnFPS;
            IniModel.MaxFps = IniControl.MaxFps;
            IniModel.GamePath = IniControl.GamePath;
            IniModel.isPopup = IniControl.isPopup;
            IniModel.isMainGridHide = IniControl.isMainGridHide;
            IniModel.isWebBg = IniControl.isWebBg;
            IniModel.FullSize = IniControl.FullSize;
            Width = IniModel.Width;
            Height = IniModel.Height;
            isUnFPS = IniModel.isUnFPS;
            IniModel.UseXunkongWallpaper = IniControl.UserXunkongWallpaper;
            ReadGameConfig();
        }

        //自动取比例
        public ICommand Auto21x9Command { get; set; }
        private async void Auto21x9()
        {
            if (Height == "" && Width != "")
            {
                int x = Convert.ToInt32(Width);
                int y = x * 9 / 21;
                Width = Convert.ToString(x);
                Height = Convert.ToString(y);
            }
            else if (Width == "" && Height != "")
            {
                int y = Convert.ToInt32(Height);
                int x = y * 21 / 9;
                Width = Convert.ToString(x);
                Height = Convert.ToString(y);
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.ErrorEYJ, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
            }
        }


        //关闭设置页面
        public ICommand ThisPageRemoveCommand { get; set; }
        private void ThisPageRemove()
        {
            MainBase.noab.MainPagesIndex = 0;
        }

        //转换国际服及转换国服绑定命令
        public ICommand GameFileConvertCommand { get; set; }
        private void GameFileConvert()
        {
            if (!CheckControl.IsFileOpen(Path.Combine(IniControl.GamePath, "Yuanshen.exe")) && !CheckControl.IsFileOpen(Path.Combine(IniControl.GamePath, "GenshinImpact.exe")))
            {

                Task start = new(async () =>
                {
                    if ((await dialogCoordinator.ShowMessageAsync(this, $"{languages.Warning} ! !", languages.WarningCCStr, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = languages.Cancel, NegativeButtonText = languages.SwitchBtn })) != MessageDialogResult.Affirmative)
                    {
                        PageUiStatus = "false";
                        ProgressBar = "Visible";
                        //判断客户端
                        string port = JudgeGamePort();
                        //判断Pkg是否正常
                        if (port == "YuanShen")
                        {
                            if (!CheckFileIntegrity(IniControl.GamePath, cnfiles, 1, ".bak"))
                            {
                                //没备份文件
                                if (Directory.Exists(@"GlobalFile"))
                                {
                                    if (JudgePkgVer("GlobalFile"))
                                    {
                                        if (CheckFileIntegrity(@"GlobalFile", globalfiles, 0))
                                        {
                                            await GlobalMoveFile();
                                        }
                                    }
                                    else
                                    {
                                        DirectoryInfo di = new(@"GlobalFile");
                                        di.Delete(true);
                                    }
                                }
                                else if (File.Exists(@"GlobalFile.pkg"))
                                {
                                    TimeStatus = languages.TimeStatusUning;
                                    //解压Pkg
                                    if (FilesControl.UnZip("GlobalFile.pkg", @""))
                                    {
                                        if (JudgePkgVer("GlobalFile"))
                                        {
                                            await GlobalMoveFile();
                                        }
                                        else
                                        {
                                            DirectoryInfo di = new(@"GlobalFile");
                                            di.Delete(true);
                                            TimeStatus = languages.TimeStatusUpdate;
                                        }
                                    }
                                    //解压失败
                                    else
                                    {
                                        TimeStatus = languages.TimeStatusUnErr;
                                        GameSwitchLog += languages.ErrorGPkgNF;
                                        await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PkgNoUnError, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                                    }
                                }
                                else
                                {
                                    TimeStatus = languages.TimeStatusCheck;
                                    GameSwitchLog += languages.ErrorGPkgNF;
                                    await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PkgNoUnError, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                                }
                            }
                            else
                            {
                                await ReGlobalGame();
                            }
                        }
                        else
                        {
                            if (!CheckFileIntegrity(IniControl.GamePath, globalfiles, 1, ".bak"))
                            {
                                //没备份文件
                                if (Directory.Exists(@"CnFile"))
                                {
                                    if (JudgePkgVer("CnFile"))
                                    {
                                        if (CheckFileIntegrity(@"CnFile", cnfiles, 0))
                                        {
                                            await CnMoveFile();
                                        }
                                    }
                                    else
                                    {
                                        DirectoryInfo di = new(@"CnFile");
                                        di.Delete(true);
                                    }
                                }
                                else if (File.Exists(@"CnFile.pkg"))
                                {
                                    TimeStatus = languages.TimeStatusUning;
                                    //解压Pkg
                                    if (FilesControl.UnZip("CnFile.pkg", @""))
                                    {
                                        if (JudgePkgVer("CnFile"))
                                        {
                                            await CnMoveFile();
                                        }
                                        else
                                        {
                                            DirectoryInfo di = new(@"CnFile");
                                            di.Delete(true);
                                            TimeStatus = languages.TimeStatusUpdate;
                                        }
                                    }
                                    //解压失败
                                    else
                                    {
                                        TimeStatus = languages.TimeStatusUnErr;
                                        GameSwitchLog += languages.ErrorCPkgNF;
                                        await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PkgNoUnError, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                                    }
                                }
                                else
                                {
                                    TimeStatus = languages.TimeStatusCheck;
                                    GameSwitchLog += languages.ErrorCPkgNF;
                                    await dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PkgNoUnError, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                                }
                            }
                            else
                            {
                                await ReCnGame();
                            }
                        }
                        ProgressBar = "Hidden";
                        PageUiStatus = "true";
                        SaveSettings();
                    }
                });
                start.Start();
            }
            else
            {
                dialogCoordinator.ShowMessageAsync(this, languages.Error, "请先关闭游戏再执行转换操作，如确定游戏已经完全关闭还是弹此提示请重启电脑再试或联系开发者！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
            }
        }

        //转换国际服及转换国服核心逻辑-判断客户端
        private string JudgeGamePort()
        {
            if (File.Exists(Path.Combine(IniControl.GamePath, "YuanShen.exe")))
            {
                return "YuanShen";
            }
            else if (File.Exists(Path.Combine(IniControl.GamePath, "GenshinImpact.exe")))
            {
                return "GenshinImpact";
            }
            else
            {
                return null;
            }
        }

        //转换国际服及转换国服核心逻辑-判断PKG文件版本
        private bool JudgePkgVer(string GamePort)
        {
            string pkgfile = MainBase.update.PkgVersion;
            if (!File.Exists($"{GamePort}/{pkgfile}"))
            {
                dialogCoordinator.ShowMessageAsync(this, languages.Warning, $"{languages.NewPkgVer} : [{ pkgfile }]\r\n", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                ProcessStartInfo info = new()
                {
                    FileName = "https://pan.baidu.com/s/1-5zQoVfE7ImdXrn8OInKqg",
                    UseShellExecute = true,
                };
                Process.Start(info);
                return false;
            }
            else
            {
                return true;
            }
        }

        //遍历判断文件是否存在
        private bool CheckFileIntegrity(string dirpath, string[] filepath, int len, string postfix = "")
        {
            bool notError = true;
            for (int i = 0; i < filepath.Length - len; i++)
            {
                if (File.Exists(Path.Combine(dirpath, filepath[i] + postfix)) == false)
                {
                    GameSwitchLog += $"{filepath[i]} {postfix} {languages.ErrorFileNF}\r\n";
                    notError = false;
                    break;
                }
                GameSwitchLog += $"{filepath[i]} {postfix} {languages.FileExist}\r\n";
            }
            return notError;
        }

        //国内转国际
        private async Task GlobalMoveFile()
        {
            Computer redir = new();
            TimeStatus = languages.TimeStatusBaking;
            for (int a = 0; a < cnfiles.Length; a++)
            {
                String newFileName = Path.GetFileNameWithoutExtension(Path.Combine(IniControl.GamePath, cnfiles[a])) + Path.GetExtension(Path.Combine(IniControl.GamePath, cnfiles[a]));
                if (File.Exists(Path.Combine(IniControl.GamePath, cnfiles[a])) == true)
                {
                    try
                    {
                        redir.FileSystem.RenameFile(Path.Combine(IniControl.GamePath, cnfiles[a]), newFileName + ".bak");
                    }
                    catch (Exception ex)
                    {
                        GameSwitchLog += $"{newFileName} {languages.ErrorBakF}";
                        GameSwitchLog += $"{ex.Message}\r\n\r\n";
                    }
                    GameSwitchLog += $"{newFileName} {languages.BakSuccess}\r\n";
                }
                else
                {
                    GameSwitchLog += $"{newFileName} {languages.BakFileNfSk}\r\n";
                }
            }
            TimeStatus = languages.TimeStatusReping;
            redir.FileSystem.RenameDirectory(Path.Combine(IniControl.GamePath, "YuanShen_Data"), "GenshinImpact_Data");
            for (int i = 0; i < globalfiles.Length; i++)
            {
                File.Copy(Path.Combine(@"GlobalFile", globalfiles[i]), Path.Combine(IniControl.GamePath, globalfiles[i]), true);
                GameSwitchLog += $"{globalfiles[i]} {languages.RepSuccess}\r\n";
            };
            IniModel.isMihoyo = 2;
            TimeStatus = languages.TimeStatusDefault;
            await dialogCoordinator.ShowMessageAsync(this, languages.TipsStr, languages.SwitchSucessStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
        }

        //国际转国内
        private async Task CnMoveFile()
        {
            Computer redir = new();
            TimeStatus = languages.TimeStatusBaking;
            for (int a = 0; a < globalfiles.Length; a++)
            {
                String newFileName = Path.GetFileNameWithoutExtension(Path.Combine(IniControl.GamePath, globalfiles[a])) + Path.GetExtension(Path.Combine(IniControl.GamePath, globalfiles[a]));
                if (File.Exists(Path.Combine(IniControl.GamePath, globalfiles[a])) == true)
                {
                    try
                    {
                        redir.FileSystem.RenameFile(Path.Combine(IniControl.GamePath, globalfiles[a]), newFileName + ".bak");
                    }
                    catch (Exception ex)
                    {
                        GameSwitchLog += $"{newFileName} {languages.ErrorBakF}";
                        GameSwitchLog += $"{ex.Message}\r\n\r\n";
                    }
                    GameSwitchLog += $"{newFileName} {languages.BakSuccess}\r\n";
                }
                else
                {
                    GameSwitchLog += $"{newFileName} {languages.BakFileNfSk}\r\n";
                }
            }
            TimeStatus = languages.TimeStatusReping;
            redir.FileSystem.RenameDirectory(Path.Combine(IniControl.GamePath, "GenshinImpact_Data"), "YuanShen_Data");
            for (int i = 0; i < cnfiles.Length; i++)
            {
                File.Copy(Path.Combine(@"CnFile", cnfiles[i]), Path.Combine(IniControl.GamePath, cnfiles[i]), true);
                GameSwitchLog += $"{cnfiles[i]} {languages.RepSuccess}\r\n";
            };
            IniModel.isMihoyo = 0;
            TimeStatus = languages.TimeStatusDefault;
            await dialogCoordinator.ShowMessageAsync(this, languages.TipsStr, languages.SwitchSucessStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
        }
        //还原
        private async Task ReCnGame()
        {
            Computer redir = new();
            TimeStatus = languages.TimeStatusCleaning;
            for (int i = 0; i < globalfiles.Length; i++)
            {
                if (File.Exists(Path.Combine(IniControl.GamePath, globalfiles[i])) == true)
                {
                    File.Delete(Path.Combine(IniControl.GamePath, globalfiles[i]));
                    GameSwitchLog += $"{globalfiles[i]} {languages.CleanedStr}\r\n";
                }
                else
                {
                    GameSwitchLog += $"{globalfiles[i]} {languages.CleanSkipStr}\r\n";
                }
            }
            TimeStatus = languages.TimeStatusRecover;
            redir.FileSystem.RenameDirectory(Path.Combine(IniControl.GamePath, "GenshinImpact_Data"), "YuanShen_Data");
            int whole = 0, success = 0;
            for (int a = 0; a < cnfiles.Length; a++)
            {
                string newFileName = Path.GetFileNameWithoutExtension(cnfiles[a]) + Path.GetExtension(cnfiles[a]);
                if (File.Exists(Path.Combine(IniControl.GamePath, cnfiles[a] + ".bak")) == true)
                {
                    redir.FileSystem.RenameFile(Path.Combine(IniControl.GamePath, cnfiles[a] + ".bak"), newFileName);
                    GameSwitchLog += $"{cnfiles[a]} {languages.RestoreSucess}\r\n";
                    success++;
                }
                else
                {

                    GameSwitchLog += $"{cnfiles[a]} {languages.RestoreSkipStr}\r\n";
                    whole++;
                }
            }
            TimeStatus = languages.TimeStatusDefault;
            IniModel.isMihoyo = 0;
            await dialogCoordinator.ShowMessageAsync(this, languages.TipsStr, $"{languages.RestoreOverTipsStr} , {languages.RestoreNum} : {success } ,{languages.RestoreErrNum} : {whole} , {languages.RestoreEndStr}", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
        }

        private async Task ReGlobalGame()
        {
            Computer redir = new();
            TimeStatus = languages.TimeStatusCleaning;
            for (int i = 0; i < cnfiles.Length; i++)
            {
                if (File.Exists(Path.Combine(IniControl.GamePath, cnfiles[i])) == true)
                {
                    File.Delete(Path.Combine(IniControl.GamePath, cnfiles[i]));
                    GameSwitchLog += $"{cnfiles[i]} {languages.CleanedStr}\r\n";
                }
                else
                {
                    GameSwitchLog += $"{cnfiles[i]} {languages.CleanSkipStr}\r\n";
                }
            }
            TimeStatus = languages.TimeStatusRecover;
            redir.FileSystem.RenameDirectory(Path.Combine(IniControl.GamePath, "YuanShen_Data"), "GenshinImpact_Data");
            int whole = 0, success = 0;
            for (int a = 0; a < globalfiles.Length; a++)
            {
                string newFileName = Path.GetFileNameWithoutExtension(globalfiles[a]) + Path.GetExtension(globalfiles[a]);
                if (File.Exists(Path.Combine(IniControl.GamePath, globalfiles[a] + ".bak")) == true)
                {
                    redir.FileSystem.RenameFile(Path.Combine(IniControl.GamePath, globalfiles[a] + ".bak"), newFileName);
                    GameSwitchLog += $"{globalfiles[a]} {languages.RestoreSucess}\r\n";
                    success++;
                }
                else
                {

                    GameSwitchLog += $"{globalfiles[a]} {languages.RestoreSkipStr}\r\n";
                    whole++;
                }
            }
            TimeStatus = languages.TimeStatusDefault;
            IniModel.isMihoyo = 2;
            await dialogCoordinator.ShowMessageAsync(this, languages.TipsStr, $"{languages.RestoreOverTipsStr} , {languages.RestoreNum} : {success } ,{languages.RestoreErrNum} : {whole}", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
        }
    }
}
