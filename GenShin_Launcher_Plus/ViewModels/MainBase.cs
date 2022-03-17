using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// 全局调用的可更新UI的静态对象
    /// </summary>
    public class MainBase
    {
        public static IniModel IniModel = new();
        public static List<LanguageListsModel> langlist = new();
        public static NoticeOverAllBase noab = new();
        public static LanguagesModel lang = new();
        public static UpdateContentModel update = new();
    }
}
