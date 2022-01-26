using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GenShin_Launcher_Plus.Command;
using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;


namespace GenShin_Launcher_Plus.ViewModels
{
    public class HomePageViewModel : NotificationObject

    {
        private IDialogCoordinator dialogCoordinator;
        public HomePageViewModel(IDialogCoordinator instance)
        {
            IniModel = new SettingsIniModel();
            dialogCoordinator = instance;
            RunGameCommand = new DelegateCommand { ExecuteAction = new Action<object>(RunGame) };
            IniModel.SwitchUser = $"账号：{IniControl.SwitchUser}";

            if (IniControl.SwitchUser != null && IniControl.SwitchUser != "")
            { IniModel.IsSwitchUser = "Visible"; OnPropChanged("IniModel"); }
            else
            { IniModel.IsSwitchUser = "Hidden"; OnPropChanged("IniModel"); }

            GetGamePort();

        }

        private SettingsIniModel _IniModel;
        public SettingsIniModel IniModel
        {
            get { return _IniModel; }
            set
            {
                _IniModel = value;
                OnPropChanged("IniModel");
            }
        }

        private void GetGamePort()
        {
            if (File.Exists(Path.Combine(IniControl.GamePath, "config.ini")))
            {
                if (IniControl.Cps == "pcadbdpz")
                { IniModel.SwitchPort = "客户端：官方服务器"; OnPropChanged("IniModel"); }
                else if (IniControl.Cps == "bilibili")
                { IniModel.SwitchPort = "客户端：哔哩哔哩服"; OnPropChanged("IniModel"); }
                else if (IniControl.Cps == "mihoyo")
                { IniModel.SwitchPort = "客户端：通用国际服"; OnPropChanged("IniModel"); }
                else
                { IniModel.SwitchPort = "客户端：未知客户端"; OnPropChanged("IniModel"); }
            }
            else
            { IniModel.SwitchPort = "客户端：未知客户端"; OnPropChanged("IniModel"); }
        }

        public DelegateCommand RunGameCommand { get; set; }
        private async void RunGame(object parameter)
        {
            if (IniControl.isUnFPS)
            {
                Process.Start(@"unlockfps.exe");
            }
            string gameMain = Path.Combine(IniControl.GamePath, "YuanShen.exe");
            var argBuilder = new CommandLineBuilder();
            argBuilder.AddOption("-screen-fullscreen ", Convert.ToString(IniControl.FullSize));
            argBuilder.AddOption("-screen-height ", IniControl.Height);
            argBuilder.AddOption("-screen-width ", IniControl.Width);
            argBuilder.AddOption("-pop ", IniControl.isPopup ? " -popupwindow " : "");

            if (!File.Exists(gameMain))
            {
                gameMain = Path.Combine(IniControl.GamePath, "GenshinImpact.exe");
                if (!File.Exists(gameMain))
                {
                    await dialogCoordinator.ShowMessageAsync(this, "错误", "游戏路径为空或游戏文件不存在\r\n请点击右侧设置按钮进入设置填写游戏目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                    return;
                }
            }

            Task StartGame = new(() =>
            {
                ProcessStartInfo info = new()
                {
                    FileName = gameMain,
                    Verb = "runas",
                    UseShellExecute = true,
                    WorkingDirectory = IniControl.GamePath,
                    Arguments = argBuilder.ToString()
                };
                Process.Start(info);
            });
            StartGame.Start();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
        }
    }
}
