using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GenShin_LauncherDIY.Config
{
    class IniControl
    {
        /// <summary>  
        /// 写操作  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="value">值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);


        /// <summary>  
        /// 读操作  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="def">未读取到的默认值</param>  
        /// <param name="retVal">读取到的值</param>  
        /// <param name="size">大小</param>  
        /// <param name="filePath">路径</param>  
        /// <returns></returns>  
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>  
        /// 读ini文件不存在则返回1
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="defValue">未读取到值时的默认值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static string ReadIniInt(string section, string key)
        {
            string IniFilePath = @"Config\Setting.ini";
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, IniFilePath);
            if (i == 0)
                return "1";
            else
                return temp.ToString();
        }

        /// <summary>  
        /// 读ini文件不存在则返回false  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="defValue">未读取到值时的默认值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static string ReadIniStr(string section, string key)
        {
            string IniFilePath = @"Config\Setting.ini";
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, IniFilePath);
            if (i == 0)
                return "false";
            else
                return temp.ToString();
        }

        /// <summary>  
        /// 读ini文件不存在则返回空  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="defValue">未读取到值时的默认值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static string ReadIniPath(string section, string key)
        {
            string IniFilePath = @"Config\Setting.ini";
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, IniFilePath);
            if (i == 0)
                return "";
            else
                return temp.ToString();
        }

        /// <summary>  
        /// 写入ini文件  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="value">值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static void WriteIni(string section, string key, string value)
        {
            string IniFilePath = @"Config\Setting.ini";
            WritePrivateProfileString(section, key, value, IniFilePath);
        }
        /// <summary>  
        /// 删除节  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="filePath"></param>  
        /// <returns></returns>  
        public static long DeleteSection(string section)
        {
            string IniFilePath = @"Config\Setting.ini";
            return WritePrivateProfileString(section, null, null, IniFilePath);
        }

        /// <summary>  
        /// 删除键  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static long DeleteKey(string section, string key)
        {
            string IniFilePath = @"Config\Setting.ini";
            return WritePrivateProfileString(section, key, null, IniFilePath);
        }
    }
    class IniGS

    {
        private static string _gamePath;
        /// <summary>
        /// 游戏路径属性
        /// </summary>
        public static string gamePath
        {
            get
            {
                _gamePath = IniControl.ReadIniPath("setup", "GamePath");
                return _gamePath;
            }
            set
            {
                _gamePath = value;
                IniControl.WriteIni("setup", "GamePath", _gamePath);
            }
        }



        private static bool _isAutoSize;
        /// <summary>
        /// 是否全屏属性
        /// </summary>
        public static bool isAutoSize
        {
            get
            {
                _isAutoSize = Convert.ToBoolean(IniControl.ReadIniStr("setup", "isAutoSize"));
                return _isAutoSize;
            }
            set
            {
                _isAutoSize = value;
                IniControl.WriteIni("setup", "isAutoSize", Convert.ToString(_isAutoSize));
            }
        }

        private static bool _isPopup;
        /// <summary>
        /// 是否无边框属性
        /// </summary>
        public static bool isPopup
        {
            get
            {
                _isPopup = Convert.ToBoolean(IniControl.ReadIniStr("setup", "isPopup"));
                return _isPopup;
            }
            set
            {
                _isPopup = value;
                IniControl.WriteIni("setup", "isPopup", Convert.ToString(_isPopup));
            }
        }



        private static ushort _Width;
        /// <summary>
        /// X尺寸属性
        /// </summary>
        public static ushort Width
        {
            get
            {
                _Width = Convert.ToUInt16(IniControl.ReadIniInt("setup", "Width"));
                return _Width;
            }
            set
            {
                _Width = value;
                IniControl.WriteIni("setup", "Width", Convert.ToString(_Width));
            }
        }

        private static ushort _Height;
        /// <summary>
        /// Y尺寸属性
        /// </summary>
        public static ushort Height
        {
            get
            {
                _Height = Convert.ToUInt16(IniControl.ReadIniInt("setup", "Height"));
                return _Height;
            }
            set
            {
                _Height = value;
                IniControl.WriteIni("setup", "Height", Convert.ToString(_Height));
            }
        }

        private static ushort _BiOrMi;
        /// <summary>
        /// 客户端服务器接口
        /// </summary>
        public static ushort BiOrMi
        {
            get
            {
                _BiOrMi = Convert.ToUInt16(IniControl.ReadIniInt("setup", "isMihoyo"));
                return _BiOrMi;
            }
            set
            {
                _BiOrMi = value;
                IniControl.WriteIni("setup", "isMihoyo", Convert.ToString(_BiOrMi));
            }
        }

        private static bool _isUnFPS;
        /// <summary>
        /// 是否解锁帧率上限
        /// </summary>
        public static bool isUnFPS
        {
            get
            {
                _isUnFPS = Convert.ToBoolean(IniControl.ReadIniStr("setup", "isUnFPS"));
                return _isUnFPS;
            }
            set
            {
                _isUnFPS = value;
                IniControl.WriteIni("setup", "isUnFPS", Convert.ToString(_isUnFPS));
            }
        }


        private static string _MaxFps;
        /// <summary>
        /// 最高FPS上限
        /// </summary>
        public static string MaxFps
        {
            get
            {
                _MaxFps = IniControl.ReadIniPath("setup", "MaxFps");
                return _MaxFps;
            }
            set
            {
                _MaxFps = value;
                IniControl.WriteIni("setup", "MaxFps", _MaxFps);
            }
        }

        /// <summary>
        /// EXE文件名字属性
        /// </summary>
        public static void EXEname(string value)
        {
            IniControl.WriteIni("setup", "LauncherPlusName", value);
        }
    }
    class YuanshenIni
    {
        /// <summary>  
        /// 写操作  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="value">值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        /// <summary>  
        /// 写入ini文件  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="value">值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static void WriteIni(string section, string key, string value)
        {
            string IniFilePath = Config.IniGS.gamePath + @"\\Genshin Impact Game\\config.ini";
            WritePrivateProfileString(section, key, value, IniFilePath);
        }
    }
    class BOM
    {
        /// <summary>
        /// Channel属性
        /// </summary>
        public static void Channel(string value)
        {
            YuanshenIni.WriteIni("General", "channel", value);
        }

        /// <summary>
        /// SUB_Channel属性
        /// </summary>
        public static void Sub_channel(string value)
        {
            YuanshenIni.WriteIni("General", "sub_channel", value);
        }

        /// <summary>
        /// CPS属性
        /// </summary>
        public static void Cps(string value)
        {
            YuanshenIni.WriteIni("General", "cps",value);
        }
    }
}
