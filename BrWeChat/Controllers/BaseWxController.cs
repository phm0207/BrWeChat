using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Brightsea.Common;
using BrWeChat.Entities;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.QY.CommonAPIs;

namespace BrWeChat.Controllers
{
    public class BaseWxController : Controller
    {
        //
        // GET: /Base/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeActionResult"></param>
        /// <param name="userId"></param>
        /// <returns>0:已有cookie，返回UserId信息，1：没有cookie，但是Request中有code信息，通过code信息获得UserId信息，
        /// 2：没有cookie，Request中也没有code信息，返回获取code的链接信息（Action）。
        /// 
        /// </returns>
        protected int GetUserInfo(out ActionResult codeActionResult, out string userId)
        {
            codeActionResult = null;
            userId = "";

            string code = "";
            string corpId = GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID;
            string corpSecret = GlobalData.Instance.SiteConfig.WeiXinConfig.CorpSecret;

            bool haveCookie = Request.Cookies["UserId"] != null && !string.IsNullOrEmpty(Request.Cookies["UserId"].Value);

            if (!haveCookie)
            {
                code = Request.QueryString["code"];
                corpId = GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID;
                if (string.IsNullOrEmpty(code))
                {
                    // 重定向到微信根据Code获取UserId页面
                    // ViewBag.UserId = 

                    string url =
                        string.Format(
                            "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=",
                            corpId, Request.Url);
                    {
                        codeActionResult = new RedirectResult(url);
                        return 2;
                    }
                }
                else
                {
                    // 重定向到微信根据Code获取UserId页面
                    // ViewBag.UserId = 
                    if (!AccessTokenContainer.CheckRegistered(GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID))
                    {
                        AccessTokenContainer.Register(corpId, corpSecret);
                    }

                    string accessToken = AccessTokenContainer.GetToken(corpId);
                    string url =
                        string.Format(
                            "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}",
                            accessToken, code);

                    string returnText = RequestUtility.HttpGet(url, null);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    if (returnText.Contains("errcode"))
                    {
                        //可能发生错误
                        WxJsonResult errorResult = js.Deserialize<WxJsonResult>(returnText);

                        //发生错误
                        throw new ErrorJsonResultException(
                            string.Format("微信请求发生错误！错误代码：{0}，说明：{1}",
                                (int)errorResult.errcode, errorResult.errmsg), null, errorResult, url);
                    }
                    else if (returnText.Contains("UserId"))
                    {
                        WxUserIdResult userIdResult = js.Deserialize<WxUserIdResult>(returnText);
                        userId = userIdResult.UserId;
                        ViewBag.UserId = userIdResult.UserId;
                        // 保存cookie信息
                        HttpCookie ck = new HttpCookie("UserId", userIdResult.UserId);

                        ck.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(ck);
                        return 1;
                    }
                    else
                    {
                        WxOpenIdResult openIdResult = js.Deserialize<WxOpenIdResult>(returnText);
                        throw new Exception(
                            string.Format("没有权限访问！"));
                    }
                }
            }
            else
            {
                HttpCookie ck = Request.Cookies["UserId"];
                Log.Instance.InfoLog4Net(ck == null ? "null" : ck.Value);
                userId = ck.Value;
                ck.Expires = DateTime.Now.AddDays(1);
                return 0;
            }
        }

    }
}
