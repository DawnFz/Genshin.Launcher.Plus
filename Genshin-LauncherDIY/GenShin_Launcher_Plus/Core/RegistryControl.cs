using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenShin_Launcher_Plus.Models;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace GenShin_Launcher_Plus.Core
{
    public class RegistryControl
    {
        public string GetFromRegedit(string name, string port)
        {
            UserRegistryModel userRegistry = new();
            userRegistry.Name = name;
            userRegistry.Port = port;
            if (port == "CN")
            {
                userRegistry.MIHOYOSDK_ADL_PROD = Encoding.UTF8.GetString((byte[])Registry.GetValue(@"HKEY_CURRENT_USER\Software\miHoYo\原神", "MIHOYOSDK_ADL_PROD_CN_h3123967166", ""));
                userRegistry.GENERAL_DATA = Encoding.UTF8.GetString((byte[])Registry.GetValue(@"HKEY_CURRENT_USER\Software\miHoYo\原神", "GENERAL_DATA_h2389025596", ""));              
            }
            else if (port == "Global")
            {
                userRegistry.MIHOYOSDK_ADL_PROD = Encoding.UTF8.GetString((byte[])Registry.GetValue(@"HKEY_CURRENT_USER\Software\miHoYo\Genshin Impact", "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810", ""));
                userRegistry.GENERAL_DATA = Encoding.UTF8.GetString((byte[])Registry.GetValue(@"HKEY_CURRENT_USER\Software\miHoYo\Genshin Impact", "GENERAL_DATA_h2389025596", ""));
            }
            return JsonConvert.SerializeObject(userRegistry);
        }
        public void SetToRegedit(string name)
        {
            string file = Path.Combine(Directory.GetCurrentDirectory(), "UserData", name);
            string json = File.ReadAllText(file);
            UserRegistryModel userRegistry = JsonConvert.DeserializeObject<UserRegistryModel>(json);
            if (userRegistry.Port == "CN")
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\miHoYo\原神", "MIHOYOSDK_ADL_PROD_CN_h3123967166", userRegistry.MIHOYOSDK_ADL_PROD);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\miHoYo\原神", "GENERAL_DATA_h2389025596", userRegistry.GENERAL_DATA);
            }
            else if (userRegistry.Port == "Global")
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\miHoYo\Genshin Impact", "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810", userRegistry.MIHOYOSDK_ADL_PROD);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\miHoYo\Genshin Impact", "GENERAL_DATA_h2389025596", userRegistry.GENERAL_DATA);
            }
            else
            {
                MessageBox.Show("账号文件错误！请检测此文件是否为 1.2.4 版本及后续版本保存的账号文件！");
            }
        }
    }
}
