using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.IO;

namespace GenShin_Launcher_Plus.Core
{
    public class IniModel : ObservableObject
    {
        public IniModel()
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
                parser.SaveSettings();
                SetProperty(ref _ReadLang,value);
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
                parser.SaveSettings();
                SetProperty(ref _GamePath, value);
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
                parser.SaveSettings();
                SetProperty(ref _Width, value);
            }
        }

        private string _Height;
        public string Height
        {
            get
            {
                _Height = parser.GetSetting("setup", "Height", 1);
                return _Height;
            }
            set
            {
                _Height = value;
                parser.AddSetting("setup", "Height", Convert.ToString(_Height));
                parser.SaveSettings();
                SetProperty(ref _Height, value);
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
                parser.SaveSettings();
                SetProperty(ref _MaxFps, value);
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
                parser.SaveSettings();
                SetProperty(ref _SwitchUser, value);
            }
        }

        private bool _isPopup;
        public bool isPopup
        {
            get
            {
                _isPopup = Convert.ToBoolean(parser.GetSetting("setup", "isPopup", 2));
                return _isPopup;
            }
            set
            {
                _isPopup = value;
                parser.AddSetting("setup", "isPopup", Convert.ToString(_isPopup));
                parser.SaveSettings();
                SetProperty(ref _isPopup, value);
            }
        }


        private bool _isWebBg;
        public bool isWebBg
        {
            get
            {
                _isWebBg = Convert.ToBoolean(parser.GetSetting("setup", "isWebBg", 2));
                return _isWebBg;
            }
            set
            {
                _isWebBg = value;
                parser.AddSetting("setup", "isWebBg", Convert.ToString(_isWebBg));
                parser.SaveSettings();
                SetProperty(ref _isWebBg, value);
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
                parser.SaveSettings();
                SetProperty(ref _FullSize, value);
            }
        }

        private bool _isUnFPS;
        public bool isUnFPS
        {
            get
            {
                _isUnFPS = Convert.ToBoolean(parser.GetSetting("setup", "isUnFPS", 2));
                return _isUnFPS;
            }
            set
            {
                _isUnFPS = value;
                parser.AddSetting("setup", "isUnFPS", Convert.ToString(_isUnFPS));
                parser.SaveSettings();
                SetProperty(ref _isUnFPS, value);
            }
        }

        private ushort _isMihoyo;
        public ushort isMihoyo
        {
            get
            {
                _isMihoyo = Convert.ToUInt16(parser.GetSetting("setup", "isMihoyo", 1));
                return _isMihoyo;
            }
            set
            {
                _isMihoyo = value;
                parser.AddSetting("setup", "isMihoyo", Convert.ToString(_isMihoyo));
                parser.SaveSettings();
                SetProperty(ref _isMihoyo, value);
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
                parser.SaveSettings();
                SetProperty(ref _UseXunkongWallpaper, value);
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
                gameparser.SaveSettings();
                SetProperty(ref _Channel, value);
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
                gameparser.SaveSettings();
                SetProperty(ref _Sub_channel, value);
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
                gameparser.SaveSettings();
                SetProperty(ref _Cps, value);
            }
        }
    }

}
