using GenShin_Launcher_Plus.Service.IService;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
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
            GameFolder = App.Current.DataModel.GamePath;
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
                    if (!CheckFileIntegrity(GameFolder, oldfiles, 1, vm, ".bak"))
                    {
                        if (Directory.Exists($"{currentPath}/{newport}File"))
                        {
                            if (CheckPackageVersion($"{newport}File", vm))
                            {
                                if (CheckFileIntegrity($"{currentPath}/{newport}File", newfiles, 0, vm))
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
                            vm.StateIndicator = App.Current.Language.StateIndicatorUning;
                            if (Decompress(newport))
                            {
                                if (CheckPackageVersion($"{newport}File", vm))
                                {
                                    await ReplaceGameFiles(oldfiles, newfiles, newport, vm);
                                }
                                else
                                {
                                    Directory.Delete($"{currentPath}/{newport}File", true);
                                    vm.StateIndicator = App.Current.Language.StateIndicatorUpdate;
                                    vm.ConvertState = false;
                                }
                            }
                            else
                            {
                                vm.StateIndicator = App.Current.Language.StateIndicatorUnErr;
                                vm.ConvertingLog += $"[{newport}File.pkg]{App.Current.Language.ErrorPkgUnzip},{App.Current.Language.ErrorPkgNF}\r\n";
                                vm.ConvertState = false;
                            }
                        }
                        else
                        {
                            vm.StateIndicator = App.Current.Language.StateIndicatorCheck;
                            vm.ConvertingLog += $"[{newport}File.pkg]{App.Current.Language.Error},{App.Current.Language.ErrorPkgNF}\r\n";
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
                    vm.ConvertingLog += $"{filepath[i]} {surfix} {App.Current.Language.ErrorFileNF}\r\n";
                    succeed = false;
                    break;
                }
                vm.ConvertingLog += $"{filepath[i]} {surfix} {App.Current.Language.FileExist}\r\n";
            }
            return succeed;
        }

        /// <summary>
        /// 替换客户端文件
        /// </summary>
        public async Task ReplaceGameFiles(string[] originalfile, string[] newfile, string scheme, SettingsPageViewModel vm)
        {
            vm.StateIndicator = App.Current.Language.StateIndicatorBaking;
            for (int a = 0; a < originalfile.Length; a++)
            {
                string newFileName = Path.Combine(GameFolder, originalfile[a]);
                if (File.Exists(Path.Combine(GameFolder, originalfile[a])))
                {
                    try
                    {
                        File.Move(newFileName, newFileName + ".bak");
                        vm.ConvertingLog += $"{newFileName} {App.Current.Language.BakSuccess}\r\n";
                    }
                    catch (Exception ex)
                    {
                        vm.ConvertingLog += $"{newFileName} {App.Current.Language.ErrorBakF} {ex.Message}\r\n";
                    }
                }
                else
                {
                    vm.ConvertingLog += $"{newFileName} {App.Current.Language.BakFileNfSk}\r\n";
                }
            }

            vm.StateIndicator = App.Current.Language.StateIndicatorReping;
            string originalGameDataFolder = scheme == GlobalFolderName ? YuanShenDataFolderName : GenshinImpactDataFolderName;
            string newGameDataFolder = scheme == GlobalFolderName ? GenshinImpactDataFolderName : YuanShenDataFolderName;

            Directory.Move(Path.Combine(GameFolder, originalGameDataFolder), Path.Combine(GameFolder, newGameDataFolder));
            for (int i = 0; i < newfile.Length; i++)
            {
                File.Copy(Path.Combine(@$"{scheme}File", newfile[i]), Path.Combine(GameFolder, newfile[i]), true);
                vm.ConvertingLog += $"{newfile[i]} {App.Current.Language.RepSuccess}\r\n";
            };
            //
            string cps = scheme == CnFolderName ? "pcadbdpz" : "mihoyo";
            vm.isMihoyo = cps == "mihoyo" ? 2 : 0;
            vm.ConvertState = true;
            //
            SaveGameConfig(vm);
            vm.ConvertingLog += App.Current.Language.RestoreEndStr;
            vm.StateIndicator = App.Current.Language.StateIndicatorDefault;
        }

        /// <summary>
        /// 还原客户端文件
        /// </summary>
        /// <param name="newfile"></param>
        /// <param name="originalfile"></param>
        /// <param name="scheme"></param>
        public async Task RestoreGameFiles(string[] newfile, string[] originalfile, string scheme, SettingsPageViewModel vm)
        {
            vm.StateIndicator = App.Current.Language.StateIndicatorCleaning;
            for (int i = 0; i < newfile.Length; i++)
            {
                if (File.Exists(Path.Combine(GameFolder, newfile[i])))
                {
                    File.Delete(Path.Combine(GameFolder, newfile[i]));
                    vm.ConvertingLog += $"{newfile[i]} {App.Current.Language.CleanedStr}\r\n";
                }
                else
                {
                    vm.ConvertingLog += $"{newfile[i]} {App.Current.Language.CleanSkipStr}\r\n";
                }
            }
            vm.StateIndicator = App.Current.Language.StateIndicatorRecover;

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
                    vm.ConvertingLog += $"{originalfile[a]} {App.Current.Language.RestoreSucess}\r\n";
                    success++;
                }
                else
                {
                    vm.ConvertingLog += $"{originalfile[a]} {App.Current.Language.RestoreSkipStr}\r\n";
                    total++;
                }
            }
            //
            string cps = scheme == CnFolderName ? "pcadbdpz" : "mihoyo";
            vm.isMihoyo = cps == "mihoyo" ? 0 : 2;
            vm.ConvertState = true;
            //
            SaveGameConfig(vm);
            vm.StateIndicator = App.Current.Language.StateIndicatorDefault;
            vm.ConvertingLog += $"{App.Current.Language.RestoreOverTipsStr}, {App.Current.Language.RestoreNum} :{success}, {App.Current.Language.RestoreErrNum} :{total} \r\n";
            vm.ConvertingLog += App.Current.Language.RestoreEndStr;
        }

        /// <summary>
        /// 保存游戏客户端配置
        /// </summary>
        /// <param name="vm"></param>
        public void SaveGameConfig(SettingsPageViewModel vm)
        {
            if (File.Exists(Path.Combine(App.Current.DataModel.GamePath, "config.ini")))
            {
                string bilibilisdk = "Plugins/PCGameSDK.dll";
                switch (vm.isMihoyo)
                {
                    case 0:
                        App.Current.DataModel.Cps = "pcadbdpz";
                        App.Current.DataModel.Channel = 1;
                        App.Current.DataModel.Sub_channel = 1;
                        if (File.Exists(Path.Combine(GameFolder, $"YuanShen_Data/{bilibilisdk}")))
                            File.Delete(Path.Combine(GameFolder, $"YuanShen_Data/{bilibilisdk}"));
                        App.Current.NoticeOverAllBase.SwitchPort = $"{App.Current.Language.GameClientStr} : {App.Current.Language.GameClientTypePStr}";
                        App.Current.NoticeOverAllBase.IsGamePortLists = "Visible";
                        App.Current.NoticeOverAllBase.GamePortListIndex = 0;
                        break;
                    case 1:
                        App.Current.DataModel.Cps = "bilibili";
                        App.Current.DataModel.Channel = 14;
                        App.Current.DataModel.Sub_channel = 0;
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
                        App.Current.DataModel.Cps = "mihoyo";
                        App.Current.DataModel.Channel = 1;
                        App.Current.DataModel.Sub_channel = 0;
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
