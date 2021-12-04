using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace GenShin_LauncherDIY.Config
{
    public class Settings
    {
        public static string Height = IniGS.Height.ToString();
        public static string Width = IniGS.Width.ToString();
        public static string GamePath = IniGS.gamePath.ToString();
        public static string GamePopup;
        public static string FullS;
        public static string Biomi;
        public static string GameMovePath;
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

        public static string aboutthis = 
            ("这是一个由WPF编写的原神启动器\r\n\r\n" +
            "你可以使用本启动器做到以下操作：\r\n" +
            "1.快速跳转到游戏的照相保存文件夹\r\n" +
            "2.自定义任意分辨率和是否全屏启动游戏\r\n" +
            "3.选择哔哩服,官服或者国际服进行启动\r\n" +
            "4.修复MihoyoSDK缺失导致的解析错误或未初始化\r\n" +
            "5.保存原神账户凭证可选快速切换账号启动\r\n" +
            "————————————————————————————\r\n" +
            "注意，以上功能涉及到注册表修改和文件替换，部分杀毒软\r\n" +
            "件可能会报毒，为了客户端数据完整建议关闭杀软后再运行\r\n" +
            "本程序完全开源，并不会将用户数据公布到网络，\r\n" +
            "本启动器需要联网部分的代码仅为版本检测和公告获取\r\n\r\n\r\n"+
            "编写：DawnFz (ねねだん)\r\n" +
            "联系邮箱：admin@dawnfz.com\r\n" +
            "您可以跳转到Github以获取本项目源代码\r\n\r\n\r\n" +
            "————————本程序用到的代码及参考————————\r\n" +
            "[genshin-account]\r\n" +
            "项目地址：https://github.com/babalae/genshin-account \r\n" +
            "————————————————————————————\r\n" +
            "[MahApps.Metro]\r\n" +
            "项目地址：https://github.com/MahApps/MahApps.Metro \r\n" +
            "————————————————————————————\r\n" +
            "[ICSharpCode.SharpZipLib]\r\n" +
            "项目地址：https://github.com/icsharpcode \r\n" +
            "————————————————————————————\r\n" +
            "[genshin-fps-unlock]\r\n" +
            "项目地址：https://gitee.com/Euphony_Facetious/genshin-fps-unlock \r\n");

        public static string hajimete =
            ("这是一个由WPF编写的原神启动器\r\n\r\n" +
            "你可以使用本启动器做到以下操作：\r\n" +
            "1.快速跳转到游戏的照相保存文件夹\r\n" +
            "2.自定义任意分辨率和是否全屏启动游戏\r\n" +
            "3.选择哔哩服,官服或者国际服进行启动\r\n" +
            "4.修复MihoyoSDK缺失导致的解析错误或未初始化\r\n" +
            "5.保存原神账户凭证可选快速切换账号启动\r\n" +
            "—————————————————————————————\r\n" +
            "注意，以上功能涉及到注册表修改和文件替换，部分杀毒软\r\n" +
            "件可能会报毒，为了客户端数据完整建议关闭杀软后再运行\r\n" +
            "本程序完全开源，并不会将用户数据公布到网络，\r\n" +
            "本启动器需要联网部分的代码仅为版本检测和公告获取\r\n");
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
                IniGS.isPopup = false;
                IniGS.isUnFPS = false;
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

