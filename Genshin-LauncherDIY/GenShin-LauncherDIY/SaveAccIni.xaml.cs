using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GenShin_LauncherDIY
{
    /// <summary>
    /// SaveAccIni.xaml 的交互逻辑
    /// </summary>
    public partial class SaveAccIni : MetroWindow
    {
        public SaveAccIni()
        {
            InitializeComponent();
        }
        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(IsCN.IsChecked==true)
            {
                if (string.IsNullOrWhiteSpace(saveFilename.Text))
                {
                    this.ShowMessageAsync("错误", "请先输入正确名字！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                    return;
                }
                Settings.regIsGlobal[0] = "原神";
                Settings.regIsGlobal[1] = "MIHOYOSDK_ADL_PROD_CN_h3123967166";
                YSAccount acct = YSAccount.ReadFromRegedit(false);
                acct.Name = "国服-"+saveFilename.Text;
                acct.WriteToDisk();
                this.Close();
            }
            else if(IsGlobal.IsChecked==true)
            {
                if (string.IsNullOrWhiteSpace(saveFilename.Text))
                {
                    this.ShowMessageAsync("错误", "请先输入正确名字！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                    return;
                }
                Settings.regIsGlobal[0] = "Genshin Impact";
                Settings.regIsGlobal[1] = "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810";
                YSAccount acct = YSAccount.ReadFromRegedit(false);
                acct.Name = "国际服-"+saveFilename.Text;
                acct.WriteToDisk();
                this.Close();
            }
            else
            {
                this.ShowMessageAsync("错误", "请选择保存的账号服务器类型！", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }




    }
}
