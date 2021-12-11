using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace GenShin_LauncherDIY
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
                throw new FileNotFoundException("Unable to locate " + iniPath);
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

        private static bool _isAutoSize;
        public static bool isAutoSize
        {
            get
            {
                IniParser parser = new IniParser();
                _isAutoSize = Convert.ToBoolean(parser.GetSetting("setup", "isAutoSize", 2));
                return _isAutoSize;
            }
            set
            {
                IniParser parser = new IniParser();
                _isAutoSize = value;
                parser.AddSetting("setup", "isAutoSize", Convert.ToString(_isAutoSize));
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

        public static void Channel(string value, string gameConfig)
        {
            IniParser parser = new IniParser(gameConfig);
            parser.AddSetting("General", "channel", value);
            parser.SaveSettings();
        }

        public static void Sub_channel(string value, string gameConfig)
        {
            IniParser parser = new IniParser(gameConfig);
            parser.AddSetting("General", "sub_channel", value);
            parser.SaveSettings();
        }

        public static void Cps(string value, string gameConfig)
        {
            IniParser parser = new IniParser(gameConfig);
            parser.AddSetting("General", "cps", value);
            parser.SaveSettings();
        }
    }
    class AddConfig
    {
        public static void NewIni()
        {
            try
            {
                UtilsTools utils = new UtilsTools();
                utils.FileWriter("Res/Config.ini", @"Config/Setting.ini");
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
            catch
            {
            }
        }
    }













}
