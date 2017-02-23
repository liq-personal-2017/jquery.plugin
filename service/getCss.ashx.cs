using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Text;
using System.Text.RegularExpressions; 

namespace jquery.plugin.service
{
    /// <summary>
    /// getCss 的摘要说明
    /// </summary>
    public class getCss : IHttpHandler
    {
        private string ip = Global.LocalhostIp;
        const string rootpath = "/jquery.plugin/resource/";
        const string cssextname = ".css";//".min.css";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/css";
            string[] files = context.Request.Params["f"].ToString().Split('^');
            StringBuilder cssContext = new StringBuilder(50000);
            //统一添加charset
            cssContext.Append("@charset \"UTF-8\";\n");
            foreach (string file in files)
            {
                //file = context.Server.MapPath(file);
                switch (file)
                {
                   
                    case "resource":
                        {
                            #region resource

                            
                            //8
                            cssContext.Append("/*jquery.list/jquery.radiolist*/\n");
                            cssContext.Append(readCssFile(getPath(rootpath + "jquery.list/jquery.radiolist", context.Server)));
                            cssContext.Append("\n");
                            //9
                            cssContext.Append("/*jquery.list/jquery.checklist*/\n");
                            cssContext.Append(readCssFile(getPath(rootpath + "jquery.list/jquery.checklist", context.Server)));
                            cssContext.Append("\n");
                            //10
                            cssContext.Append("/*jquery.list/jquery.itemlist*/\n");
                            cssContext.Append(readCssFile(getPath(rootpath + "jquery.list/jquery.itemlist", context.Server)));
                            cssContext.Append("\n");
                            //11
                           
                            //13
                            cssContext.Append("/*jquery.fileuploader/jquery.fileuploader*/\n");
                            cssContext.Append(readCssFile(getPath(rootpath + "jquery.fileuploader/jquery.fileuploader", context.Server)));
                            cssContext.Append("\n");
                             
                            #endregion
                        }
                        break; 
                    case "bootstrap":
                        {
                            #region bootstrap
                            //    //=================bootstrap//=================
                         
                            //1
                            cssContext.Append("/*bootstrap/bootstrap*/\n");
                            cssContext.Append(readCssFile(getPath(rootpath + "bootstrap/bootstrap", context.Server)));
                            cssContext.Append("\n");

                            #endregion
                        }
                        break;
                    case "common":
                        {
                            #region common
                           
                            //1
                            cssContext.Append("/*common/common*/\n");
                            cssContext.Append(readCssFile(getPath(rootpath + "common/common", context.Server)));
                            cssContext.Append("\n");
                            #endregion
                        }
                        break; 
                    case "":
                        break;
                    default:
                        //cssContext.Append(readCssFile(getPath(file, context.Server)));
                        break;
                }
            }
            context.Response.Write(cssContext.ToString());
            context.Response.End();
        }

        private string getPath(string path, HttpServerUtility server)
        {
            if (path.EndsWith(".css"))
            {
                return server.MapPath(path);
            }
            return server.MapPath(path) + cssextname;
        }



        private string readCssFile(string filepath)
        {
            string cssContent = "";
            object cachecss = CommonClass.CacheUtil.GetCache(filepath);
            if (cachecss == null || Global.isCache == false)
            {

                cssContent = CommonClass.FileUtil.readFile(filepath);
                if (cssContent == null)
                {
                    cssContent += "/*file:\"" + filepath + "\" read error;*/";
                }
                else
                {
                    //将文件中的charset设置都去掉
                    cssContent = cssContent.Replace("@charset \"UTF-8\";", "")
                        .Replace("[localhostIp]", ip)//处理ip问题
                        .Replace("\"", "'");//处理引号问题

                    //2017-01-10 处理../相对路径的问题，当前项目下应该没有相对路径的问题
                    if (cssContent.Contains("../"))
                    {
                        var arr = new string[] { "jquery.plugin" };
                        foreach (var a in arr)
                        {
                            if (filepath.Contains(a))
                            {
                                var path = "/" + filepath.Substring(filepath.IndexOf(a));
                                //Regex r = new Regex("('\\.\\.\\/.*?')|(\"\\.\\.\\/.*?\")");
                                Regex r = new Regex("('\\.\\.\\/.*?')");//上面已经将所有的双引号都替换成了单引号，所以不存在双引号的问题
                                Match m = r.Match(cssContent);

                                while (m.Value != null && m.Value != "")
                                {
                                    cssContent = cssContent.Replace(m.Value, "'//" + ip + "/" + CommonClass.FileUtil.mergePath(path, m.Value.Replace("'", "")) + "'");
                                    m = m.NextMatch();
                                }
                                cssContent = cssContent.Replace("./", "");
                            }
                        }
                    }
                    //逻辑上来说，这里应该只有在成功读取之后才缓存，如果读取错误，则不缓存结果，缓存一个读取失败貌似也没啥意义
                    if (Global.isCache)
                    {
                        CommonClass.CacheUtil.SetCache(filepath, cssContent);
                    }
                }

            }
            else
            {
                cssContent = cachecss.ToString();
            }
            return cssContent;

        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}