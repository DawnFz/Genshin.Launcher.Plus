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
        /// 读ini文件  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="defValue">未读取到值时的默认值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static string ReadIni(string section, string key)
        {
            string IniFilePath = @"Config\Setting.ini";
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, IniFilePath);
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
                _gamePath = IniControl.ReadIni("setup", "GamePath");
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
                _isAutoSize = Convert.ToBoolean(IniControl.ReadIni("setup", "isAutoSize"));
                return _isAutoSize;
            }
            set
            {
                _isAutoSize = value;
                IniControl.WriteIni("setup", "isAutoSize", Convert.ToString(_isAutoSize));
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
                _Width = Convert.ToUInt16(IniControl.ReadIni("setup", "Width"));
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
                _Height = Convert.ToUInt16(IniControl.ReadIni("setup", "Height"));
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
        /// Y尺寸属性
        /// </summary>
        public static ushort BiOrMi
        {
            get
            {
                _BiOrMi = Convert.ToUInt16(IniControl.ReadIni("setup", "isMihoyo"));
                return _BiOrMi;
            }
            set
            {
                _BiOrMi = value;
                IniControl.WriteIni("setup", "isMihoyo", Convert.ToString(_BiOrMi));
            }
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
        /// 读ini文件  
        /// </summary>  
        /// <param name="section">节</param>  
        /// <param name="key">键</param>  
        /// <param name="defValue">未读取到值时的默认值</param>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static string ReadIni(string section, string key)
        {
            string IniFilePath = Config.IniGS.gamePath + @"\\Genshin Impact Game\\config.ini";
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, IniFilePath);
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
            string IniFilePath = Config.IniGS.gamePath + @"\\Genshin Impact Game\\config.ini";
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
            string IniFilePath = Config.IniGS.gamePath + @"\\Genshin Impact Game\\config.ini";
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
            string IniFilePath = Config.IniGS.gamePath + @"\\Genshin Impact Game\\config.ini";
            return WritePrivateProfileString(section, key, null, IniFilePath);
        }
    }
    class BOM
    {
        private static ushort _Channel;
        /// <summary>
        /// Channel属性
        /// </summary>
        public static ushort Channel
        {
            get
            {
                _Channel = Convert.ToUInt16(YuanshenIni.ReadIni("General", "channel"));
                return _Channel;
            }
            set
            {
                _Channel = value;
                YuanshenIni.WriteIni("General", "channel", Convert.ToString(_Channel));
            }
        }

        private static ushort _Sub_channel;
        /// <summary>
        /// SUB_Channel属性
        /// </summary>
        public static ushort Sub_channel
        {
            get
            {
                _Sub_channel = Convert.ToUInt16(YuanshenIni.ReadIni("General", "sub_channel"));
                return _Channel;
            }
            set
            {
                _Sub_channel = value;
                YuanshenIni.WriteIni("General", "sub_channel", Convert.ToString(_Sub_channel));
            }
        }
    }
    //
}
