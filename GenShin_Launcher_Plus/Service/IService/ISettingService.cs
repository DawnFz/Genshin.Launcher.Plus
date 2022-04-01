using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.ViewModels;
using System.Collections.Generic;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface ISettingService
    {
        void SettingsPageCreated(SettingsPageViewModel vm);

        void SetDisplaySelectedIndex(int index, SettingsPageViewModel vm);

        List<DisplaySizeListModel> CreateDisplaySizeList();

        List<GameWindowModeListModel> CreateGameWindowModeList();

        List<GamePortListModel> CreateGamePortList();

    }
}
