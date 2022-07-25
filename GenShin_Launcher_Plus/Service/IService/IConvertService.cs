using GenShin_Launcher_Plus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface IConvertService
    {
        /// <summary>
        /// 异步转换游戏客户端文件
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task ConvertGameFileAsync(SettingsPageViewModel vm);

        /// <summary>
        /// 转换国际服及转换国服核心逻辑-判断PKG文件版本
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        bool CheckPackageVersion(string scheme, SettingsPageViewModel vm);

        /// <summary>
        /// 保存游戏客户端配置
        /// </summary>
        /// <param name="vm"></param>
        void SaveGameConfig(SettingsPageViewModel vm);
    }
}
