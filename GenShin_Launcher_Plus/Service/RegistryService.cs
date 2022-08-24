using Microsoft.Win32;
using System.Text;
using GenShin_Launcher_Plus.Models;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using GenShin_Launcher_Plus.Service.IService;

namespace GenShin_Launcher_Plus.Service
{
    /// <summary>
    /// 对原神注册表的操作
    /// </summary>
    public class RegistryService : IRegistryService
    {
        private const string CnPathKey = @"HKEY_CURRENT_USER\Software\miHoYo\原神";
        private const string CnSdkKey = "MIHOYOSDK_ADL_PROD_CN_h3123967166";
        private const string GlobalPathKey = @"HKEY_CURRENT_USER\Software\miHoYo\Genshin Impact";
        private const string GlobalSdkKey = "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810";
        private const string DataKey = "GENERAL_DATA_h2389025596";

        /// <summary>
        /// 获取注册表内容
        /// </summary>
        /// <param name="name"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public string? GetFromRegistry(string name, string port, bool isSaveGameConfig)
        {
            RegistryModel userRegistry = new();
            userRegistry.Name = name;
            userRegistry.Port = port;
            try
            {
                if (port == "CN")
                {
                    object? cnsdk = Registry.GetValue(CnPathKey, CnSdkKey, string.Empty);
                    userRegistry.MIHOYOSDK_ADL_PROD = Encoding.UTF8.GetString((byte[])cnsdk);
                    if (isSaveGameConfig)
                    {
                        object? data = Registry.GetValue(CnPathKey, DataKey, string.Empty);
                        userRegistry.GENERAL_DATA = Encoding.UTF8.GetString((byte[])data);
                    }
                }
                else if (port == "Global")
                {
                    object? globalsdk = Registry.GetValue(GlobalPathKey, GlobalSdkKey, string.Empty);
                    userRegistry.MIHOYOSDK_ADL_PROD = Encoding.UTF8.GetString((byte[])globalsdk);
                    if (isSaveGameConfig)
                    {
                        object? data = Registry.GetValue(GlobalPathKey, DataKey, string.Empty);
                        userRegistry.GENERAL_DATA = Encoding.UTF8.GetString((byte[])data);
                    }
                }
            }
            catch
            {
                MessageBox.Show(App.Current.Language.SaveAccountErr);
            }
            return JsonConvert.SerializeObject(userRegistry);
        }

        /// <summary>
        /// 更新注册表内容
        /// </summary>
        /// <param name="name"></param>
        public void SetToRegistry(string name)
        {
            string file = Path.Combine(Directory.GetCurrentDirectory(), "UserData", name);
            string json = File.ReadAllText(file);
            RegistryModel userRegistry = JsonConvert.DeserializeObject<RegistryModel>(json);
            if (userRegistry.MIHOYOSDK_ADL_PROD != null &&
                userRegistry.MIHOYOSDK_ADL_PROD != "null" &&
                userRegistry.MIHOYOSDK_ADL_PROD != string.Empty)
                if (userRegistry.Port == "CN")
                {
                    Registry.SetValue(CnPathKey, CnSdkKey, Encoding.UTF8.GetBytes(userRegistry.MIHOYOSDK_ADL_PROD));
                    if (userRegistry.GENERAL_DATA != null && userRegistry.GENERAL_DATA != "null" && userRegistry.GENERAL_DATA != string.Empty)
                    {
                        Registry.SetValue(CnPathKey, DataKey, Encoding.UTF8.GetBytes(userRegistry.GENERAL_DATA));
                    }
                }
                else if (userRegistry.Port == "Global")
                {
                    Registry.SetValue(GlobalPathKey, GlobalSdkKey, Encoding.UTF8.GetBytes(userRegistry.MIHOYOSDK_ADL_PROD));
                    if (userRegistry.GENERAL_DATA != null && userRegistry.GENERAL_DATA != "null" && userRegistry.GENERAL_DATA != string.Empty)
                    {
                        Registry.SetValue(GlobalPathKey, DataKey, Encoding.UTF8.GetBytes(userRegistry.GENERAL_DATA));
                    }
                }
                else
                {
                    MessageBox.Show("Error : The file does not support ! !");
                }
            else
            {
                MessageBox.Show("Error : The file does not support ! !");
            }
        }
    }
}
