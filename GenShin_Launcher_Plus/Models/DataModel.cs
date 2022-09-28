using System;
using System.IO;
using System.Windows;

namespace GenShin_Launcher_Plus.Core
{
    public class DataModel
    {
        public DataModel()
        {
            parser = new(@"Config\Setting.ini");
            gameparser = new(Path.Combine(GamePath ?? "", "Config.ini"));
        }
        IniParser parser { get; set; }
        IniParser gameparser { get; set; }

        /// <summary>
        /// 对程序目录下Config中Setting.ini的操作
        /// </summary>
        private string _ReadLang;
        public string ReadLang
        {
            get
            {
                _ReadLang = parser.GetSetting("setup", "Language", 0);
                return _ReadLang;
            }
            set
            {
                _ReadLang = value;
                parser.AddSetting("setup", "Language", _ReadLang);
            }
        }

        private string _GamePath;
        public string GamePath
        {
            get
            {
                _GamePath = parser.GetSetting("setup", "GamePath", 0);
                return _GamePath;
            }
            set
            {
                _GamePath = value;
                parser.AddSetting("setup", "GamePath", _GamePath);
            }
        }

        private string _BackgroundPath;
        public string BackgroundPath
        {
            get
            {
                _BackgroundPath = parser.GetSetting("setup", "BackgroundPath", 0);
                return _BackgroundPath;
            }
            set
            {
                _BackgroundPath = value;
                parser.AddSetting("setup", "BackgroundPath", _BackgroundPath);
            }
        }

        private string _Width;
        public string Width
        {
            get
            {
                _Width = parser.GetSetting("setup", "Width", 1);
                return _Width;
            }
            set
            {
                _Width = value;
                parser.AddSetting("setup", "Width", Convert.ToString(_Width));
            }
        }

        private string _Height;
        public string Height
        {
            get
            {
                _Height = parser.GetSetting("setup", "Height");
                return _Height;
            }
            set
            {
                _Height = value;
                parser.AddSetting("setup", "Height", Convert.ToString(_Height));
            }
        }

        private double _MainWidth;
        public double MainWidth
        {
            get
            {
                _MainWidth = Convert.ToDouble(parser.GetSetting("setup", "MainWidth", 1));
                if (_MainWidth == 1)
                {
                    return 1280;
                }
                return _MainWidth;
            }
            set
            {
                _MainWidth = value;
                parser.AddSetting("setup", "MainWidth", Convert.ToString(_MainWidth));
            }
        }

        private double _MainHeight;
        public double MainHeight
        {
            get
            {
                _MainHeight = Convert.ToDouble(parser.GetSetting("setup", "MainHeight", 1));
                if (_MainHeight == 1)
                {
                    return 730;
                }
                return _MainHeight;
            }
            set
            {
                _MainHeight = value;
                parser.AddSetting("setup", "MainHeight", Convert.ToString(_MainHeight));
            }
        }


        private string _ImageDate;
        public string ImageDate
        {
            get
            {
                _ImageDate = parser.GetSetting("setup", "ImageDate", 1);
                return _ImageDate;
            }
            set
            {
                _ImageDate = value;
                parser.AddSetting("setup", "ImageDate", Convert.ToString(_ImageDate));
                SaveDataToFile();
            }
        }

        private string _MaxFps;
        public string MaxFps
        {
            get
            {
                _MaxFps = parser.GetSetting("setup", "MaxFps", 0);
                return _MaxFps;
            }
            set
            {
                _MaxFps = value;
                parser.AddSetting("setup", "MaxFps", _MaxFps);
            }
        }

        private string _SwitchUser;
        public string SwitchUser
        {
            get
            {
                _SwitchUser = parser.GetSetting("setup", "SwitchUser", 0);
                return _SwitchUser;
            }
            set
            {
                _SwitchUser = value;
                parser.AddSetting("setup", "SwitchUser", _SwitchUser);
            }
        }

        private string _ImagePid;
        public string ImagePid
        {
            get
            {
                _ImagePid = parser.GetSetting("setup", "ImagePid", 0);
                return _ImagePid;
            }
            set
            {
                _ImagePid = value;
                parser.AddSetting("setup", "ImagePid", _ImagePid);
            }
        }

        private bool _IsPopup;
        public bool IsPopup
        {
            get
            {
                _IsPopup = Convert.ToBoolean(parser.GetSetting("setup", "isPopup", 2));
                return _IsPopup;
            }
            set
            {
                _IsPopup = value;
                parser.AddSetting("setup", "isPopup", Convert.ToString(_IsPopup));
            }
        }


