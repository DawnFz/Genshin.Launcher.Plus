using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenShin_LauncherDIY.Config
{
    public class Settings
    {
        public static string Height = IniGS.Height.ToString();
        public static string Width = IniGS.Width.ToString();
        public static string GamePath = IniGS.gamePath.ToString();
        public static string FullS;
        public static string Biomi;
        //下面是检查需要替换的国际服文件的路径
        public static string[] globalfiles = new string[] 
        { "//GenshinImpact_Data//app.info",
          "//GenshinImpact_Data//globalgamemanagers",
          "//GenshinImpact_Data//Managed//Metadata//global-metadata.dat" ,
          "//GenshinImpact_Data//Native//Data//Metadata//global-metadata.dat",
          "//GenshinImpact_Data//Native//UserAssembly.dll",
          "//GenshinImpact_Data//Native//UserAssembly.exp",
          "//GenshinImpact_Data//Native//UserAssembly.lib",
          "//GenshinImpact_Data//Plugins//cri_mana_vpx.dll",
          "//GenshinImpact_Data//Plugins//cri_vip_unity_pc.dll",
          "//GenshinImpact_Data//Plugins//cri_ware_unity.dll",
          "//GenshinImpact_Data//Plugins//d3dcompiler_43.dll",
          "//GenshinImpact_Data//Plugins//d3dcompiler_47.dll",
          "//GenshinImpact_Data//Plugins//hdiffz.dll",
          "//GenshinImpact_Data//Plugins//hpatchz.dll",
          "//GenshinImpact_Data//Plugins//mihoyonet.dll",
          "//GenshinImpact_Data//Plugins//Mmoron.dll",
          "//GenshinImpact_Data//Plugins//MTBenchmark_Windows.dll",
          "//GenshinImpact_Data//Plugins//NamedPipeClient.dll",
          "//GenshinImpact_Data//Plugins//UnityNativeChromaSDK.dll",
          "//GenshinImpact_Data//Plugins//UnityNativeChromaSDK3.dll",
          "//GenshinImpact_Data//Plugins//xlua.dll",
          "//GenshinImpact_Data//StreamingAssets//20527480.blk",
          "//Audio_Chinese_pkg_version",
          "//GenshinImpact.exe",
          "//pkg_version",
          "//UnityPlayer.dll"
        };
        //下面是检查需要替换的国内双服的文件的路径
        public static string[] cnfiles = new string[]
        { "//YuanShen_Data//app.info",
          "//YuanShen_Data//globalgamemanagers",
          "//YuanShen_Data//Managed//Metadata//global-metadata.dat" ,
          "//YuanShen_Data//Native//Data//Metadata//global-metadata.dat",
          "//YuanShen_Data//Native//UserAssembly.dll",
          "//YuanShen_Data//Native//UserAssembly.exp",
          "//YuanShen_Data//Native//UserAssembly.lib",
          "//YuanShen_Data//Plugins//cri_mana_vpx.dll",
          "//YuanShen_Data//Plugins//cri_vip_unity_pc.dll",
          "//YuanShen_Data//Plugins//cri_ware_unity.dll",
          "//YuanShen_Data//Plugins//d3dcompiler_43.dll",
          "//YuanShen_Data//Plugins//d3dcompiler_47.dll",
          "//YuanShen_Data//Plugins//hdiffz.dll",
          "//YuanShen_Data//Plugins//hpatchz.dll",
          "//YuanShen_Data//Plugins//mihoyonet.dll",
          "//YuanShen_Data//Plugins//Mmoron.dll",
          "//YuanShen_Data//Plugins//MTBenchmark_Windows.dll",
          "//YuanShen_Data//Plugins//NamedPipeClient.dll",
          "//YuanShen_Data//Plugins//UnityNativeChromaSDK.dll",
          "//YuanShen_Data//Plugins//UnityNativeChromaSDK3.dll",
          "//YuanShen_Data//Plugins//xlua.dll",
          "//YuanShen_Data//StreamingAssets//20527480.blk",
          "//Audio_Chinese_pkg_version",
          "//YuanShen.exe",
          "//pkg_version",
          "//UnityPlayer.dll"
        };

    }

    class setConfig
    {
        /// <summary>
        /// 填补一个空的setting.ini配置文件
        /// </summary>
        public static void newini()
        {
            try
            {
                //gamepath
                IniGS.gamePath = "";
                //size
                IniGS.isAutoSize = false;
                IniGS.Width = 1280;
                IniGS.Height = 720;
                //bilibiliormihayo
                IniGS.BiOrMi = 1;
            }
            catch
            {
            }
        }



        /// <summary>
        /// 检查ini文件是否存在并且版本正确，否则新建
        /// </summary>
        public static void checkini()
        {
            try
            {
                if (Directory.Exists(@"Config") == false)
                {
                    Directory.CreateDirectory("Config");
                    setConfig.newini();
                }
                else if (File.Exists(@"Config\Setting.ini") == false)
                {
                    setConfig.newini();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// ini文件是否正常
        /// </summary>
        public static bool isRightIni()
        {
            try
            {
                if (!Directory.Exists(@"Config") || !File.Exists(@"Config\Setting.ini"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }

}

