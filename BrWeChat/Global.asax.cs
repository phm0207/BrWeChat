using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Brightsea.Common;

namespace BrWeChat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalData.Instance.Init();
        }

        /// <summary>
        /// 全局的异常处理
        /// </summary>
        public void ExceptionHandlerStarter(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            Log.Instance.ErrorLog4Net("未处理错误", "GlobalError", ex);
            var httpStatusCode = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误  
            var httpContext = ((MvcApplication)sender).Context;
            string s = HttpContext.Current.Request.Url.ToString();

            HttpServerUtility server = HttpContext.Current.Server;
            if (ex != null)
            {
                Application["LastError"] = ex;
                int statusCode = HttpContext.Current.Response.StatusCode;
                string exceptionOperator = "/SysException/Error";

                try
                {
                    if (!String.IsNullOrEmpty(exceptionOperator))
                    {
                        exceptionOperator = new System.Web.UI.Control().ResolveUrl(exceptionOperator);
                        string url = string.Format("{0}?ErrorUrl={1}", exceptionOperator, server.UrlEncode(s));
                        string script = String.Format("<script language='javascript' type='text/javascript'>window.top.location='{0}';</script>", url);
                        Response.Write(script);
                        Response.End();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 全局的异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //#if DEBUG
            //            //调试状态不进行异常跟踪
            //#else
            ExceptionHandlerStarter(sender, e);
            //#endif
        }
    }
}
