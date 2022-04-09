using GenShin_Launcher_Plus.ViewModels;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface IGameConvertService
    {
        /// <summary>
        /// 异步转换客户端文件
        /// </summary>
        Task ConvertGameFileAsync(SettingsPageViewModel vm);

        /// <summary>
        /// 转换国际服及转换国服核心逻辑-判断客户端
        /// </summary>
        string GetCurrentSchemeName();

        /// <summary>
        /// 转换国际服及转换国服核心逻辑-判断PKG文件版本
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        bool CheckPackageVersion(string scheme, SettingsPageViewModel vm);

        /// <summary>
        /// 遍历判断文件是否存在
        /// </summary>
        /// <param name="dirpath"></param>
        /// <param name="filepath"></param>
        /// <param name="length"></param>
        /// <param name="vm"></param>
        /// <param name="surfix"></param>
        /// <returns></returns>
        bool CheckFileIntegrity(string dirpath, string[] filepath, int length, SettingsPageViewModel vm, string surfix = "");

        /// <summary>
        /// 替换客户端文件
        /// </summary>
        /// <param name="originalfile"></param>
        /// <param name="newfile"></param>
        /// <param name="scheme"></param>
        /// <param name="vm"></param>
        Task ReplaceGameFiles(string[] originalfile, string[] newfile, string scheme, SettingsPageViewModel vm);

        /// <summary>
        /// 还原客户端文件
        /// </summary>
        /// <param name="newfile"></param>
        /// <param name="originalfile"></param>
        /// <param name="scheme"></param>
        /// <param name="vm"></param>
        Task RestoreGameFiles(string[] newfile, string[] originalfile, string scheme, SettingsPageViewModel vm);

        /// <summary>
        /// 保存游戏客户端配置
        /// </summary>
        /// <param name="vm"></param>
        void SaveGameConfig(SettingsPageViewModel vm);
    }
}
