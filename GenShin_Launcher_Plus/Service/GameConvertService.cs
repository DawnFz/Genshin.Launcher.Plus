using GenShin_Launcher_Plus.Service.IService;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using GenShin_Launcher_Plus.ViewModels;
using GenShin_Launcher_Plus.Helper;

namespace GenShin_Launcher_Plus.Service
{
    public class GameConvertService : IGameConvertService
    {
        private readonly string[] globalfiles = new string[]
        { "GenshinImpact_Data/app.info",
          "GenshinImpact_Data/globalgamemanagers",
          "GenshinImpact_Data/globalgamemanagers.assets",
          "GenshinImpact_Data/globalgamemanagers.assets.resS",
          "GenshinImpact_Data/upload_crash.exe",
          "GenshinImpact_Data/Managed/Metadata/global-metadata.dat" ,
          "GenshinImpact_Data/Native/Data/Metadata/global-metadata.dat",
          "GenshinImpact_Data/Native/UserAssembly.dll",
          "GenshinImpact_Data/Native/UserAssembly.exp",
          "GenshinImpact_Data/Native/UserAssembly.lib",
          //2.6新加部分
          "GenshinImpact_Data/Plugins/ZFProxyWeb.dll",
          "GenshinImpact_Data/Plugins/ZFEmbedWeb.dll",
          "GenshinImpact_Data/Plugins/zf_cef.dll",
          "GenshinImpact_Data/Plugins/XInputInterface64.dll",
          "GenshinImpact_Data/Plugins/widevinecdmadapter.dll",
          "GenshinImpact_Data/Plugins/sqlite3.dll",
          "GenshinImpact_Data/Plugins/Rewired_DirectInput.dll",
          "GenshinImpact_Data/Plugins/metakeeper.dll",
          "GenshinImpact_Data/Plugins/libUbiCustomEvent.dll",
          "GenshinImpact_Data/Plugins/libGLESv2.dll",
          "GenshinImpact_Data/Plugins/libEGL.dll",
          "GenshinImpact_Data/Plugins/InControlNative.dll",
          "GenshinImpact_Data/Plugins/chrome_elf.dll",
          //
          "GenshinImpact_Data/Plugins/cri_mana_vpx.dll",
          "GenshinImpact_Data/Plugins/cri_vip_unity_pc.dll",
          "GenshinImpact_Data/Plugins/cri_ware_unity.dll",
          "GenshinImpact_Data/Plugins/d3dcompiler_43.dll",
          "GenshinImpact_Data/Plugins/d3dcompiler_47.dll",
          "GenshinImpact_Data/Plugins/hdiffz.dll",
          "GenshinImpact_Data/Plugins/hpatchz.dll",
          "GenshinImpact_Data/Plugins/mihoyonet.dll",
          "GenshinImpact_Data/Plugins/Mmoron.dll",
          "GenshinImpact_Data/Plugins/MTBenchmark_Windows.dll",
          "GenshinImpact_Data/Plugins/NamedPipeClient.dll",
          "GenshinImpact_Data/Plugins/UnityNativeChromaSDK.dll",
          "GenshinImpact_Data/Plugins/UnityNativeChromaSDK3.dll",
          "GenshinImpact_Data/Plugins/xlua.dll",
          "GenshinImpact_Data/StreamingAssets/20527480.blk",
          //2.6新加部分
          "mhyprot3.Sys",
          "mhyprot2.Sys",
          //
          "Audio_Chinese_pkg_version",
          "pkg_version",
          "UnityPlayer.dll",
          "GenshinImpact.exe"
        };

