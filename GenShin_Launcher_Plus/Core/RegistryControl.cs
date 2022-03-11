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
        private const string CnPathKey = @"HKEY_CURRENT_USER\Software\miHoYo\原神";
        private const string CnSdkKey = "MIHOYOSDK_ADL_PROD_CN_h3123967166";
        private const string GlobalPathKey = @"HKEY_CURRENT_USER\Software\miHoYo\Genshin Impact";
        private const string GlobalSdkKey = "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810";
        private const string DataKey = "GENERAL_DATA_h2389025596";

        public string GetFromRegedit(string name, string port)
        {
            UserRegistryModel userRegistry = new();
            userRegistry.Name = name;
            userRegistry.Port = port;
            if (port == "CN")
            {
                object? cnsdk = Registry.GetValue(CnPathKey, CnSdkKey, string.Empty);
                object? data = Registry.GetValue(CnPathKey, DataKey, string.Empty);
                userRegistry.MIHOYOSDK_ADL_PROD = Encoding.UTF8.GetString((byte[])cnsdk);
                userRegistry.GENERAL_DATA = Encoding.UTF8.GetString((byte[])data);
            }
            else if (port == "Global")
            {
                object? globalsdk = Registry.GetValue(GlobalPathKey, GlobalSdkKey, string.Empty);
                object? data = Registry.GetValue(GlobalPathKey, DataKey, string.Empty);
                userRegistry.MIHOYOSDK_ADL_PROD = Encoding.UTF8.GetString((byte[])globalsdk);
                userRegistry.GENERAL_DATA = Encoding.UTF8.GetString((byte[])data);
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
                Registry.SetValue(CnPathKey, CnSdkKey, Encoding.UTF8.GetBytes(userRegistry.MIHOYOSDK_ADL_PROD));
                Registry.SetValue(CnPathKey, DataKey, Encoding.UTF8.GetBytes(userRegistry.GENERAL_DATA));
            }
            else if (userRegistry.Port == "Global")
            {
                Registry.SetValue(GlobalPathKey, GlobalSdkKey, Encoding.UTF8.GetBytes(userRegistry.MIHOYOSDK_ADL_PROD));
                Registry.SetValue(GlobalPathKey, DataKey, Encoding.UTF8.GetBytes(userRegistry.GENERAL_DATA));
            }
            else
            {
                MessageBox.Show("Error : The file does not support ! !");
            }
        }
    }
}
