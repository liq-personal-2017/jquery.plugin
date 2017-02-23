using System.IO;
using System.Text.RegularExpressions;

namespace jquery.plugin.CommonClass
{
    public class FileUtil
    {
        public static string readFile(string filepath)
        {
            StreamReader sr = null;
            string content = "";
            try
            {
                FileInfo fi = new FileInfo(filepath);
                sr = new StreamReader(fi.OpenRead());
                content = sr.ReadToEnd();
            }
            catch
            {
                content = null;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return content;

        }

        /// <summary>
        /// 合并两个路径，不论是否有根路径，path1是主路径，path2是相对路径
        ///     /d1/d2/d3/file
        ///     ../../d4/file2
        ///         /d1/d4/file2
        /// </summary>
        /// <param name="path1">/d1/d2/d3/file--如果本身是目录而不是文件，则必须以/结尾，否则认为是文件层级数会出错</param>
        /// <param name="path2">../../d4/file2</param>
        /// <returns>/d1/d4/file2</returns>
        public static string mergePath(string path1, string path2)
        {
            var path = "";
            path1 = path1.Replace("\\", "/");
            path2 = path2.Replace("\\", "/");

            Regex r = new Regex(@"(\.\.\/)+?");
            MatchCollection m = r.Matches(path2);
            var censhu = m.Count + 1;
            path = path1;
            while (censhu-- > 0)
            {
                path = path.Substring(0, path.LastIndexOf("/"));
            }
            path2 = path2.Replace("../", "");
            path = (path + "/" + path2);

            return path;
        }
    }
}