        private readonly string[] cnfiles = new string[]
        { "YuanShen_Data/app.info",
          "YuanShen_Data/globalgamemanagers",
          "YuanShen_Data/globalgamemanagers.assets",
          "YuanShen_Data/globalgamemanagers.assets.resS",
          "YuanShen_Data/upload_crash.exe",
          "YuanShen_Data/Managed/Metadata/global-metadata.dat" ,
          "YuanShen_Data/Native/Data/Metadata/global-metadata.dat",
          "YuanShen_Data/Native/UserAssembly.dll",
          "YuanShen_Data/Native/UserAssembly.exp",
          "YuanShen_Data/Native/UserAssembly.lib",
          //2.6新加部分
          "YuanShen_Data/Plugins/ZFProxyWeb.dll",
          "YuanShen_Data/Plugins/ZFEmbedWeb.dll",
          "YuanShen_Data/Plugins/zf_cef.dll",
          "YuanShen_Data/Plugins/XInputInterface64.dll",
          "YuanShen_Data/Plugins/widevinecdmadapter.dll",
          "YuanShen_Data/Plugins/sqlite3.dll",
          "YuanShen_Data/Plugins/Rewired_DirectInput.dll",
          "YuanShen_Data/Plugins/metakeeper.dll",
          "YuanShen_Data/Plugins/libUbiCustomEvent.dll",
          "YuanShen_Data/Plugins/libGLESv2.dll",
          "YuanShen_Data/Plugins/libEGL.dll",
          "YuanShen_Data/Plugins/InControlNative.dll",
          "YuanShen_Data/Plugins/chrome_elf.dll",
          //
          "YuanShen_Data/Plugins/cri_mana_vpx.dll",
          "YuanShen_Data/Plugins/cri_vip_unity_pc.dll",
          "YuanShen_Data/Plugins/cri_ware_unity.dll",
          "YuanShen_Data/Plugins/d3dcompiler_43.dll",
          "YuanShen_Data/Plugins/d3dcompiler_47.dll",
          "YuanShen_Data/Plugins/hdiffz.dll",
          "YuanShen_Data/Plugins/hpatchz.dll",
          "YuanShen_Data/Plugins/mihoyonet.dll",
          "YuanShen_Data/Plugins/Mmoron.dll",
          "YuanShen_Data/Plugins/MTBenchmark_Windows.dll",
          "YuanShen_Data/Plugins/NamedPipeClient.dll",
          "YuanShen_Data/Plugins/UnityNativeChromaSDK.dll",
          "YuanShen_Data/Plugins/UnityNativeChromaSDK3.dll",
          "YuanShen_Data/Plugins/xlua.dll",
          "YuanShen_Data/StreamingAssets/20527480.blk",
          //2.6新加部分
          "mhyprot3.Sys",
          "mhyprot2.Sys",
          //
          "Audio_Chinese_pkg_version",
          "pkg_version",
          "UnityPlayer.dll",
          "YuanShen.exe"
        };


        private const string CnFolderName = "Cn";
        private const string GlobalFolderName = "Global";
        private const string UnknownFolderName = "unknown";

        private const string YuanShenDataFolderName = "YuanShen_Data";
        private const string GenshinImpactDataFolderName = "GenshinImpact_Data";

        public GameConvertService()
        {
            GameFolder = App.Current.IniModel.GamePath;
        }
        private string GameFolder { get; set; }

        /// <summary>
        /// 转换游戏文件
        /// </summary>
        public async Task ConvertGameFileAsync(SettingsPageViewModel vm)
        {
            string currentPath = Environment.CurrentDirectory;
            string port = GetCurrentSchemeName();
            string newport = port == CnFolderName ? GlobalFolderName : CnFolderName;

            string[]? oldfiles = port switch
            {
                CnFolderName => cnfiles,
                GlobalFolderName => globalfiles,
                UnknownFolderName => null,
                _ => throw new NotImplementedException()
            };

            string[]? newfiles = port switch
            {
                CnFolderName => globalfiles,
                GlobalFolderName => cnfiles,
                UnknownFolderName => null,
                _ => throw new NotImplementedException()
            };

            await Task.Run(async () =>
            {
                if (oldfiles != null && newfiles != null)
                {
                    if (!CheckFileIntegrity(GameFolder, oldfiles, 1,vm, ".bak"))
                    {
                        if (Directory.Exists($"{currentPath}/{newport}File"))
                        {
                            if (CheckPackageVersion($"{newport}File", vm))
                            {
                                if (CheckFileIntegrity($"{currentPath}/{newport}File", newfiles, 0,vm))
                                {
                                    await ReplaceGameFiles(oldfiles, newfiles, newport, vm);
                                }
                            }
                            else
                            {
                                Directory.Delete($"{currentPath}/{newport}File", true);
                            }
                        }
                        else if (File.Exists($"{currentPath}/{newport}File.pkg"))
                        {
                            vm.StateIndicator = "状态：解压PKG资源文件中";
                            if (Decompress(newport))
                            {
                                if (CheckPackageVersion($"{newport}File", vm))
                                {
                                    await ReplaceGameFiles(oldfiles, newfiles, newport, vm);
                                }
                                else
                                {
                                    Directory.Delete($"{currentPath}/{newport}File", true);
                                    vm.StateIndicator = "状态：PKG资源文件有新版本";
                                    vm.ConvertState = false;
                                }
                            }
                            else
                            {
                                vm.StateIndicator = "状态：PKG解压失败，请检查PKG是否正常";
                                vm.ConvertingLog += $"资源[{newport}File.pkg]解压失败，请检查Pkg文件是否正常\r\n";
                                vm.ConvertState = false;
                            }
                        }
                        else
                        {
                            vm.StateIndicator = "状态：请检查PKG文件是否存在";
                            vm.ConvertingLog += $"没有找到资源[{newport}File.pkg]，请检查Pkg文件是否存在于本程序目录下\r\n";
                            vm.ConvertState = false;
                        }
                    }
                    else
                    {
                        await RestoreGameFiles(oldfiles, newfiles, port, vm);
                    }
                }
            });
        }

