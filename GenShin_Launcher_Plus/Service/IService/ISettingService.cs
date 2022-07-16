using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.ViewModels;
using System.Collections.Generic;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface ISettingService
    {
        /// <summary>
        /// 从预设中设置分辨率大小
        /// </summary>
        /// <param name="sizeName"></param>
        /// <param name="vm"></param>
        void SetDisplaySelectedValue(string sizeName, SettingsPageViewModel vm);

        /// <summary>
        /// 保存分辨率到预设
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        void SaveDisplaySizeToList(SettingsPageViewModel vm, string Width, string Height);

        /// <summary>
        /// 删除预设里的分辨率选项
        /// </summary>
        /// <param name="vm"></param>
        public void RemoveDisplaySizeToList(SettingsPageViewModel vm);

        /// <summary>
        /// 添加每日一图Pid到本地并序列化到Json文件中
        /// </summary>
        /// <returns></returns>
        bool SetDailyImageDataToJson(SettingsPageViewModel vm);

        /// <summary>
        /// 创建分辨率列表
        /// </summary>
        /// <returns></returns>
        List<DisplaySizeListModel> CreateDisplaySizeList();

        /// <summary>
        /// 创建客户端列表
        /// </summary>
        /// <returns></returns>
        List<GameWindowModeListModel> CreateGameWindowModeList();

        /// <summary>
        /// 创建窗口模式列表
        /// </summary>
        /// <returns></returns>
        List<GamePortListModel> CreateGamePortList();

        /// <summary>
        /// 从本地Json文件中反序列化每日一图数据
        /// </summary>
        List<DailyImageArray> ReadDailyImageSourceFromJson();

    }
}
