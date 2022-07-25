using GenShin_Launcher_Plus.Service.IService;
using GenShin_Launcher_Plus.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GenShin_Launcher_Plus.Service
{
    internal class ConvertService : IConvertService
    {
        private const string CN_DIRECTORY = "CnFile";
        private const string GLOBAL_DIRECTORY = "GlobalFile";
        private const string YUANSHEN_DATA = "YuanShen_Data";
        private const string GENSHINIMPACT_DATA = "GenshinImpact_Data";
        private const string YUANSHEN_EXE = "YuanShen.exe";
        private const string GENSHINIMPACT_EXE = "GenshinImpact.exe";

        private string GamePath { get; set; }
        private string Scheme { get; set; }
        private string GameSource { get; set; }
        private string GameDest { get; set; }
        private string CurrentPath { get; set; }
        private string ReplaceSourceDirectory { get; set; }
        private List<string> GameFileList { get; set; }
        public ConvertService()
        {
            GameFileList = new List<string>();
            GamePath = App.Current.DataModel.GamePath;
            CurrentPath = Environment.CurrentDirectory;
        }
        /// <summary>
        /// 检查Pkg版本
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckPackageVersion(string scheme, SettingsPageViewModel vm)
        {
            string pkgfile = App.Current.UpdateObject.PkgVersion;
            if (!File.Exists($"{scheme}/{pkgfile}"))
            {
                vm.ConvertingLog = $"{App.Current.Language.NewPkgVer} : [{pkgfile}]\r\n";
                ProcessStartInfo info = new()
                {
                    FileName = "https://pan.baidu.com/s/1-5zQoVfE7ImdXrn8OInKqg",
                    UseShellExecute = true,
                };
                Process.Start(info);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 异步转换游戏任务
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ConvertGameFileAsync(SettingsPageViewModel vm)
        {
            Scheme = GetCurrentSchemeName();
            GameSource = Scheme == CN_DIRECTORY ? YUANSHEN_DATA : GENSHINIMPACT_DATA;
            GameDest = Scheme == CN_DIRECTORY ? GENSHINIMPACT_DATA : YUANSHEN_DATA;
            ReplaceSourceDirectory = Scheme == CN_DIRECTORY ? GLOBAL_DIRECTORY : CN_DIRECTORY;

            await GetFiles(Path.Combine(CurrentPath, ReplaceSourceDirectory));
            await ReplaceGameFiles(vm);

        }

        /// <summary>
        /// 获取所有文件加入到清单
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public async Task GetFiles(string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string temp = file.FullName.Replace(Path.Combine(Environment.CurrentDirectory, ReplaceSourceDirectory), "");
                GameFileList.Add(temp);
            }
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            if (dirs.Length > 0)
            {
                foreach (DirectoryInfo dir in dirs)
                {
                    GetFiles(dir.FullName);
                }
            }
        }

        public string GetCurrentSchemeName()
        {
            if (File.Exists(Path.Combine(GamePath, YUANSHEN_EXE)))
            {
                return CN_DIRECTORY;
            }
            else if (File.Exists(Path.Combine(GamePath, GENSHINIMPACT_EXE)))
            {
                return GLOBAL_DIRECTORY;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 替换游戏文件逻辑
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ReplaceGameFiles(SettingsPageViewModel vm)
        {
            vm.ConvertingLog += "开始备份文件\r\n";
            await BackupGameFile(vm);
            Directory.Move(Path.Combine(GamePath, GameSource), Path.Combine(GamePath, GameDest));
            vm.ConvertingLog += "释放Pkg文件至游戏目录\r\n";
            foreach (string file in GameFileList)
            {
                string temp = file.Replace(@$"\{GameDest}", GameDest);
                string gameFilePath = temp.Insert(0, $@"{GamePath}\");
                string pkgFilePath = temp.Insert(0, $@"{Path.Combine(Environment.CurrentDirectory, ReplaceSourceDirectory)}\");
                if (File.Exists(pkgFilePath))
                {
                    File.Move(pkgFilePath, gameFilePath);
                    vm.ConvertingLog += $"{pkgFilePath} 替换成功\r\n";
                }
                else
                {
                    vm.ConvertingLog += $"{gameFilePath}替换失败，文件有所缺失\r\n";
                }
            }
            vm.ConvertingLog += $"所有文件替换完成，尽情享受吧...\r\n";
        }

        /// <summary>
        /// 备份原来的游戏文件
        /// </summary>
        /// <returns></returns>
        public async Task BackupGameFile(SettingsPageViewModel vm)
        {
            foreach (string file in GameFileList)
            {
                string temp = file.Replace(@$"\{GameDest}", GameSource);
                string filePath = temp.Insert(0, $@"{GamePath}\");
                if (File.Exists(filePath))
                {
                    File.Move(filePath, $@"{filePath}.bak");
                    vm.ConvertingLog += $"{filePath} 备份成功\r\n";
                }
                else
                {
                    vm.ConvertingLog += $"{filePath}备份失败，文件不存在\r\n";
                }
            }
            string gameExecute = Scheme == YUANSHEN_EXE ? GENSHINIMPACT_EXE : YUANSHEN_EXE;
            File.Move(Path.Combine(GamePath, gameExecute), Path.Combine(GamePath, $"{gameExecute}.bak"));
        }

        /// <summary>
        /// 还原游戏文件逻辑
        /// </summary>
        /// <param name="newfile"></param>
        /// <param name="originalfile"></param>
        /// <param name="scheme"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task RestoreGameFiles(string[] newfile, string[] originalfile, string scheme, SettingsPageViewModel vm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 保存游戏设置
        /// </summary>
        /// <param name="vm"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SaveGameConfig(SettingsPageViewModel vm)
        {
            throw new NotImplementedException();
        }
    }
}