        /// <summary>
        /// 转换国际服及转换国服核心逻辑-判断客户端
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSchemeName()
        {
            if (File.Exists(Path.Combine(GameFolder, "YuanShen.exe")))
            {
                return CnFolderName;
            }
            else if (File.Exists(Path.Combine(GameFolder, "GenshinImpact.exe")))
            {
                return GlobalFolderName;
            }
            else
            {
                return UnknownFolderName;
            }
        }

        /// <summary>
        /// 转换国际服及转换国服核心逻辑-判断PKG文件版本
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public bool CheckPackageVersion(string scheme, SettingsPageViewModel vm)
        {
            string pkgfile = App.Current.UpdateObject.PkgVersion;
            if (!File.Exists($"{scheme}/{pkgfile}"))
            {
                vm.ConvertingLog = $"{App.Current.Language.NewPkgVer} : [{ pkgfile }]\r\n";
                ProcessStartInfo info = new()
                {
                    FileName = "https://pan.baidu.com/s/1-5zQoVfE7ImdXrn8OInKqg",
                    UseShellExecute = true,
                };
                Process.Start(info);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 遍历判断文件是否存在
        /// </summary>
        /// <param name="dirpath"></param>
        /// <param name="filepath"></param>
        /// <param name="length"></param>
        /// <param name="surfix"></param>
        /// <returns></returns>
        public bool CheckFileIntegrity(string dirpath, string[] filepath, int length, SettingsPageViewModel vm, string surfix = "")
        {
            bool succeed = true;
            for (int i = 0; i < filepath.Length - length; i++)
            {
                if (File.Exists(Path.Combine(dirpath, filepath[i] + surfix)) == false)
                {
                    vm.ConvertingLog += $"{filepath[i]} {surfix} 文件不存在，将尝试下一步操作\r\n若无反应请重新下载资源文件！\r\n";
                    succeed = false;
                    break;
                }
                vm.ConvertingLog += $"{filepath[i]} {surfix} 存在\r\n";
            }
            return succeed;
        }

        /// <summary>
        /// 替换客户端文件
        /// </summary>
        public async Task ReplaceGameFiles(string[] originalfile, string[] newfile, string scheme, SettingsPageViewModel vm)
        {
            vm.StateIndicator = "状态：备份原始客户端中";
            for (int a = 0; a < originalfile.Length; a++)
            {
                string newFileName = Path.Combine(GameFolder, originalfile[a]);
                if (File.Exists(Path.Combine(GameFolder, originalfile[a])))
                {
                    try
                    {
                        File.Move(newFileName, newFileName + ".bak");
                        vm.ConvertingLog += $"{newFileName} 备份成功\r\n";
                    }
                    catch (Exception ex)
                    {
                        vm.ConvertingLog += $"{newFileName} 备份失败：{ex.Message}\r\n";
                    }
                }
                else
                {
                    vm.ConvertingLog += $"{newFileName} 文件不存在，备份失败，跳过\r\n";
                }
            }

            vm.StateIndicator = "状态：正在替换新文件到客户端";
            string originalGameDataFolder = scheme == GlobalFolderName ? YuanShenDataFolderName : GenshinImpactDataFolderName;
            string newGameDataFolder = scheme == GlobalFolderName ? GenshinImpactDataFolderName : YuanShenDataFolderName;

            Directory.Move(Path.Combine(GameFolder, originalGameDataFolder), Path.Combine(GameFolder, newGameDataFolder));
            for (int i = 0; i < newfile.Length; i++)
            {
                File.Copy(Path.Combine(@$"{scheme}File", newfile[i]), Path.Combine(GameFolder, newfile[i]), true);
                vm.ConvertingLog += $"{newfile[i]} 替换成功\r\n";
            };
            //
            string cps = scheme == CnFolderName ? "pcadbdpz" : "mihoyo";
            vm.isMihoyo = cps == "mihoyo" ? 2 : 0;
            vm.ConvertState = true;
            //
            SaveGameConfig(vm);
            vm.ConvertingLog += "转换完成，您可以启动游戏了";
            vm.StateIndicator = "状态：无状态";
        }

        /// <summary>
        /// 还原客户端文件
        /// </summary>
        /// <param name="newfile"></param>
        /// <param name="originalfile"></param>
        /// <param name="scheme"></param>
        public async Task RestoreGameFiles(string[] newfile, string[] originalfile, string scheme, SettingsPageViewModel vm)
        {
            vm.StateIndicator = "状态：清理多余文件中";
            for (int i = 0; i < newfile.Length; i++)
            {
                if (File.Exists(Path.Combine(GameFolder, newfile[i])))
                {
                    File.Delete(Path.Combine(GameFolder, newfile[i]));
                    vm.ConvertingLog += $"{newfile[i]} 清理完毕\r\n";
                }
                else
                {
                    vm.ConvertingLog += $"{newfile[i]} 文件不存在，已跳过\r\n";
                }
            }
            vm.StateIndicator = "状态：正在还原原始客户端文件";

            string nowGameDataFolder = scheme == GlobalFolderName ? GenshinImpactDataFolderName : YuanShenDataFolderName;
            string originalGameDataFolder = scheme == GlobalFolderName ? YuanShenDataFolderName : GenshinImpactDataFolderName;

            Directory.Move(Path.Combine(GameFolder, nowGameDataFolder), Path.Combine(GameFolder, originalGameDataFolder));
            int total = 0, success = 0;
            for (int a = 0; a < originalfile.Length; a++)
            {
                string newFileName = Path.Combine(GameFolder, originalfile[a]);
                if (File.Exists(Path.Combine(GameFolder, originalfile[a] + ".bak")))
                {
                    Directory.Move(newFileName + ".bak", newFileName);
                    vm.ConvertingLog += $"{originalfile[a]} 还原成功\r\n";
                    success++;
                }
                else
                {
                    vm.ConvertingLog += $"{originalfile[a]} 跳过还原\r\n";
                    total++;
                }
            }
            //
            string cps = scheme == CnFolderName ? "pcadbdpz" : "mihoyo";
            vm.isMihoyo = cps == "mihoyo" ? 0 : 2;
            vm.ConvertState = true;
            //
            SaveGameConfig(vm);
            vm.StateIndicator = "状态：无状态";
            vm.ConvertingLog += $"还原完毕 , 还原成功 : {success} 个文件 ,还原失败 : {total} 个文件\r\n";
            vm.ConvertingLog += "转换完成，您可以启动游戏了";
        }

        /// <summary>
        /// 保存游戏客户端配置
        /// </summary>
        /// <param name="vm"></param>
        public void SaveGameConfig(SettingsPageViewModel vm)
        {
            if (File.Exists(Path.Combine(App.Current.IniModel.GamePath, "config.ini")))
            {
                string bilibilisdk = "Plugins/PCGameSDK.dll";
                switch (vm.isMihoyo)
                {
                    case 0:
                        App.Current.IniModel.Cps = "pcadbdpz";
                        App.Current.IniModel.Channel = 1;
                        App.Current.IniModel.Sub_channel = 1;
                        if (File.Exists(Path.Combine(GameFolder, $"YuanShen_Data/{bilibilisdk}")))
                            File.Delete(Path.Combine(GameFolder, $"YuanShen_Data/{bilibilisdk}"));
                        App.Current.NoticeOverAllBase.SwitchPort = $"{App.Current.Language.GameClientStr} : {App.Current.Language.GameClientTypePStr}";
                        App.Current.NoticeOverAllBase.IsGamePortLists = "Visible";
                        App.Current.NoticeOverAllBase.GamePortListIndex = 0;
                        break;
                    case 1:
                        App.Current.IniModel.Cps = "bilibili";
                        App.Current.IniModel.Channel = 14;
                        App.Current.IniModel.Sub_channel = 0;
                        if (!File.Exists(Path.Combine(GameFolder, $"YuanShen_Data/{bilibilisdk}")))
                        {
                            try
                            {
                                string sdkPath = Path.Combine(GameFolder, $"YuanShen_Data/{bilibilisdk}");
                                FileHelper.ExtractEmbededAppResource("StaticRes/mihoyosdk.dll", sdkPath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        App.Current.NoticeOverAllBase.SwitchPort = $"{App.Current.Language.GameClientStr} : {App.Current.Language.GameClientTypeBStr}";
                        App.Current.NoticeOverAllBase.IsGamePortLists = "Visible";
                        App.Current.NoticeOverAllBase.GamePortListIndex = 1;

                        break;
                    case 2:
                        App.Current.IniModel.Cps = "mihoyo";
                        App.Current.IniModel.Channel = 1;
                        App.Current.IniModel.Sub_channel = 0;
                        if (File.Exists(Path.Combine(GameFolder, $"GenshinImpact_Data/{bilibilisdk}")))
                            File.Delete(Path.Combine(GameFolder, $"GenshinImpact_Data/{bilibilisdk}"));
                        App.Current.NoticeOverAllBase.SwitchPort = $"{App.Current.Language.GameClientStr} : {App.Current.Language.GameClientTypeMStr}";
                        App.Current.NoticeOverAllBase.IsGamePortLists = "Hidden";
                        App.Current.NoticeOverAllBase.GamePortListIndex = -1;
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="archiveName"></param>
        /// <returns></returns>
        private bool Decompress(string archiveName)
        {
            try
            {
                ZipFile.ExtractToDirectory($"{Environment.CurrentDirectory}/{archiveName}File.pkg", Environment.CurrentDirectory, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
