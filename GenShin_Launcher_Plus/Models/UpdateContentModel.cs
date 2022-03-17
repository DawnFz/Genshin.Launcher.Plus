using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.Models
{
    public class UpdateContentModel
    {
        public string Version { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DownloadUrl { get; set; }
        public string GlobalDownloadUrl { get; set; }
        public string PkgVersion { get; set; }
        public string BgUrl { get; set; }

    }
}
