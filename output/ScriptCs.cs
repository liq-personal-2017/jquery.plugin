
using System.Web;
using System.Text;
using jquery.plugin.CommonClass;

namespace jquery.plugin.output
{
    public class ScriptCs : IHttpHandler
    {
        bool IHttpHandler.IsReusable
        {
            get
            {
                return true;
            }
        }

        private string ip = Global.LocalhostIp;

        public void ProcessRequest(HttpContext context)
        {
            string url = context.Request.Path;
            context.Response.ContentType = "text/javascript";
            StringBuilder content = new StringBuilder(5000);

            string filepath = context.Server.MapPath(url);
            object filecontent = CacheUtil.GetCache(url);

            if (filecontent == null || context.Request.Params["rc"] == "1" || Global.isCache == false)
            {
                filecontent = FileUtil.readFile(filepath);
                if (filecontent != null)
                {
                    CacheUtil.SetCache(url, filecontent);
                }
                else {
                    filecontent = $"console.log('{url} load error')";
                }
            }
            filecontent = filecontent.ToString().Replace("[localhostIp]", ip);
            context.Response.Write(filecontent.ToString());
            context.Response.Flush();

        }
    }
}