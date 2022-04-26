using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GenShin_Launcher_Plus.Service
{

    /// <summary>
    /// 连接SettingsPage与HomePage的服务
    /// 隶属于ViewModel
    /// </summary>

    public class NoticeOverAllBase : ObservableObject
    {
        //贯通到主页面的索引
        private int _MainPagesIndex;
        public int MainPagesIndex
        {
            get => _MainPagesIndex;
            set => SetProperty(ref _MainPagesIndex, value);
        }
        //Home页面显示label

        private string _SwitchUser;
        public string SwitchUser
        {
            get => _SwitchUser;
            set => SetProperty(ref _SwitchUser, value);
        }

        private string _SwitchPort;
        public string SwitchPort
        {
            get
            {
                string gameClientType = App.Current.DataModel.Cps switch
                {
                    "pcadbdpz" => App.Current.Language.GameClientTypePStr,
                    "bilibili" => App.Current.Language.GameClientTypeBStr,
                    "mihoyo" => App.Current.Language.GameClientTypeMStr,
                    _ => App.Current.Language.GameClientTypeNullStr,
                };
                return $"{App.Current.Language.GameClientStr} : {gameClientType} ";
            }
            set => SetProperty(ref _SwitchPort, value);
        }

        private int _GamePortListIndex;
        public int GamePortListIndex
        {
            get
            {
                int index = App.Current.DataModel.Cps switch
                {
                    "pcadbdpz" => 0,
                    "bilibili" => 1,
                    "mihoyo" => -1,
                    _ => -1,
                };
                string gameClientType = index switch
                {
                    0 => App.Current.Language.GameClientTypePStr,
                    1 => App.Current.Language.GameClientTypeBStr,
                    -1 => App.Current.Language.GameClientTypeMStr,
                    _ => App.Current.Language.GameClientTypeNullStr,
                };
                SwitchPort = $"{App.Current.Language.GameClientStr} : {gameClientType} ";
                return index;
            }
            set
            {
                SetProperty(ref _GamePortListIndex, value);
                if (App.Current.DataModel.Cps != "mihoyo")
                {
                    switch (value)
                    {
                        case 0:
                            App.Current.DataModel.Cps = "pcadbdpz";
                            App.Current.DataModel.Channel = 1;
                            App.Current.DataModel.Sub_channel = 1;
                            if (File.Exists(Path.Combine(App.Current.DataModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                                File.Delete(Path.Combine(App.Current.DataModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                            App.Current.NoticeOverAllBase.SwitchPort = $"{App.Current.Language.GameClientStr} : {App.Current.Language.GameClientTypePStr}";
                            break;
                        case 1:
                            App.Current.DataModel.Cps = "bilibili";
                            App.Current.DataModel.Channel = 14;
                            App.Current.DataModel.Sub_channel = 0;
                            if (!File.Exists(Path.Combine(App.Current.DataModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll")))
                            {
                                try
                                {
                                    FileHelper.ExtractEmbededAppResource("StaticRes/mihoyosdk.dll", 
                                        Path.Combine(App.Current.DataModel.GamePath, "YuanShen_Data/Plugins/PCGameSDK.dll"));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            App.Current.NoticeOverAllBase.SwitchPort = $"{App.Current.Language.GameClientStr} : {App.Current.Language.GameClientTypeBStr}";
                            break;
                        default:
                            break;
                    }
                    App.Current.DataModel.SaveDataToFile();
                }
            }
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
                    App.Current.NoticeOverAllBase.SwitchUser = $"{App.Current.Language.UserNameLab} : {SwitchUserValue}";
                    //更改注册表账号状态
                    App.Current.DataModel.SwitchUser = SwitchUserValue;
                    App.Current.DataModel.SaveDataToFile();
                    RegistryService registryControl = new();
                    registryControl.SetToRegistry(SwitchUserValue);
                }
            }
        }

        //游戏端口列表
        private List<GamePortListModel> _GamePortLists;
        public List<GamePortListModel> GamePortLists
        {
            get => _GamePortLists;
            set => SetProperty(ref _GamePortLists, value);
        }

        //选择游戏端口Combobox控件状态
        private string _IsGamePortLists;
        public string IsGamePortLists
        {
            get
            {
                return App.Current.DataModel.Cps switch
                {
                    "mihoyo" => "Hidden",
                    _ => "Visible",
                };
            }
            set => SetProperty(ref _IsGamePortLists, value);
        }

        //选择账户Combobox控件状态
        private string _IsSwitchUser;
        public string IsSwitchUser
        {
            get => _IsSwitchUser;
            set => SetProperty(ref _IsSwitchUser, value);
        }

        //用户列表
        public List<UserListModel> _UserLists;
        public List<UserListModel> UserLists
        {
            get => _UserLists;
            set => SetProperty(ref _UserLists, value);
        }
    }

}
