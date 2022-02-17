using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class NoticeOverAllBase: ObservableObject
    {
        public NoticeOverAllBase()
        {

        }
        //贯通到主页面的索引
        private int _MainPagesIndex;
        public int MainPagesIndex
        {
            get => _MainPagesIndex;
            set => SetProperty(ref _MainPagesIndex,value);
        }
    }
}
