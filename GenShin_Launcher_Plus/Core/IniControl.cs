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
        private String iniFilePath;
        private struct SectionPair
        {
            public String Section;
            public String Key;
        }
        public IniParser(String iniPath = @"Config\Setting.ini")
        {
            TextReader iniFile = null;
            String strLine = null;
            String currentRoot = null;
            String[] keyPair = null;
            iniFilePath = iniPath;
        A: if (File.Exists(iniPath))
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
                                String value = null;
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
                    throw ex;
                }
                finally
                {
                    if (iniFile != null)
                        iniFile.Close();
                }
            }
            else
            {
                AddConfig.CheckIni();
                goto A;
            }
        }

        public String GetSetting(String sectionName, String settingName, int i)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;
            switch (i)
            {
                case 1:
                    if ((String)keyPairs[sectionPair] != "")
                        return (String)keyPairs[sectionPair];
                    else
                        return "1";
                case 2:
                    if ((String)keyPairs[sectionPair] != "")
                        return (String)keyPairs[sectionPair];
                    else
                        return "False";
                default:
                    return (String)keyPairs[sectionPair];
            }

        }

        public void AddSetting(String sectionName, String settingName, String settingValue)
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
            String tmpValue = "";
            String strToSave = "";
            foreach (SectionPair sectionPair in keyPairs.Keys)
            {
                if (!sections.Contains(sectionPair.Section))
                    sections.Add(sectionPair.Section);
            }
            foreach (String section in sections)
            {
                strToSave += ("[" + section + "]\r\n");
                foreach (SectionPair sectionPair in keyPairs.Keys)
                {
                    if (sectionPair.Section == section)
                    {
                        tmpValue = (String)keyPairs[sectionPair];
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
                throw ex;
            }
        }
    }
    class IniControl
    {

        private static string _ReadLang;
        public static string ReadLang
        {
            get
            {
                IniParser parser = new IniParser();
                _ReadLang = parser.GetSetting("setup", "Language", 0);
                return _ReadLang;
            }
            set
            {
                IniParser parser = new IniParser();
                _ReadLang = value;
                parser.AddSetting("setup", "Language", _ReadLang);
                parser.SaveSettings();
            }
        }

        private static string _GamePath;
        public static string GamePath
        {
            get
            {
                IniParser parser = new IniParser();
                _GamePath = parser.GetSetting("setup", "GamePath", 0);
                return _GamePath;
            }
            set
            {
                IniParser parser = new IniParser();
                _GamePath = value;
                parser.AddSetting("setup", "GamePath", _GamePath);
                parser.SaveSettings();
            }
        }

        private static string _Width;
        public static string Width
        {
            get
            {
                IniParser parser = new IniParser();
                _Width = parser.GetSetting("setup", "Width", 1);
                return _Width;
            }
            set
            {
                IniParser parser = new IniParser();
                _Width = value;
                parser.AddSetting("setup", "Width", Convert.ToString(_Width));
                parser.SaveSettings();
            }
        }

        private static string _Height;
        public static string Height
        {
            get
            {
                IniParser parser = new IniParser();
                _Height = parser.GetSetting("setup", "Height", 1);
                return _Height;
            }
            set
            {
                IniParser parser = new IniParser();
                _Height = value;
                parser.AddSetting("setup", "Height", Convert.ToString(_Height));
                parser.SaveSettings();
            }
        }

        private static string _MaxFps;
        public static string MaxFps
        {
            get
            {
                IniParser parser = new IniParser();
                _MaxFps = parser.GetSetting("setup", "MaxFps", 0);
                return _MaxFps;
            }
            set
            {
                IniParser parser = new IniParser();
                _MaxFps = value;
                parser.AddSetting("setup", "MaxFps", _MaxFps);
                parser.SaveSettings();
            }
        }

        private static string _SwitchUser;
        public static string SwitchUser
        {
            get
            {
                IniParser parser = new IniParser();
                _SwitchUser = parser.GetSetting("setup", "SwitchUser", 0);
                return _SwitchUser;
            }
            set
            {
                IniParser parser = new IniParser();
                _SwitchUser = value;
                parser.AddSetting("setup", "SwitchUser", _SwitchUser);
                parser.SaveSettings();
            }
        }

        private static bool _isPopup;
        public static bool isPopup
        {
            get
            {
                IniParser parser = new IniParser();
                _isPopup = Convert.ToBoolean(parser.GetSetting("setup", "isPopup", 2));
                return _isPopup;
            }
            set
            {
                IniParser parser = new IniParser();
                _isPopup = value;
                parser.AddSetting("setup", "isPopup", Convert.ToString(_isPopup));
                parser.SaveSettings();
            }
        }


        private static bool _isWebBg;
        public static bool isWebBg
        {
            get
            {
                IniParser parser = new IniParser();
                _isWebBg = Convert.ToBoolean(parser.GetSetting("setup", "isWebBg", 2));
                return _isWebBg;
            }
            set
            {
                IniParser parser = new IniParser();
                _isWebBg = value;
                parser.AddSetting("setup", "isWebBg", Convert.ToString(_isWebBg));
                parser.SaveSettings();
            }
        }




        private static bool _isClose;
        public static bool isClose
        {
            get
            {
                IniParser parser = new IniParser();
                _isClose = Convert.ToBoolean(parser.GetSetting("setup", "isClose", 2));
                return _isClose;
            }
            set
            {
                IniParser parser = new IniParser();
                _isClose = value;
                parser.AddSetting("setup", "isClose", Convert.ToString(_isClose));
                parser.SaveSettings();
            }
        }

        private static bool _isMainGridHide;
        public static bool isMainGridHide
        {
            get
            {
                IniParser parser = new IniParser();
                _isMainGridHide = Convert.ToBoolean(parser.GetSetting("setup", "isMainGridHide", 2));
                return _isMainGridHide;
            }
            set
            {
                IniParser parser = new IniParser();
                _isMainGridHide = value;
                parser.AddSetting("setup", "isMainGridHide", Convert.ToString(_isMainGridHide));
                parser.SaveSettings();
            }
        }

        private static ushort _FullSize;
        public static ushort FullSize
        {
            get
            {
                IniParser parser = new IniParser();
                _FullSize = Convert.ToUInt16(parser.GetSetting("setup", "FullSize", 1));
                return _FullSize;
            }
            set
            {
                IniParser parser = new IniParser();
                _FullSize = value;
                parser.AddSetting("setup", "FullSize", Convert.ToString(_FullSize));
                parser.SaveSettings();
            }
        }

        private static bool _isUnFPS;
        public static bool isUnFPS
        {
            get
            {
                IniParser parser = new IniParser();
                _isUnFPS = Convert.ToBoolean(parser.GetSetting("setup", "isUnFPS", 2));
                return _isUnFPS;
            }
            set
            {
                IniParser parser = new IniParser();
                _isUnFPS = value;
                parser.AddSetting("setup", "isUnFPS", Convert.ToString(_isUnFPS));
                parser.SaveSettings();
            }
        }


        private static bool _isSwitchUser;
        public static bool isSwitchUser
        {
            get
            {
                IniParser parser = new IniParser();
                _isSwitchUser = Convert.ToBoolean(parser.GetSetting("setup", "isSwitchUser", 2));
                return _isSwitchUser;
            }
            set
            {
                IniParser parser = new IniParser();
                _isSwitchUser = value;
                parser.AddSetting("setup", "isSwitchUser", Convert.ToString(_isSwitchUser));
                parser.SaveSettings();
            }
        }


        private static ushort _isMihoyo;
        public static ushort isMihoyo
        {
            get
            {
                IniParser parser = new IniParser();
                _isMihoyo = Convert.ToUInt16(parser.GetSetting("setup", "isMihoyo", 1));
                return _isMihoyo;
            }
            set
            {
                IniParser parser = new IniParser();
                _isMihoyo = value;
                parser.AddSetting("setup", "isMihoyo", Convert.ToString(_isMihoyo));
                parser.SaveSettings();
            }
        }

        public static void EXEname(string value)
        {
            IniParser parser = new IniParser();
            parser.AddSetting("setup", "LauncherPlusName", value);
            parser.SaveSettings();
        }

        private static ushort _Channel;
        public static ushort Channel
        {
            get
            {
                IniParser parser = new IniParser(Path.Combine(GamePath, "Config.ini"));
                _Channel = Convert.ToUInt16(parser.GetSetting("General", "channel", 1));
                return _Channel; 
            }
            set
            {
                IniParser parser = new IniParser(Path.Combine(GamePath, "Config.ini"));
                _Channel = value;
                parser.AddSetting("General", "channel", Convert.ToString(_Channel));
                parser.SaveSettings();
            }
        }

        private static ushort _Sub_channel;
        public static ushort Sub_channel
        {
            get
            {
                IniParser parser = new IniParser(Path.Combine(GamePath, "Config.ini"));
                _Sub_channel = Convert.ToUInt16(parser.GetSetting("General", "sub_channel", 1));
                return _Sub_channel;
            }
            set
            {
                IniParser parser = new IniParser(Path.Combine(GamePath, "Config.ini"));
                _Sub_channel = value;
                parser.AddSetting("General", "sub_channel", Convert.ToString(_Sub_channel));
                parser.SaveSettings();
            }
        }

        private static string _Cps;
        public static string Cps
        {
            get
            {
                IniParser parser = new IniParser(Path.Combine(GamePath, "Config.ini"));
                _Cps = parser.GetSetting("General", "cps", 0);
                return _Cps;
            }
            set
            {
                IniParser parser = new IniParser(Path.Combine(GamePath, "Config.ini"));
                _Cps = value;
                parser.AddSetting("General", "cps", _Cps);
                parser.SaveSettings();
            }
        }

        private static bool _UserXunkongWallpaper;
        public static bool UserXunkongWallpaper
        {
            get
            {
                IniParser parser = new IniParser();
                _UserXunkongWallpaper = Convert.ToBoolean(parser.GetSetting("setup", "UserXunkongWallpaper", 2));
                return _UserXunkongWallpaper;
            }
            set
            {
                IniParser parser = new IniParser();
                _UserXunkongWallpaper = value;
                parser.AddSetting("setup", "UserXunkongWallpaper", Convert.ToString(_UserXunkongWallpaper));
                parser.SaveSettings();
            }
        }
    }
    class AddConfig
    {
        public static void NewIni()
        {
            try
            {
                FilesControl utils = new FilesControl();
                utils.FileWriter("StaticRes/Config.ini", @"Config/Setting.ini");
            }
            catch
            {
            }
        }

        public static void CheckIni()
        {
            try
            {
                if (Directory.Exists(@"Config") == false)
                {
                    Directory.CreateDirectory("Config");
                    NewIni();
                }
                else if (File.Exists(@"Config\Setting.ini") == false)
                {
                    NewIni();
                }
                if (Directory.Exists(@"UserData") == false)
                {
                    Directory.CreateDirectory("UserData");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Setting.ini创建失败，原因:{ex}");
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
