using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.Service.IService;
using System.Collections.Generic;
using System.IO;

namespace GenShin_Launcher_Plus.Service
{
    public class UserDataService : IUserDataService
    {
        public List<UserListModel> ReadUserList()
        {
            List<UserListModel> list = new();
            DirectoryInfo TheFolder = new(@"UserData");
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                list.Add(new UserListModel { UserName = NextFile.Name });
            }
            return list;
        }
    }
}