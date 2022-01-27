using System;
using System.Collections.Generic;
using System.Text;

namespace GenShin_Launcher_Plus.Models
{
    public class SettingsIniModel
    {
        public string SwitchPort { get; set; }
        public string IsSwitchUser { get; set; }
        public string SwitchUser { get; set; }
        public string GamePath { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string MaxFps { get; set; }
        public bool isPopup { get; set; }
        public bool isWebBg { get; set; }
        public bool isClose { get; set; }
        public bool isMainGridHide  { get; set; }
        public bool isUnFPS { get; set; }
        public ushort FullSize { get; set; }
        public ushort isMihoyo  { get; set; }
    }
}
