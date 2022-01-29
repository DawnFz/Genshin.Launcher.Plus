using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class NoticeOverAllBase:NotificationObject
    {
        public NoticeOverAllBase()
        {

        }
        //贯通到主页面的索引
        private int _MainPagesIndex;
        public int MainPagesIndex
        {
            get { return _MainPagesIndex; }
            set { _MainPagesIndex = value; OnPropChanged("MainPagesIndex"); }
        }
    }
}
