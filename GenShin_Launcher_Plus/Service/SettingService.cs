using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.Service.IService;
using GenShin_Launcher_Plus.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GenShin_Launcher_Plus.Service
{
    public class SettingService : ISettingService
    {
        /// <summary>
        /// SettingsViewModel启动时数据初始化
        /// </summary>
        /// <param name="vm"></param>
        public SettingService(SettingsPageViewModel vm)
        {
            vm.Width = App.Current.DataModel.Width ?? "1600";
            vm.Height = App.Current.DataModel.Height ?? "900";
            vm.IsUnFPS = App.Current.DataModel.IsUnFPS;
            vm.GamePath = App.Current.DataModel.GamePath;
            vm.SwitchUser = App.Current.DataModel.SwitchUser;
            vm.IsPopup = App.Current.DataModel.IsPopup;
            vm.FullSize = App.Current.DataModel.FullSize;
            vm.MaxFps = App.Current.DataModel.MaxFps;
            vm.IsWebBg = App.Current.DataModel.IsWebBg;
            vm.UseXunkongWallpaper = App.Current.DataModel.UseXunkongWallpaper;
            vm.IsRunThenClose = App.Current.DataModel.IsRunThenClose;
            vm.IsCloseUpdate = App.Current.DataModel.IsCloseUpdate;
            vm.ConvertingLog = App.Current.Language.ConvertingLogStr;
            vm.StateIndicator = App.Current.Language.StateIndicatorDefault;
            vm.IsMihoyo = App.Current.DataModel.Cps switch
            {
                "pcadbdpz" => 0,
                "bilibili" => 1,
                "mihoyo" => 2,
                _ => 3,
            };
        }

        /// <summary>
        /// 从Json文件中反序列化分辨率列表到列表
        /// </summary>
        /// <returns></returns>
        public List<DisplaySizeListModel> CreateDisplaySizeList()
        {
            if (File.Exists(@"Config/DisplaySize.json"))
            {
                string json = File.ReadAllText(@"Config/DisplaySize.json");
                List<DisplaySizeListModel> list = JsonConvert.DeserializeObject<List<DisplaySizeListModel>>(json);
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 创建游戏客户端列表
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 创建启动窗口的模式列表
        /// </summary>
        /// <returns></returns>
        public List<GameWindowModeListModel> CreateGameWindowModeList()
        {
            List<GameWindowModeListModel> list = new();
            list.Add(new GameWindowModeListModel { GameWindowMode = App.Current.Language.WindowMode });
            list.Add(new GameWindowModeListModel { GameWindowMode = App.Current.Language.Fullscreen });
            return list;
        }

        /// <summary>
        /// 选中预设分辨率列表中的项操作
        /// </summary>
        /// <param name="sizeName"></param>
        /// <param name="vm"></param>
        public void SetDisplaySelectedValue(string sizeName, SettingsPageViewModel vm)
        {
            foreach (DisplaySizeListModel dsm in vm.DisplaySizeLists)
            {
                if (sizeName == dsm.SizeName)
                {
                    vm.Height = dsm.Height;
                    vm.Width = dsm.Width;
                }
            }
        }

        /// <summary>
        /// 将宽高数据存入预设分辨率列表
        /// 并将列表序列化为Json写入本地Json文件
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public void SaveDisplaySizeToList(SettingsPageViewModel vm, string Width, string Height)
        {
            List<DisplaySizeListModel> allList;
            if (vm.DisplaySizeLists[0].IsNull) { allList = new List<DisplaySizeListModel>(); }
            else { allList = new(vm.DisplaySizeLists); }
            foreach (DisplaySizeListModel dsm in allList)
            { if ($"{Width} x {Height}" == dsm.SizeName) { return; } }
            int divisor = GetDivisor(Convert.ToInt32(Width), Convert.ToInt32(Height));
            string windowScale = $"{(Convert.ToInt32(Width) / divisor)}:{(Convert.ToInt32(Height) / divisor)}";
            DisplaySizeListModel list = new();
            list.Width = Width;
            list.Height = Height;
            list.SizeName = $"{Width} x {Height}  |  {windowScale}";
            list.IsNull = false;
            allList.Add(list);
            string newJson = JsonConvert.SerializeObject(allList);
            File.WriteAllText(@"Config/DisplaySize.json", newJson);
            vm.DisplaySizeLists = allList;
        }

        /// <summary>
        /// 通过宽高来取得除数
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private int GetDivisor(int width, int height)
        {
            if (width % height == 0)
            { return height; }
            return GetDivisor(height, width % height);
        }
    }
}
