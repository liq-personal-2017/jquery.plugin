using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace jquery.plugin
{
    public class Global : System.Web.HttpApplication
    {
        //严禁在global中写属性元素给外部元素调用（只读可以）
        private static string localhostip = null;
        public static string LocalhostIp
        {
            get
            {
                return localhostip;
            }
        }

        private static bool iscache = true;

        public static bool isCache
        {
            get { return iscache; }
            set { iscache = value; }
        }


        protected void Application_Start(object sender, EventArgs e)
        {
            localhostip = System.Configuration.ConfigurationManager.AppSettings["LocalhostIp"]?.ToString();
            if (localhostip == null)
            {
                //localhostip = System.这里不能自动获取，否则如果通过外网映射进来的请求，反而不能执行，所以写个清晰的没有配置ip地址对于解决问题会更方便
                localhostip = "没有配置ip地址";
            }
            var iscache = "";
            iscache = System.Configuration.ConfigurationManager.AppSettings["isCache"]?.ToString();
            if (iscache == "false")
            {
                isCache = false;
                CommonClass.CacheUtil.Clear();
            }
            else
            {
                isCache = true;
            }

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}