namespace GenShin_Launcher_Plus.Service.IService
{
    public interface IRegistryService
    {
        /// <summary>
        /// 从注册表获取数据
        /// </summary>
        string? GetFromRegistry(string name, string port);

        /// <summary>
        /// 写入数据到注册表
        /// </summary>
        public void SetToRegistry(string name);
    }
}
