using System;
using System.IO;
using System.Web.Mvc;
using System.Xml.Linq;
using Brightsea.Common;
using Senparc.Weixin.MP.MvcExtension;
using Senparc.Weixin.QY;
using Senparc.Weixin.QY.Entities;
using Tencent;

namespace BrWeChat.Controllers
{
    public class WeiXinCbController : Controller
    {
        ///// <summary>
        ///// 首次验证
        ///// </summary>
        ///// <param name="msg_signature"></param>
        ///// <param name="timestamp"></param>
        ///// <param name="nonce"></param>
        ///// <param name="echostr"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[ActionName("Index")]
        ////public ActionResult Get(PostModel postModel, string echostr)
        //public ActionResult Get(string msg_signature, string timestamp, string nonce, string echostr)
        ////public ActionResult Index(PostModel postModel, string echostr)
        //{
        //    //if (postModel == null)
        //    //{
        //    //    Log.Instance.CommonLog("PostModel Null");
        //    //}
        //    //else
        //    //Log.Instance.CommonLog(string.Format("Token:{0}, EncodeingAESKey:{1}, CorpIp:{2}, Msg_Signature:{3},"
        //    //    + "Timestamp:{4}, Nonce:{5}", 
        //    //    postModel.Token ?? "Null",
        //    //    postModel.EncodingAESKey ?? "Null",
        //    //    postModel.CorpId ?? "Null",
        //    //    postModel.Msg_Signature ?? "Null",
        //    //    postModel.Timestamp ?? "Null",
        //    //    postModel.Nonce ?? "Null"));
        //    try
        //    {
        //        PostModel postModel = new PostModel();
        //        postModel.Token = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.Token;
        //        postModel.EncodingAESKey = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.EncodingAESKey;
        //        postModel.CorpId = GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID;
        //        postModel.Msg_Signature = msg_signature;
        //        postModel.Timestamp = timestamp;
        //        postModel.Nonce = nonce;

        //        //if (postModel == null)
        //        //{
        //        //    Log.Instance.CommonLog("PostModel Null");
        //        //}
        //        //else
        //        //    Log.Instance.CommonLog(string.Format("Token:{0}, EncodeingAESKey:{1}, CorpIp:{2}, Msg_Signature:{3},"
        //        //        + "Timestamp:{4}, Nonce:{5}",
        //        //        postModel.Token ?? "Null",
        //        //        postModel.EncodingAESKey ?? "Null",
        //        //        postModel.CorpId ?? "Null",
        //        //        postModel.Msg_Signature ?? "Null",
        //        //        postModel.Timestamp ?? "Null",
        //        //        postModel.Nonce ?? "Null"));

        //        string sEchoStr = Signature.VerifyURL(postModel.Token, postModel.EncodingAESKey, postModel.CorpId,
        //            postModel.Msg_Signature,
        //            postModel.Timestamp, postModel.Nonce, echostr);
        //        return Content(sEchoStr);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance.ErrorLog(ex);
        //        throw;
        //    }
            
        //}

