using GenShin_Tools.Config;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace GenShin_Tools
{
    /// <summary>
    /// setWindow.xaml 的交互逻辑
    /// </summary>
    public partial class setWindow : MetroWindow
    {
        public setWindow()
        {
            InitializeComponent();
            GamePath.Text = Config.IniGS.gamePath;//游戏路径
            if (!Config.IniGS.isAutoSize)//全屏
                FullF.IsChecked = true;
            else
                FullT.IsChecked = true;
            GHeight.Text = Config.IniGS.Height.ToString();//高度
            GWidth.Text = Config.IniGS.Width.ToString();//宽度
            if (!Config.IniGS.BiOrMi)//读取服务器
                BIliS.IsChecked = true;
            else
                MiS.IsChecked = true;
            IsSDK();
        }

        private void setSave_Click(object sender, RoutedEventArgs e)
        {
            //游戏路径保存
            if (string.IsNullOrWhiteSpace(GamePath.Text))
            {
                this.ShowMessageAsync("警告", "游戏路径为空,请输入游戏路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                return;
            }
            else if (Directory.Exists(GamePath.Text) == false)
            {
                this.ShowMessageAsync("错误", "游戏路径不存在,请输入正确的游戏路径！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                return;
            }
            else
            {
                Config.IniGS.gamePath = GamePath.Text;
                Config.Settings.GamePath = GamePath.Text;
            }
            //游戏分辨率保存
            if (!Config.checkTool.IsNumber(GWidth.Text) || !Config.checkTool.IsNumber(GHeight.Text))
            {
                this.ShowMessageAsync("警告", "窗口大小错误, 请输入正确的分辨率！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                return;
            }
            else
            {
                Settings.Height = GHeight.Text;
                Settings.Width = GWidth.Text;
                Config.IniGS.Width = Convert.ToUInt16(GWidth.Text);
                Config.IniGS.Height = Convert.ToUInt16(GHeight.Text);
            }
            //是否全屏
            if (FullF.IsChecked == true)
            {
                Settings.FullS = "0";
                Config.IniGS.isAutoSize = false;
            }
            else if (FullT.IsChecked == true)
            {
                Settings.FullS = "1";
                Config.IniGS.isAutoSize = true;
            }
            //B服或官服
            if (MiS.IsChecked == true)
            {
                Config.IniGS.BiOrMi = true;
                BOM.Sub_channel = 1;
                BOM.Channel = 1;
            }
            else
            {
                Config.IniGS.BiOrMi = false;
                BOM.Sub_channel = 0;
                BOM.Channel = 14;
            }
            Config.setConfig.checkini();
            this.Close();
        }

        private void Button_Click21_9(object sender, RoutedEventArgs e)
        {
            int x = (int)SystemParameters.WorkArea.Width - 80;
            int y = (int)(SystemParameters.WorkArea.Width - 80) * 9 / 21;
            GWidth.Text = Convert.ToString(x);
            GHeight.Text = Convert.ToString(y);
        }

        private void Button_Click4_3(object sender, RoutedEventArgs e)
        {
            int x = (int)(SystemParameters.WorkArea.Height - 80) * 4 / 3;
            int y = (int)SystemParameters.WorkArea.Height - 80;
            GWidth.Text = Convert.ToString(x);
            GHeight.Text = Convert.ToString(y);
        }
        private void IsSDK()
        {
            if (GamePath.Text == "")
            {
                SDKlive.Content = "SDK:未知";
                Fixbtn.IsEnabled = false;
            }
            else
            {
                if (File.Exists(GamePath.Text + "\\Genshin Impact Game\\YuanShen_Data\\Plugins\\PCGameSDK.dll") == false)
                {
                    SDKlive.Content = "SDK:缺失";
                    Fixbtn.IsEnabled = true;
                }
                else
                {
                    SDKlive.Content = "SDK:存在";
                    Fixbtn.IsEnabled = false;
                }
            }
        }
        private void Fix_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"mihoyosdk.dll") == false)
            {
                this.ShowMessageAsync("错误", "mihoyo.dll文件不存在，无法进行MihoyoSDK修复\r\n请检查本启动器文件是否完整或前往GitHub重新下载！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
            else
            {
                this.ShowMessageAsync("提示", "MihoyoSDK修复成功", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                File.Copy(@"mihoyosdk.dll", GamePath.Text + "\\Genshin Impact Game\\YuanShen_Data\\Plugins\\PCGameSDK.dll", true);
                IsSDK();
            }
        }
    }
}
