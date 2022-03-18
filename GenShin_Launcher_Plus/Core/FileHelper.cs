using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Windows;
using GenShin_Launcher_Plus.Core.Extension;

namespace GenShin_Launcher_Plus.Core
{
    public class FileHelper
    {
        public string ReadHTML(string url)
        {
            string strHTML = "";
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
                return "";
            }
            return strHTML;
        }
        public string MiddleText(string Str, string preStr, string nextStr)
        {
            try
            {
                string tempStr = Str.Substring(Str.IndexOf(preStr) + preStr.Length);
                tempStr = tempStr.Substring(0, tempStr.IndexOf(nextStr));
                return tempStr;
            }
            catch
            {
                return "";
            }
        }
        public string GetJsonFromHtml(string tag)
        {
            return MiddleText(ReadHTML("https://www.cnblogs.com/DawnFz/p/15990791.html"),$"【{tag}++】",$"【{tag}--】");
        }
        //

        public static bool UnZip(string zipFile, string directory)
        {
            try
            {
                ZipInputStream f = new(File.OpenRead(zipFile));
            A: ZipEntry zp = f.GetNextEntry();
                while (zp != null)
                {
                    string un_tmp2;
                    if (zp.Name.EndsWith("/"))
                    {
                        int tmp1 = zp.Name.LastIndexOf("/");
                        un_tmp2 = zp.Name.Substring(0, tmp1);
                        Directory.CreateDirectory(directory + un_tmp2);
                    }
                    if (!zp.IsDirectory && zp.Crc != 00000000L)
                    {
                        int i = 2048;
                        byte[] b = new byte[i];
                        FileStream s = File.Create(directory + zp.Name);
                        while (true)
                        {
                            i = f.Read(b, 0, b.Length);
                            if (i > 0)
                                s.Write(b, 0, i);
                            else
                                break;
                        }
                        s.Close();
                    }
                    goto A;
                }
                f.Close();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 将程序中的资源文件写出到硬盘
        /// </summary>
        public static void ExtractEmbededAppResource(string resourceName, string fileName)
        {
            Uri uri = new($"pack://application:,,,/{resourceName}", UriKind.RelativeOrAbsolute);
            Stream stream = Application.GetResourceStream(uri).Stream;
            stream.ToFile(fileName);
        }
    }
}
