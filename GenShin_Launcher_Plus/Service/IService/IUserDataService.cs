using GenShin_Launcher_Plus.Models;
using System.Collections.Generic;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface IUserDataService
    {
        List<UserListModel> ReadUserList();
    }
}
