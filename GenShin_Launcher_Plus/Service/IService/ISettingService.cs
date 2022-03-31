using GenShin_Launcher_Plus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface ISettingService
    {
        void SettingsPageCreated();

        void SetDisplaySelectedIndex(int index);

        List<UserListModel> ReadUserList();

        List<DisplaySizeListModel> CreateDisplaySizeList();

        List<GameWindowModeListModel> CreateGameWindowModeList();

        List<GamePortListModel> CreateGamePortList();

    }
}
