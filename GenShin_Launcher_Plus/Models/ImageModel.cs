using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.Models
{
    public class DailyImageModel
    {
        public List<DailyImageArray> ImageInfo { get; set; }
    }
    public class DailyImageArray
    {
        public string ImagePid { get; set; }
        public string ImageDate { get; set; }
    }


    //下面是用于测试未正式启用的直接从Api获取图片的模型
    public class ImageModel
    {
        public string Error { get; set; }
        public List<PixivJson> data { get; set; }

    }
    public class PixivJson
    {
        public string pid { get; set; }
        public int p { get; set; }
        public bool r18 { get; set; }
        public string ext { get; set; }
        public OriginalUrl urls { get; set; }
    }
    public class OriginalUrl
    {
        public string original { get; set; }
    }
}
