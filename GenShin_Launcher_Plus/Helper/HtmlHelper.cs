using System.IO;
using System.Text;
using System.Net;
using System.Windows;
using GenShin_Launcher_Plus.Core.Extension;
using System.Threading.Tasks;
using System.Net.Http;

namespace GenShin_Launcher_Plus.Core
{
    public class HtmlHelper
    {

        private const string Url = "https://www.cnblogs.com/DawnFz/p/15990791.html";
        public static string ReadHTMLAsText(string url)
        {
            string strHTML = string.Empty;
            try
            {
                WebClient myWebClient = new();
                myWebClient.Proxy = null;
                Stream myStream = myWebClient.OpenRead(url);
                StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
                strHTML = sr.ReadToEnd();
                myStream.Close();
            }
            catch
            {
                return string.Empty;
            }
            return strHTML;
        }
        public static string MiddleText(string Str, string preStr, string nextStr)
        {
            try
            {
                string tempStr = Str.Substring(Str.IndexOf(preStr) + preStr.Length);
                tempStr = tempStr.Substring(0, tempStr.IndexOf(nextStr));
                return tempStr;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string GetInfoFromHtmlAsync(string tag)
        {
            return MiddleText(ReadHTMLAsText(Url), $"【{tag}++】", $"【{tag}--】");
        }

    }
}
