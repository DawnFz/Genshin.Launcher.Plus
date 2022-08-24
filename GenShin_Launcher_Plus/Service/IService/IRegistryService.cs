namespace GenShin_Launcher_Plus.Service.IService
{
    public interface IRegistryService
    {
        /// <summary>
        /// 从注册表获取数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        string? GetFromRegistry(string name, string port,bool isSaveGameConfig);

        /// <summary>
        /// 写入数据到注册表
        /// </summary>
        /// <param name="name"></param>
        public void SetToRegistry(string name);
    }
}
