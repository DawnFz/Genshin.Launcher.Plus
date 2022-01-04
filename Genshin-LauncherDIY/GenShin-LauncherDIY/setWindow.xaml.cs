using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.VisualBasic.Devices;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GenShin_LauncherDIY
{
    /// <summary>
    /// SetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetWindow : MetroWindow
    {
        public SetWindow()
        {
            InitializeComponent();
            Closing += Window_Closing;
            RunLoad();
            ReadUser();
            IsSDK();
        }

        private void LauncherPath_Text(object sender, TextChangedEventArgs e)
        {
            Settings.launcherPath = LauncherPath.Text;
        }

        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (bqload.Visibility == Visibility.Hidden)
            {
                e.Cancel = false;
            }
            else
            {
                this.ShowMessageAsync("警告", "请等待文件解压缩完毕，否则可能出现致命错误！！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                e.Cancel = true;
            }
        }

        private void setSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LauncherPath.Text))
            {
                this.ShowMessageAsync("警告", "路径为空,请输入游戏路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                return;
            }
            else if (Directory.Exists(Path.Combine(LauncherPath.Text, "Genshin Impact Game")) == false)
            {
                this.ShowMessageAsync("错误", "官方启动器路径不存在,请输入正确的路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                return;
            }
            else
            {
                IniControl.GamePath = LauncherPath.Text;
                Settings.gameMain = Path.Combine(LauncherPath.Text, "Genshin Impact Game/YuanShen.exe");
            }

            if (!CheckTool.IsNumber(GWidth.Text) || !CheckTool.IsNumber(GHeight.Text))
            {
                this.ShowMessageAsync("警告", "窗口大小错误, 请输入正确的分辨率！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                return;
            }
            else
            {
                IniControl.Width = GWidth.Text;
                IniControl.Height = GHeight.Text;
            }

            if (IsFullscreen.SelectedIndex == 0)
                IniControl.isAutoSize = false;
            else
                IniControl.isAutoSize = true;

            if (GamePort.SelectedIndex == 1)
            {
                IniControl.isMihoyo = 2;
                IniControl.Sub_channel("0", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                IniControl.Channel("14", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                IniControl.Cps("bilibili", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                (this.Owner as MainWindow).NowPort.Content = "当前客户端：哔哩哔哩服";
            }
            else if (GamePort.SelectedIndex == 2)
            {
                IniControl.isMihoyo = 3;
                IniControl.Cps("mihoyo", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                (this.Owner as MainWindow).NowPort.Content = "当前客户端：通用国际服";
            }
            else
            {
                IniControl.isMihoyo = 1;
                IniControl.Sub_channel("1", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                IniControl.Channel("1", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                IniControl.Cps("pcadbdpz", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                (this.Owner as MainWindow).NowPort.Content = "当前客户端：米哈游官服";
            }

            if (isUnFPS.IsChecked == true)
            {
                if (!CheckTool.IsNumber(MaxFPS.Text))
                {
                    this.ShowMessageAsync("警告", "帧率错误, 请输入正确的数字！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                    return;
                }
                else
                {
                    IniControl.MaxFps = MaxFPS.Text;
                    IniControl.isUnFPS = true;
                }
            }
            else
            {
                IniControl.MaxFps = "60";
                IniControl.isUnFPS = false;
            }

            if (MainGridHide.IsChecked == true)
            {
                IniControl.isMainGridHide = true;
                MainWindow mainWindow = new MainWindow();
                (this.Owner as MainWindow).MainGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                IniControl.isMainGridHide = false;
                MainWindow mainWindow = new MainWindow();
                (this.Owner as MainWindow).MainGrid.Visibility = Visibility.Visible;
            }
            if (UserList.SelectedIndex != -1)
            {
                string user = (UserList as ListBox).SelectedItem.ToString();
                IniControl.SwitchUser =user ;
                (this.Owner as MainWindow).NowUser.Content = $"账号：{user}";
                (this.Owner as MainWindow).NowUser.Visibility = Visibility.Visible;
            }
            IniControl.isWebBg = (bool)IsWebBg.IsChecked;
            IniControl.isPopup = (bool)PopupUP.IsChecked;
            IniControl.isClose = (bool)IsClose.IsChecked;
            AddConfig.CheckIni();
            WriteUser();
            Close();
        }
        private void Button_Click21_9(object sender, RoutedEventArgs e)
        {
            if (GHeight.Text == "" && GWidth.Text != "")
            {
                int x = Convert.ToInt32(GWidth.Text);
                int y = x * 9 / 21;
                GWidth.Text = Convert.ToString(x);
                GHeight.Text = Convert.ToString(y);
            }
            else if (GWidth.Text == "" && GHeight.Text != "")
            {
                int y = Convert.ToInt32(GHeight.Text);
                int x = y * 21 / 9;
                GWidth.Text = Convert.ToString(x);
                GHeight.Text = Convert.ToString(y);
            }
            else
            {
                this.ShowMessageAsync("提醒", "在上面随便一个框填上想要的宽或者高另一个框留空使用本按钮自动取21:9比例分辨率", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }
        private async void UnFps_Click(object sender, RoutedEventArgs e)
        {
            if (isUnFPS.IsChecked == true)
            {
                if ((await this.ShowMessageAsync("超级警告", "此操作涉及修改游戏客户端进程，我也不知道会不会出现封号风险，出现问题请自行承担后果！如之前没使用过UnlockFPS的建议不要使用！\r\n\r\n只解锁到160(大部分人屏幕应该都是144Hz)", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "不同意", NegativeButtonText = "同意" })) != MessageDialogResult.Affirmative)
                {
                    isUnFPS.IsChecked = true;
                    MaxFPS.IsEnabled = true;
                    MaxFPS.Text = IniControl.MaxFps;
                }
                else
                {
                    isUnFPS.IsChecked = false;
                    MaxFPS.IsEnabled = false;
                }
            }
            else
            {
                MaxFPS.IsEnabled = false;
                MaxFPS.Text = "";
            }
        }

        private async void IsClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsClose.IsChecked == true)
                await this.ShowMessageAsync("提醒", "选择后最小化和开始游戏时启动器将会遁入托盘图标，双击后可再次打开启动器主页面[本功能可能导致进程残留]", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
        }
        private async void IsWebBg_Click(object sender, RoutedEventArgs e)
        {
            if (IsWebBg.IsChecked == true)
                await this.ShowMessageAsync("提醒", "选中该功能后将在下次启动生效，根据网络速度打开瞬间启动器可能会出现白色无背景状态，等待几秒即可~[使用自定义背景后本选项不生效]", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
        }

        private void IsSDK()
        {
            if (LauncherPath.Text == "")
            {
                SDKlive.Content = "SDK:未知";
                Fixbtn.IsEnabled = false;
            }
            else
            {
                if (GamePort.SelectedIndex == 2 || GamePort.SelectedIndex == 0)
                {
                    SDKlive.Content = "SDK:无需";
                    Fixbtn.IsEnabled = false;
                }
                else if (GamePort.SelectedIndex == 1 && File.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game/YuanShen_Data/Plugins/PCGameSDK.dll")))
                {
                    SDKlive.Content = "SDK:存在";
                    Fixbtn.IsEnabled = false;
                }
                else
                {
                    SDKlive.Content = "SDK:缺失";
                    Fixbtn.IsEnabled = true;
                }
            }
        }
        private async void Fix_Click(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("提醒", "修复SDK仅用于官转哔服，国际服和国服无需修复", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "取消", NegativeButtonText = "确定修复" })) != MessageDialogResult.Affirmative)
            {
                if (Directory.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game/YuanShen_Data")) == true)
                {
                    UtilsTools utils = new UtilsTools();
                    utils.FileWriter("Res/mihoyosdk.dll", Path.Combine(Settings.launcherPath, "Genshin Impact Game/YuanShen_Data/Plugins/PCGameSDK.dll"));
                    IsSDK();
                }
                else
                {
                    await this.ShowMessageAsync("提醒", "该端无需修复SDK文件", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                }
            }
        }
        private async void ToGlobal_Click(object sender, RoutedEventArgs e)
        {
            if ((await this.ShowMessageAsync("警告！！", "转换或还原将会执行重命名，替换，删除等操作修改客户端文件，该过程大概率会触发杀软报毒！为了防止客户端损坏导致不完整，执行前检查杀软（包括 Windows Defender）是否完全关闭或将本启动器加入白名单，并检查游戏是否彻底关闭，否则可能将导致客户端文件缺失！！\r\n\r\n提示：如游戏大版本更新时请执行还原转换为国内服使用游戏自带启动器更新！", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "取消转换", NegativeButtonText = "确定转换" })) != MessageDialogResult.Affirmative)
            {
                if (Directory.Exists(Path.Combine(LauncherPath.Text, "Genshin Impact Game")) == true)
                {
                    string pkgfile = UtilsTools.MiddleText(UtilsTools.ReadHTML("https://www.cnblogs.com/DawnFz/p/7271382.html", "UTF-8"), "[$pkg$]", "[#pkg#]");
                    if (Convert.ToString(ToGlobal.Content) == "复原")
                    {
                        Thread StartRe = new Thread(() => ReCnGame());
                        bqload.Visibility = Visibility.Visible;
                        setSave.IsEnabled = false;
                        ToGlobal.IsEnabled = false;
                        TimeStatus.Content = "当前状态：正在还原游戏";
                        StartRe.Start();
                    }
                    else if ("2.3.0-2" != pkgfile)
                    {
                        await this.ShowMessageAsync("提示", "国际服转换包有新版本：" + pkgfile + "\r\n访问密码：etxd  已复制到剪切板", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                        Clipboard.SetText("etxd");
                        Thread.Sleep(2500);
                        Process.Start("https://pan.baidu.com/s/1-5zQoVfE7ImdXrn8OInKqg");
                    }
                    else if (Directory.Exists(@"GlobalFile") == true)
                    {
                        bool error = false;
                        for (int i = 0; i < Settings.globalfiles.Length; i++)
                        {
                            if (File.Exists(@"GlobalFile//" + Settings.globalfiles[i]) == false)
                            {
                                LogBox.Text = Settings.globalfiles[i] + "文件不存在，请重新下载资源包或尝试重新操作";
                                error = true;
                                break;
                            }
                            LogBox.Text = Settings.globalfiles[i] + "存在";
                        }
                        if (!error)
                        {
                            Thread StartMove = new Thread(() => MoveFile());
                            bqload.Visibility = Visibility.Visible;
                            setSave.IsEnabled = false;
                            ToGlobal.IsEnabled = false;
                            TimeStatus.Content = "当前状态：正在替换资源";
                            StartMove.Start();
                        }
                        else
                        {
                            await this.ShowMessageAsync("提示", "转换资源不完整\r\n请重新下载", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                            bqload.Visibility = Visibility.Hidden;
                            setSave.IsEnabled = true;
                        }
                    }
                    else
                    {
                        Thread StartUn = new Thread(() => UnFile());
                        bqload.Visibility = Visibility.Visible;
                        setSave.IsEnabled = false;
                        ToGlobal.IsEnabled = false;
                        TimeStatus.Content = "当前状态：正在解压资源";
                        StartUn.Start();
                    }
                }
                else
                {
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        this.ShowMessageAsync("提示", "目录不存在，请填写正确的目录", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                        return;
                    }));
                }
            }
        }
        private void UnFile()
        {
            if (UtilsTools.UnZip(@"GlobalFile.pkg", @""))
            {
                bool error = false;
                for (int i = 0; i < Settings.globalfiles.Length; i++)
                {
                    if (File.Exists(@"GlobalFile//" + Settings.globalfiles[i]) == false)
                    {
                        Dispatcher.Invoke(new Action(delegate ()
                        {
                            LogBox.Text = Settings.globalfiles[i] + "文件不存在，请重新下载资源包或尝试重新操作";
                        }));
                        error = true;
                        break;
                    }
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        LogBox.Text = Settings.globalfiles[i] + "存在";
                    }));
                }
                if (!error)
                {
                    MoveFile();
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        bqload.Visibility = Visibility.Hidden;
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        this.ShowMessageAsync("提示", "资源解压完成但不完整\r\n请重新下载", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                        bqload.Visibility = Visibility.Hidden;
                        setSave.IsEnabled = true;
                    }));
                }
            }
            else
            {
                Dispatcher.Invoke(new Action(async delegate ()
                {
                    await this.ShowMessageAsync("提示", "没有找到资源[GlobalFile.pkg]或解压失败\r\n点击确定跳转到下载资源页面，访问密码已复制到剪贴板", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                    Clipboard.SetText("etxd");
                    Process.Start(Settings.htmlUrl[5]);
                    bqload.Visibility = Visibility.Hidden;
                    ToGlobal.IsEnabled = true;
                    TimeStatus.Content = "当前状态：未找到资源包";
                    setSave.IsEnabled = true;
                }));
            }
        }
        private void MoveFile()
        {
            Computer redir = new Computer();
            Dispatcher.Invoke(new Action(delegate ()
            {
                TimeStatus.Content = "当前状态：正在备份原文件";
            }));
            for (int a = 0; a < Settings.cnfiles.Length; a++)
            {
                String newFileName = Path.GetFileNameWithoutExtension(Path.Combine(Settings.launcherPath, "Genshin Impact Game/") + Settings.cnfiles[a]) + Path.GetExtension(Path.Combine(Settings.launcherPath, "Genshin Impact Game/") + Settings.cnfiles[a]);
                if (File.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game", Settings.cnfiles[a])) == true)
                {
                    redir.FileSystem.RenameFile(Path.Combine(Settings.launcherPath, "Genshin Impact Game", Settings.cnfiles[a]), newFileName + ".bak");
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        LogBox.Text = newFileName + "备份成功";
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        LogBox.Text = newFileName + "备份失败，跳过";
                    }));
                }
            }
            Dispatcher.Invoke(new Action(delegate ()
            {
                TimeStatus.Content = "当前状态：开始替换资源";
            }));
            redir.FileSystem.RenameDirectory(Path.Combine(Settings.launcherPath, "Genshin Impact Game", "YuanShen_Data"), "GenshinImpact_Data");
            for (int i = 0; i < Settings.globalfiles.Length; i++)
            {
                File.Copy(Path.Combine(@"GlobalFile", Settings.globalfiles[i]), Path.Combine(Settings.launcherPath, "Genshin Impact Game", Settings.globalfiles[i]), true);
                this.Dispatcher.Invoke(new Action(delegate ()
                {
                    LogBox.Text = Settings.globalfiles[i] + "替换成功";
                }));
            };
            Dispatcher.Invoke(new Action(delegate ()
            {
                IniControl.Sub_channel("0", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                IniControl.Channel("1", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                TimeStatus.Content = "当前状态：无状态";
            }));
            if (File.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game/GenshinImpact_Data/Plugins/PCGameSDK.dll")) == true)
                File.Delete(Path.Combine(Settings.launcherPath, "Genshin Impact Game/GenshinImpact_Data/Plugins/PCGameSDK.dll"));
            IniControl.isMihoyo = 3;
            Dispatcher.Invoke(new Action(delegate ()
            {
                ToGlobal.IsEnabled = true;
                IsGlobal();
                IsSDK();
                setSave.IsEnabled = true;
                bqload.Visibility = Visibility.Hidden;
                GamePort.SelectedIndex = 2;
                this.ShowMessageAsync("提示", "转换完毕，尽情享受吧！~", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }));
        }
        private void IsGlobal()
        {
            if (File.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game/YuanShen.exe")) == true)
            {
                ToGlobal.Content = "转换";
                GlobalItem.IsEnabled = false;

            }
            else if (File.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game", "GenshinImpact.exe")) == true)
            {
                ToGlobal.Content = "复原";
                GlobalItem.IsEnabled = true;
                MihoyoItem.IsEnabled = false;
                BiliItem.IsEnabled = false;
            }
        }
        private void ReCnGame()
        {
            Computer redir = new Computer();
            Dispatcher.Invoke(new Action(delegate ()
            {
                TimeStatus.Content = "当前状态：清理现存文件";
            }));
            for (int i = 0; i < Settings.globalfiles.Length; i++)
            {
                if (File.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game", Settings.globalfiles[i])) == true)
                {
                    File.Delete(Path.Combine(Settings.launcherPath, "Genshin Impact Game", Settings.globalfiles[i]));
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        LogBox.Text = Settings.globalfiles[i] + "清理完毕";
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        LogBox.Text = Settings.globalfiles[i] + "文件不存在，已跳过";
                    }));
                }
            }
            Dispatcher.Invoke(new Action(delegate ()
            {
                TimeStatus.Content = "当前状态：正在还原文件";
            }));
            redir.FileSystem.RenameDirectory(Path.Combine(Settings.launcherPath, "Genshin Impact Game/GenshinImpact_Data"), "YuanShen_Data");
            int whole = 0, success = 0;
            for (int a = 0; a < Settings.cnfiles.Length; a++)
            {
                string newFileName = Path.GetFileNameWithoutExtension(Settings.cnfiles[a]) + Path.GetExtension(Settings.cnfiles[a]);
                if (File.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game", Settings.cnfiles[a] + ".bak")) == true)
                {
                    redir.FileSystem.RenameFile(Path.Combine(Settings.launcherPath, "Genshin Impact Game", Settings.cnfiles[a] + ".bak"), newFileName);
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        LogBox.Text = Settings.cnfiles[a] + "还原成功";
                    }));
                    success++;
                }
                else
                {
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        LogBox.Text = Settings.cnfiles[a] + "不存在，跳过还原";
                    }));
                    whole++;
                }
            }
            UtilsTools utils = new UtilsTools();

            Dispatcher.Invoke(new Action(delegate ()
            {
                IsSDK();
                GlobalItem.IsEnabled = false;
                MihoyoItem.IsEnabled = true;
                BiliItem.IsEnabled = true;
                GamePort.SelectedIndex = 0;
                TimeStatus.Content = "当前状态：无状态";
                IniControl.isMihoyo = 1;
                IniControl.Sub_channel("1", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                IniControl.Channel("1", Path.Combine(LauncherPath.Text, "Genshin Impact Game/Config.ini"));
                AddConfig.CheckIni();
                bqload.Visibility = Visibility.Hidden;
                setSave.IsEnabled = true;
                ToGlobal.Content = "转换";
                ToGlobal.IsEnabled = true;
                this.ShowMessageAsync("提示", "还原完毕，本次还原成功" + success + "个文件，失败或缺失" + whole + "个文件", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }));
        }

        private async void DelUser_Click(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedIndex != -1)
            {
                if ((await this.ShowMessageAsync("警告", "您确定删除账号：" + (UserList as ListBox).SelectedItem.ToString() + "吗？！", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "取消", NegativeButtonText = "删除" })) != MessageDialogResult.Affirmative)
                {
                    File.Delete(Path.Combine(@"UserData", (UserList as ListBox).SelectedItem.ToString()));
                    UserList.Items.RemoveAt(UserList.Items.IndexOf(UserList.SelectedItem));
                }
            }
            else
            {
                this.ShowMessageAsync("错误", "请选择要删除的账户再进行操作", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }
        private void XY_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object a;
            a = GameXY.SelectedValue;
            switch (a)
            {
                case 1:
                    GWidth.Text = "3840";
                    GHeight.Text = "2160";
                    break;
                case 2:
                    GWidth.Text = "2560";
                    GHeight.Text = "1080";
                    break;
                case 3:
                    GWidth.Text = "1920";
                    GHeight.Text = "1080";
                    break;
                case 4:
                    GWidth.Text = "1600";
                    GHeight.Text = "900";
                    break;
                case 45:
                    GWidth.Text = "1360";
                    GHeight.Text = "768";
                    break;
                case 6:
                    GWidth.Text = "1280";
                    GHeight.Text = "1024";
                    break;
                case 7:
                    GWidth.Text = "1280";
                    GHeight.Text = "720";
                    break;
                default:
                    break;
            }
        }
        private void ReadUser()
        {
            DirectoryInfo TheFolder = new DirectoryInfo(@"UserData");
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                UserList.Items.Add(NextFile.Name);
            }
        }
        private void WriteUser()
        {
            if (UserList.SelectedIndex != -1)
            {
                string name = (UserList as ListBox).SelectedItem.ToString();
                if (name.StartsWith("国服-") == true)
                {
                    Settings.regIsGlobal[0] = "原神";
                    Settings.regIsGlobal[1] = "MIHOYOSDK_ADL_PROD_CN_h3123967166";
                    YSAccount acct = YSAccount.ReadFromDisk(name);
                    acct.WriteToRegedit();
                }
                else if (name.StartsWith("国际服-") == true)
                {
                    Settings.regIsGlobal[0] = "Genshin Impact";
                    Settings.regIsGlobal[1] = "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810";
                    YSAccount acct = YSAccount.ReadFromDisk(name);
                    acct.WriteToRegedit();
                }
                else
                {
                    Settings.regIsGlobal[0] = "原神";
                    Settings.regIsGlobal[1] = "MIHOYOSDK_ADL_PROD_CN_h3123967166";
                    YSAccount acct = YSAccount.ReadFromDisk(name);
                    acct.WriteToRegedit();
                }
            }
        }

        private void Dir_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog("请选择原神启动器所在文件夹或游戏本体上一级目录");
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                LauncherPath.Text = dialog.FileName;
            }
        }

        private void RunLoad()
        {
            LauncherPath.Text = IniControl.GamePath;
            IsFullscreen.SelectedIndex = IniControl.isAutoSize ? 1 : 0;
            PopupUP.IsChecked = IniControl.isPopup ? true : false;
            IsWebBg.IsChecked = IniControl.isWebBg? true : false;
            IsClose.IsChecked = IniControl.isClose ? true : false;
            MainGridHide.IsChecked = IniControl.isMainGridHide ? true : false;
            isUnFPS.IsChecked = IniControl.isUnFPS ? true : false;
            MaxFPS.IsEnabled = IniControl.isUnFPS ? true : false;
            MaxFPS.Text = IniControl.MaxFps;
            GHeight.Text = IniControl.Height;
            GWidth.Text = IniControl.Width;
            IsGlobal();

            if (IniControl.isMihoyo == 1)
            {
                GamePort.SelectedIndex = 0;
                GlobalItem.IsEnabled = false;
            }
            else if (IniControl.isMihoyo == 2)
            {
                GamePort.SelectedIndex = 1;
                GlobalItem.IsEnabled = false;
            }
            else
            {
                GamePort.SelectedIndex = 2;
                GlobalItem.IsEnabled = true;
                MihoyoItem.IsEnabled = false;
                BiliItem.IsEnabled = false;
            }

            {
                List<DisplayList> list = new List<DisplayList>();
                list.Add(new DisplayList { Name = "3840×2160-16:9", ID = 0, X = 1 });
                list.Add(new DisplayList { Name = "2560×1080-21:9", ID = 0, X = 2 });
                list.Add(new DisplayList { Name = "1920×1080-16:9", ID = 1, X = 3 });
                list.Add(new DisplayList { Name = "1600×900-16:9", ID = 2, X = 4 });
                list.Add(new DisplayList { Name = "1360×768-16:9", ID = 3, X = 5 });
                list.Add(new DisplayList { Name = "1280×1024-4:3", ID = 4, X = 6 });
                list.Add(new DisplayList { Name = "1280×720-16:9", ID = 5, X = 7 });
                GameXY.ItemsSource = list;
                GameXY.DisplayMemberPath = "Name";
                GameXY.SelectedValuePath = "X";
                GameXY.SelectedIndex = -1;
            }
        }

        private async void BiliItem_Selected(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(Path.Combine(Settings.launcherPath, "Genshin Impact Game/YuanShen_Data/Plugins/PCGameSDK.dll")))
            {
                await this.ShowMessageAsync("提示", "检测到哔哩端SDK缺失，记得修复后再启动游戏", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                IsSDK();
            }
        }
    }
}
