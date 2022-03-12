using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace GenShin_Launcher_Plus.Core
{
    public class IniParser
    {
        private Hashtable keyPairs = new Hashtable();
        private string iniFilePath;
        private struct SectionPair
        {
            public string Section;
            public string Key;
        }
        public IniParser(string iniPath = @"Config\Setting.ini")
        {
            TextReader iniFile = null;
            string strLine = null;
            string currentRoot = null;
            string[] keyPair = null;
            iniFilePath = iniPath;
            if (File.Exists(iniPath))
            {
                try
                {
                    iniFile = new StreamReader(iniPath);
                    strLine = iniFile.ReadLine();
                    while (strLine != null)
                    {
                        strLine = strLine.Trim();
                        if (strLine != "")
                        {
                            if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                            {
                                currentRoot = strLine.Substring(1, strLine.Length - 2);
                            }
                            else
                            {
                                keyPair = strLine.Split(new char[] { '=' }, 2);
                                SectionPair sectionPair;
                                string value = null;
                                if (currentRoot == null)
                                    currentRoot = "ROOT";
                                sectionPair.Section = currentRoot;
                                sectionPair.Key = keyPair[0];
                                if (keyPair.Length > 1)
                                    value = keyPair[1];
                                keyPairs.Add(sectionPair, value);
                            }
                        }
                        strLine = iniFile.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error : {ex.Message}");              
                }
                finally
                {
                    if (iniFile != null)
                        iniFile.Close();
                }
            }
            else
            {
                if (!Directory.Exists(@"Config"))
                {
                    Directory.CreateDirectory("Config");
                }
            }
        }

        public string GetSetting(string sectionName, string settingName, int i)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;
            switch (i)
            {
                case 1:
                    if ((string)keyPairs[sectionPair] != "")
                        return (string)keyPairs[sectionPair];
                    else
                        return "1";
                case 2:
                    if ((string)keyPairs[sectionPair] != "")
                        return (string)keyPairs[sectionPair];
                    else
                        return "False";
                default:
                    return (string)keyPairs[sectionPair];
            }

        }

        public void AddSetting(string sectionName, string settingName, string settingValue)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;
            if (keyPairs.ContainsKey(sectionPair))
                keyPairs.Remove(sectionPair);
            keyPairs.Add(sectionPair, settingValue);
        }

        public void SaveSettings()
        {
            ArrayList sections = new ArrayList();
            string tmpValue = "";
            string strToSave = "";
            foreach (SectionPair sectionPair in keyPairs.Keys)
            {
                if (!sections.Contains(sectionPair.Section))
                    sections.Add(sectionPair.Section);
            }
            foreach (string section in sections)
            {
                strToSave += ("[" + section + "]\r\n");
                foreach (SectionPair sectionPair in keyPairs.Keys)
                {
                    if (sectionPair.Section == section)
                    {
                        tmpValue = (string)keyPairs[sectionPair];
                        if (tmpValue != null)
                            tmpValue = "=" + tmpValue;
                        strToSave += (sectionPair.Key + tmpValue + "\r\n");
                    }
                }
                strToSave += "\r\n";
            }
            try
            {
                TextWriter tw = new StreamWriter(iniFilePath);
                tw.Write(strToSave);
                tw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}");
            }
        }
    }
    public class IniControl
    {
        public IniControl()
        {
            parser = new();
            gameparser = new(Path.Combine(GamePath == null ? "" : GamePath, "Config.ini"));
        }
        IniParser parser { get; set; }
        IniParser gameparser { get; set; }

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
            }
        }




        private bool _isClose;
        public bool isClose
        {
            get
            {
                _isClose = Convert.ToBoolean(parser.GetSetting("setup", "isClose", 2));
                return _isClose;
            }
            set
            {
                _isClose = value;
                parser.AddSetting("setup", "isClose", Convert.ToString(_isClose));
                parser.SaveSettings();
            }
        }

        private bool _isMainGridHide;
        public bool isMainGridHide
        {
            get
            {
                _isMainGridHide = Convert.ToBoolean(parser.GetSetting("setup", "isMainGridHide", 2));
                return _isMainGridHide;
            }
            set
            {
                _isMainGridHide = value;
                parser.AddSetting("setup", "isMainGridHide", Convert.ToString(_isMainGridHide));
                parser.SaveSettings();
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
            }
        }


        private bool _isSwitchUser;
        public bool isSwitchUser
        {
            get
            {
                _isSwitchUser = Convert.ToBoolean(parser.GetSetting("setup", "isSwitchUser", 2));
                return _isSwitchUser;
            }
            set
            {
                _isSwitchUser = value;
                parser.AddSetting("setup", "isSwitchUser", Convert.ToString(_isSwitchUser));
                parser.SaveSettings();
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
            }
        }

        public void EXEname(string value)
        {
            parser.AddSetting("setup", "LauncherPlusName", value);
            parser.SaveSettings();
        }

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
            }
        }
    }
    public class CommandLineBuilder
    {
        readonly Dictionary<string, string> _options = new Dictionary<string, string>();
        public void AddOption(string name, string value)
        {
            _options.Add(name, value);
        }
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var e in _options)
            {
                s.Append(" ");
                s.Append(e.Key);
                s.Append(' ');
                s.Append(e.Value);
                s.Append(' ');
            }
            return s.ToString();
        }
    }
}
