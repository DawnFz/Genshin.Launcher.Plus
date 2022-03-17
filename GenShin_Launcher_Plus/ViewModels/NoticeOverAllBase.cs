using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GenShin_Launcher_Plus.ViewModels
{

    /// <summary>
    /// 这个类用于连接SettingsPageViewModel对
    /// HomePageViewModel和HomePage的绑定更新
    /// 目前已经有更好的解决办法，这个类勿动，作者会
    /// 以新的实现方式对SettingsPageViewModel与
    /// HomePageViewModel和HomePage的绑定更新
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
                string gameClientType = MainBase.IniModel.Cps switch
                {
                    "pcadbdpz" => MainBase.lang.GameClientTypePStr,
                    "bilibili" => MainBase.lang.GameClientTypeBStr,
                    "mihoyo" => MainBase.lang.GameClientTypeMStr,
                    _ => MainBase.lang.GameClientTypeNullStr,
                };
                return $"{MainBase.lang.GameClientStr} : {gameClientType} ";
            }
            set => SetProperty(ref _SwitchPort, value);
        }

        private int _GamePortListIndex;
        public int GamePortListIndex
        {
            get
            {
                int index = MainBase.IniModel.Cps switch
                {
                    "pcadbdpz" => 0,
                    "bilibili" => 1,
                    "mihoyo" => -1,
                    _ => -1,
                };
                string gameClientType = index switch
                {
                    0 => MainBase.lang.GameClientTypePStr,
                    1 => MainBase.lang.GameClientTypeBStr,
                    -1 => MainBase.lang.GameClientTypeMStr,
                    _ => MainBase.lang.GameClientTypeNullStr,
                };
                SwitchPort = $"{MainBase.lang.GameClientStr} : {gameClientType} ";
                return index;
            }
            set
            {
                SetProperty(ref _GamePortListIndex, value);
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
                            MainBase.noab.SwitchPort = $"{MainBase.lang.GameClientStr} : {MainBase.lang.GameClientTypePStr}";
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
                            MainBase.noab.SwitchPort = $"{MainBase.lang.GameClientStr} : {MainBase.lang.GameClientTypeBStr}";
                            break;
                        default:
                            break;
                    }
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
                    MainBase.noab.SwitchUser = $"{MainBase.lang.UserNameLab} : {SwitchUserValue}";
                    //更改注册表账号状态
                    MainBase.IniModel.SwitchUser = SwitchUserValue;
                    RegistryControl registryControl = new();
                    registryControl.SetToRegedit(SwitchUserValue);
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
                return MainBase.IniModel.Cps switch
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
