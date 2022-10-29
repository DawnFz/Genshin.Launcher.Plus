using GenShin_Launcher_Plus.ViewModels;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface IGameConvertService
    {

        /// <summary>
        /// 异步转换游戏客户端文件
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task ConvertGameFileAsync(SettingsPageViewModel vm);


        /// <summary>
        /// 保存游戏客户端配置
        /// </summary>
        /// <param name="vm"></param>
        void SaveGameConfig(SettingsPageViewModel vm);
    }
}
