using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.Service.IService;
using GenShin_Launcher_Plus.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace GenShin_Launcher_Plus.Service
{
    public class SettingService : ISettingService
    {
        public SettingService(SettingsPageViewModel vm)
        {
            SettingsPageCreated(vm);
        }

        public void SettingsPageCreated(SettingsPageViewModel vm)
        {
            vm.SwitchUser = App.Current.IniModel.SwitchUser;
            vm.GamePath = App.Current.IniModel.GamePath;
            vm.isUnFPS = App.Current.IniModel.isUnFPS;
            vm.Width = App.Current.IniModel.Width ?? "1600";
            vm.Height = App.Current.IniModel.Height ?? "900";
            vm.ConvertingLog = App.Current.Language.ConvertingLogStr;
            vm.StateIndicator = App.Current.Language.StateIndicatorDefault;
            vm.isMihoyo = App.Current.IniModel.Cps switch
            {
                "pcadbdpz" => 0,
                "bilibili" => 1,
                "mihoyo" => 2,
                _ => 3,
            };
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

        public void SetDisplaySelectedIndex(int index, SettingsPageViewModel vm)
        {
            switch (index)
            {
                case 0:
                    vm.Width = "3840";
                    vm.Height = "2160";
                    break;
                case 1:
                    vm.Width = "2560";
                    vm.Height = "1080";
                    break;
                case 2:
                    vm.Width = "1920";
                    vm.Height = "1080";
                    break;
                case 3:
                    vm.Width = "1600";
                    vm.Height = "900";
                    break;
                case 4:
                    vm.Width = "1360";
                    vm.Height = "768";
                    break;
                case 5:
                    vm.Width = "1280";
                    vm.Height = "1024";
                    break;
                case 6:
                    vm.Width = "1280";
                    vm.Height = "720";
                    break;
                case 7:
                    vm.Width = Convert.ToString(SystemParameters.PrimaryScreenWidth);
                    vm.Height = Convert.ToString(SystemParameters.PrimaryScreenHeight);
                    break;
                default:
                    break;
            }
        }
    }

}
