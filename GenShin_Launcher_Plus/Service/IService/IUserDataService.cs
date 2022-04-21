using GenShin_Launcher_Plus.Models;
using System.Collections.Generic;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface IUserDataService
    {
        /// <summary>
        /// 读取用户数据文件到List
        /// </summary>
        /// <returns></returns>
        List<UserListModel> ReadUserList();
    }
}
