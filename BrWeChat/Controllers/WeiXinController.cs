using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Brightsea.Common;
using Senparc.Weixin.QY.Entities;

namespace BrWeChat.Controllers
{
    public class WeiXinController : BaseWxController
    {
        //
        // GET: /WeiXin/
        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Instance.ErrorLog4Net("WeiXinAction", "WeiXinAction", filterContext.Exception);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