        private bool _IsWebBg;
        public bool IsWebBg
        {
            get
            {
                _IsWebBg = Convert.ToBoolean(parser.GetSetting("setup", "isWebBg", 2));
                return _IsWebBg;
            }
            set
            {
                _IsWebBg = value;
                parser.AddSetting("setup", "isWebBg", Convert.ToString(_IsWebBg));
            }
        }

        private bool _IsLocalDailyImage;
        public bool IsLocalDailyImage
        {
            get
            {
                _IsLocalDailyImage = Convert.ToBoolean(parser.GetSetting("setup", "IsLocalDailyImage", 2));
                return _IsLocalDailyImage;
            }
            set
            {
                _IsLocalDailyImage = value;
                parser.AddSetting("setup", "IsLocalDailyImage", Convert.ToString(_IsLocalDailyImage));
            }
        }

        private ushort _FullSize;
        public ushort FullSize
        {
            get
            {
                _FullSize = Convert.ToUInt16(parser.GetSetting("setup", "FullSize", 1));
                return _FullSize;
            }
            set
            {
                _FullSize = value;
                parser.AddSetting("setup", "FullSize", Convert.ToString(_FullSize));
            }
        }

        private bool _IsUnFPS;
        public bool IsUnFPS
        {
            get
            {
                _IsUnFPS = Convert.ToBoolean(parser.GetSetting("setup", "isUnFPS", 2));
                return _IsUnFPS;
            }
            set
            {
                _IsUnFPS = value;
                parser.AddSetting("setup", "isUnFPS", Convert.ToString(_IsUnFPS));
            }
        }

        private bool _IsCloseUpdate;
        public bool IsCloseUpdate
        {
            get
            {
                _IsCloseUpdate = Convert.ToBoolean(parser.GetSetting("setup", "IsCloseUpdate", 2));
                return _IsCloseUpdate;
            }
            set
            {
                _IsCloseUpdate = value;
                parser.AddSetting("setup", "IsCloseUpdate", Convert.ToString(_IsCloseUpdate));
            }
        }

        private bool _IsRunThenClose;
        public bool IsRunThenClose
        {
            get
            {
                _IsRunThenClose = Convert.ToBoolean(parser.GetSetting("setup", "IsRunThenClose", 2));
                return _IsRunThenClose;
            }
            set
            {
                _IsRunThenClose = value;
                parser.AddSetting("setup", "IsRunThenClose", Convert.ToString(_IsRunThenClose));
            }
        }

        private bool _UseXunkongWallpaper;
        public bool UseXunkongWallpaper
        {
            get
            {
                _UseXunkongWallpaper = Convert.ToBoolean(parser.GetSetting("setup", "UseXunkongWallpaper", 2));
                return _UseXunkongWallpaper;
            }
            set
            {
                _UseXunkongWallpaper = value;
                parser.AddSetting("setup", "UseXunkongWallpaper", Convert.ToString(_UseXunkongWallpaper));
            }
        }

        public void EXEname(string value)
        {
            parser.AddSetting("setup", "LauncherPlusName", value);
            parser.SaveSettings();
        }

        /// <summary>
        /// 对游戏目录下的Config.ini操作
        /// </summary>
        private ushort _Channel;
        public ushort Channel
        {
            get
            {
                _Channel = Convert.ToUInt16(gameparser.GetSetting("General", "channel", 1));
                return _Channel;
            }
            set
            {
                _Channel = value;
                gameparser.AddSetting("General", "channel", Convert.ToString(_Channel));
            }
        }

        private ushort _Sub_channel;
        public ushort Sub_channel
        {
            get
            {
                _Sub_channel = Convert.ToUInt16(gameparser.GetSetting("General", "sub_channel", 1));
                return _Sub_channel;
            }
            set
            {
                _Sub_channel = value;
                gameparser.AddSetting("General", "sub_channel", Convert.ToString(_Sub_channel));
            }
        }

        private string _Cps;
        public string Cps
        {
            get
            {
                _Cps = gameparser.GetSetting("General", "cps", 0);
                return _Cps;
            }
            set
            {
                _Cps = value;
                gameparser.AddSetting("General", "cps", _Cps);
            }
        }

        public void SaveDataToFile()
        {
            parser.SaveSettings();
            gameparser.SaveSettings();
        }
    }

}
