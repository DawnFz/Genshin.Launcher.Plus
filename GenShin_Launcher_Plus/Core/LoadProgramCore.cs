using GenShin_Launcher_Plus.ViewModels;
using GenShin_Launcher_Plus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace GenShin_Launcher_Plus.Core
{
    public static class LoadProgramCore
    {
        public static bool ReadLangList()
        {
            if (IniControl.ReadLang == "" || IniControl.ReadLang == null)
            {
                IniControl.ReadLang = "Lang_CN.json";
            }
            //获取网络上的语言列表

            FilesControl fc = new();
            string json = fc.GetJsonFromHtml("LangList");
            if (IniControl.ReadLang != "Lang_CN.json")
            {
                if (json != null && json != "")
                {
                    MainBase.langlist = JsonConvert.DeserializeObject<List<LanguageListsModel>>(json);
                }
                else
                {
                    MessageBox.Show("Error: Language pack list is empty, please check the network! !");
                    Environment.Exit(0);
                }
            }
            else
            {
                if (json != null && json != "")
                {
                    MainBase.langlist = JsonConvert.DeserializeObject<List<LanguageListsModel>>(json);
                }
                else
                {
                    MessageBox.Show("错误：语言包列表为空，请检查网络！！");
                    Environment.Exit(0);
                }
            }
            //从JSON字符串生成集合对象用于存放语言包列表

            if (!File.Exists($@"Config/{IniControl.ReadLang}"))
            {
                LoadLanguageCore(IniControl.ReadLang);
            }

            //在检查语言包更新前，提前将本地存放的语言包Json序列化到实体类LanguagesModel中
            string oldfile = $@"Config/{IniControl.ReadLang}";
            string oldjson = File.ReadAllText(oldfile);
            if (oldjson != null && oldjson != "")
            {
                MainBase.lang = JsonConvert.DeserializeObject<LanguagesModel>(oldjson);
            }
            else
            {
                LoadLanguageCore(IniControl.ReadLang);
            }
            //判断语言包的版本号，返回一个布尔类型的值给MainWindow对象创建时判断是否执行LoadLanguageCore方法更新语言文件
            foreach (LanguageListsModel ll in MainBase.langlist)
            {
                if (MainBase.lang.Languages == ll.LangName)
                {
                    if (MainBase.lang.LangVersion != ll.LangVersion)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void LoadLanguageCore(string langfile)
        {
            /*
             * 此处的langID为博客园的Html静态页面的文件名
             * 例如 https://www.cnblogs.com/DawnFz/p/15990791.html 
             */
            string langID = "0001";

            /*
             * 这里循环集合里的语言包文件名对比从传入的文件名是否相等，相等的话取出ID
             * 然后使用这个ID去读取对应的Html到文本并且取里面的Json内容保存为文件
             * 这么做虽然有损性能，但是大大节省了服务器经费，博客园静态Html文章页yyds
             * 将取到的Html内容中的Json分离出来的方法见[Core.FilesControl]类
             */
            foreach (LanguageListsModel llnew in MainBase.langlist)
            {
                if (llnew.LangFileName == langfile)
                {
                    langID = llnew.LangID;
                }
            }
            FilesControl fc = new();
            string tmp = fc.GetLangFromHtml(langID);
            File.WriteAllText($@"Config/{langfile}", tmp, Encoding.UTF8);
            string file = $@"Config/{langfile}";
            string json = File.ReadAllText(file);
            if (json != null && json != "")
            {
                MainBase.lang = JsonConvert.DeserializeObject<LanguagesModel>(json);
            }




        }
        public static void LoadUpdateCore()
        {
            FilesControl fc = new();
            if (IniControl.ReadLang != "Lang_CN.json")
            {
                string json = fc.GetJsonFromHtml("UpdateGlobal");
                MainBase.update = JsonConvert.DeserializeObject<UpdateContentModel>(json);
            }
            else
            {
                string json = fc.GetJsonFromHtml("UpdateCN");
                MainBase.update = JsonConvert.DeserializeObject<UpdateContentModel>(json);
            }
        }
    }
}
