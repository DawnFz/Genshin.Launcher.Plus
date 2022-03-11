using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.Models
{
    public class UpdateContentModel
    {
        //软件本体
        public string Version { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DownloadUrl { get; set; }
        //Pkg文件包
        public string PkgVersion { get; set; }
        public string BgUrl { get; set; }

    }
}
