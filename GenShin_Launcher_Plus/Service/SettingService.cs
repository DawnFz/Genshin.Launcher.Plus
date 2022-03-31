using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.Service.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace GenShin_Launcher_Plus.Service
{
    public class SettingService : ISettingService
    {
        public SettingService(int delay)
        {
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                SettingsPageCreated();
            });
        }

        public void SettingsPageCreated()
        {
            App.Current.SettingsPageViewModel.SwitchUser = App.Current.IniModel.SwitchUser;
            App.Current.SettingsPageViewModel.GamePath = App.Current.IniModel.GamePath;
            App.Current.SettingsPageViewModel.isUnFPS = App.Current.IniModel.isUnFPS;
            App.Current.SettingsPageViewModel.Width = App.Current.IniModel.Width ?? "1600";
            App.Current.SettingsPageViewModel.Height = App.Current.IniModel.Height ?? "900";
            App.Current.SettingsPageViewModel.ConvertingLog = App.Current.Language.ConvertingLogStr;
            App.Current.SettingsPageViewModel.StateIndicator = App.Current.Language.StateIndicatorDefault;
            App.Current.SettingsPageViewModel.isMihoyo = App.Current.IniModel.Cps switch
            {
                "pcadbdpz" => 0,
                "bilibili" => 1,
                "mihoyo" => 2,
                _ => 3,
            };
        }

        public List<UserListModel> ReadUserList()
        {
            List<UserListModel> list = new();
            DirectoryInfo TheFolder = new(@"UserData");
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                list.Add(new UserListModel { UserName = NextFile.Name });
            }
            return list;
        }

        public List<DisplaySizeListModel> CreateDisplaySizeList()
        {
            List<DisplaySizeListModel> list = new()
            {
                new DisplaySizeListModel { DisplaySize = "3840 × 2160  | 16:9" },
                new DisplaySizeListModel { DisplaySize = "2560 × 1080  | 21:9" },
                new DisplaySizeListModel { DisplaySize = "1920 × 1080  | 16:9" },
                new DisplaySizeListModel { DisplaySize = "1600 × 900    | 16:9" },
                new DisplaySizeListModel { DisplaySize = "1360 × 768    | 16:9" },
                new DisplaySizeListModel { DisplaySize = "1280 × 1024  |  4:3" },
                new DisplaySizeListModel { DisplaySize = "1280 × 720    | 16:9" },
                new DisplaySizeListModel { DisplaySize = App.Current.Language.AdaptiveStr },
            };
            return list;
        }
        public List<GamePortListModel> CreateGamePortList()
        {
            List<GamePortListModel> list = new()
            {
                new GamePortListModel { GamePort = App.Current.Language.GameClientTypePStr },
                new GamePortListModel { GamePort = App.Current.Language.GameClientTypeBStr },
                new GamePortListModel { GamePort = App.Current.Language.GameClientTypeMStr }
            };
            return list;
        }

        public List<GameWindowModeListModel> CreateGameWindowModeList()
        {
            List<GameWindowModeListModel> list = new();
            list.Add(new GameWindowModeListModel { GameWindowMode = App.Current.Language.WindowMode });
            list.Add(new GameWindowModeListModel { GameWindowMode = App.Current.Language.Fullscreen });
            return list;
        }

        public void SetDisplaySelectedIndex(int index)
        {
            switch (index)
            {
                case 0:
                    App.Current.SettingsPageViewModel.Width = "3840";
                    App.Current.SettingsPageViewModel.Height = "2160";
                    break;
                case 1:
                    App.Current.SettingsPageViewModel.Width = "2560";
                    App.Current.SettingsPageViewModel.Height = "1080";
                    break;
                case 2:
                    App.Current.SettingsPageViewModel.Width = "1920";
                    App.Current.SettingsPageViewModel.Height = "1080";
                    break;
                case 3:
                    App.Current.SettingsPageViewModel.Width = "1600";
                    App.Current.SettingsPageViewModel.Height = "900";
                    break;
                case 4:
                    App.Current.SettingsPageViewModel.Width = "1360";
                    App.Current.SettingsPageViewModel.Height = "768";
                    break;
                case 5:
                    App.Current.SettingsPageViewModel.Width = "1280";
                    App.Current.SettingsPageViewModel.Height = "1024";
                    break;
                case 6:
                    App.Current.SettingsPageViewModel.Width = "1280";
                    App.Current.SettingsPageViewModel.Height = "720";
                    break;
                case 7:
                    App.Current.SettingsPageViewModel.Width = Convert.ToString(SystemParameters.PrimaryScreenWidth);
                    App.Current.SettingsPageViewModel.Height = Convert.ToString(SystemParameters.PrimaryScreenHeight);
                    break;
                default:
                    break;
            }
        }
    }

}