        /// <summary>
        /// 微信后台验证地址（使用Get），微信企业后台应用的“修改配置”的Url填写如：http://weixin.senparc.com/qy
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string msg_signature = "", string timestamp = "", string nonce = "", string echostr = "")
        {

            //        postModel.Token = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.Token;
            //        postModel.EncodingAESKey = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.EncodingAESKey;
            //        postModel.CorpId = GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID;

            //return Content(echostr); //返回随机字符串则表示验证通过
            var verifyUrl = Signature.VerifyURL(GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.Token,
                GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.EncodingAESKey,
                GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID, msg_signature, timestamp, nonce,
                echostr);
            if (verifyUrl != null)
            {
                return Content(verifyUrl); //返回解密后的随机字符串则表示验证通过
            }
            else
            {
                return Content("如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 微信后台验证地址（使用Post），微信企业后台应用的“修改配置”的Url填写如：http://weixin.senparc.com/qy
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            var maxRecordCount = 10;

            postModel.Token = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.Token;
            postModel.EncodingAESKey = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.EncodingAESKey;
            postModel.CorpId = GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID;

            //Log.Instance.CommonLog(string.Format("Token:{0}, EncodeingAESKey:{1}, CorpIp:{2}, Msg_Signature:{3},"
            //                                     + "Timestamp:{4}, Nonce:{5}",
            //    postModel.Token ?? "Null",
            //    postModel.EncodingAESKey ?? "Null",
            //    postModel.CorpId ?? "Null",
            //    postModel.Msg_Signature ?? "Null",
            //    postModel.Timestamp ?? "Null",
            //    postModel.Nonce ?? "Null"));

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new QyCustomMessageHandler(Request.InputStream, postModel, maxRecordCount);

            if (messageHandler.RequestMessage == null)
            {
                //验证不通过或接受信息有错误
            }

            try
            {
                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                //Log.Instance.CommonLog(messageHandler.RequestDocument.ToString());

                //messageHandler.RequestDocument.Save(FileDirPath.AppPath() + DateTime.Now.Ticks + "_Request_" + messageHandler.RequestMessage.FromUserName + ".txt");
                //执行微信处理过程
                messageHandler.Execute();
                //测试时可开启，帮助跟踪数据
                //Log.Instance.CommonLog(messageHandler.ResponseDocument.ToString());
                //messageHandler.ResponseDocument.Save(FileDirPath.AppPath() + DateTime.Now.Ticks + "_Response_" + messageHandler.ResponseMessage.ToUserName + ".txt");
                //messageHandler.FinalResponseDocument.Save(FileDirPath.AppPath() + DateTime.Now.Ticks + "_FinalResponse_" + messageHandler.ResponseMessage.ToUserName + ".txt");
                
                //自动返回加密后结果
                return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
            }
            catch (Exception ex)
            {
                using (TextWriter tw = new StreamWriter(FileDirPath.AppPath() + DateTime.Now.Ticks + ".txt"))
                {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);
                    //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                    if (messageHandler.FinalResponseDocument != null)
                    {
                        tw.WriteLine(messageHandler.FinalResponseDocument.ToString());
                    }
                    tw.Flush();
                    tw.Close();
                }
                return Content("");
            }
        }

        /// <summary>
        /// 这是一个最简洁的过程演示
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MiniPost(PostModel postModel)
        {
            var maxRecordCount = 10;

            postModel.Token = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.Token;
            postModel.EncodingAESKey = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.EncodingAESKey;
            postModel.CorpId = GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new QyCustomMessageHandler(Request.InputStream, postModel, maxRecordCount);
            //执行微信处理过程
            messageHandler.Execute();
            //自动返回加密后结果
            return new FixWeixinBugWeixinResult(messageHandler);
        }

        /// <summary>
        /// 后续调用
        /// </summary>
        /// <param name="msg_signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ActionName("Index")]
        //public ActionResult Post(PostModel postModel)
        ////public ActionResult Index(string msg_signature, string timestamp, string nonce)
        ////public ActionResult Index(PostModel postModel)
        //{
        //    //PostModel postModel = new PostModel();
        //    postModel.Token = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.Token;
        //    postModel.EncodingAESKey = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.EncodingAESKey;
        //    postModel.CorpId = GlobalData.Instance.SiteConfig.WeiXinConfig.CorpID;

        //    //postModel.Msg_Signature = msg_signature;
        //    //postModel.Timestamp = timestamp;
        //    //postModel.Nonce = nonce;

        //    string msg = Signature.GenarateSinature(postModel.Token, postModel.Timestamp, postModel.Nonce,
        //        postModel.Msg_Signature);

        //    postModel.Token = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.Token;
        //    postModel.EncodingAESKey = GlobalData.Instance.SiteConfig.WeiXinCallBackConfig.EncodingAESKey;

        //    Log.Instance.CommonLog(string.Format("Token:{0}, EncodeingAESKey:{1}, CorpIp:{2}, Msg_Signature:{3},"
        //                                         + "Timestamp:{4}, Nonce:{5}, Msg:{6}",
        //        postModel.Token ?? "Null",
        //        postModel.EncodingAESKey ?? "Null",
        //        postModel.CorpId ?? "Null",
        //        postModel.Msg_Signature ?? "Null",
        //        postModel.Timestamp ?? "Null",
        //        postModel.Nonce ?? "Null",
        //        msg ?? "Null"));
            

        //    //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
        //    var maxRecordCount = 10;

        //    var logPath = Server.MapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
        //    if (!Directory.Exists(logPath))
        //    {
        //        Directory.CreateDirectory(logPath);
        //    }

        //    //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
        //    var messageHandler = new QyCustomMessageHandler(Request.InputStream, postModel, maxRecordCount);


        //    try
        //    {
        //        //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
        //        messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName)));
        //        if (messageHandler.UsingEcryptMessage)
        //        {
        //            messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName)));
        //        }

        //        /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
        //         * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
        //        messageHandler.OmitRepeatedMessage = true;


        //        //执行微信处理过程
        //        messageHandler.Execute();

        //        //测试时可开启，帮助跟踪数据

        //        //if (messageHandler.ResponseDocument == null)
        //        //{
        //        //    throw new Exception(messageHandler.RequestDocument.ToString());
        //        //}

        //        if (messageHandler.ResponseDocument != null)
        //        {
        //            messageHandler.ResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName)));
        //        }

        //        if (messageHandler.UsingEcryptMessage)
        //        {
        //            //记录加密后的响应信息
        //            messageHandler.FinalResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_Final_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName)));
        //        }

        //        //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
        //        return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
        //        //return new WeixinResult(messageHandler);//v0.8+
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance.ErrorLog(ex);
        //        return Content("");
        //    }
        //}
    }
}
