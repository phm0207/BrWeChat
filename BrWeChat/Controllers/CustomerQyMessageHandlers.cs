using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Senparc.Weixin.Context;
using Senparc.Weixin.QY;
using Senparc.Weixin.QY.Entities;
using Senparc.Weixin.QY.MessageHandlers;

namespace BrWeChat.Controllers
{
    public class CustomerQyMessageHandlers : QyMessageHandler<MessageContext<IRequestMessageBase, IResponseMessageBase>>
    {
        public CustomerQyMessageHandlers(XDocument requestDoc, PostModel postModel, int maxRecordCount = 0)
            : base(requestDoc, postModel, maxRecordCount)
        {
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage =
               ResponseMessageBase.CreateFromRequestMessage(RequestMessage, ResponseMsgType.Text) as
               ResponseMessageText;
            responseMessage.Content = "文字信息";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
                case "OneClick":
                    {
                        //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        reponseMessage = strongResponseMessage;

                        //List<Article> articles = new List<Article>();
                        Article article = new Article();
                        article.Description = "这是一个描述信息，可以链接到www.baidu.com";
                        article.Url = "www.baidu.com";
                        article.Title = "这是一个标题信息";

                        strongResponseMessage.Articles.Add(article);

                        article.Description = "这是第二个描述信息，可以链接到www.qq.com";
                        article.Url = "www.qq.com";
                        article.Title = "这是第二个标题信息";
                        strongResponseMessage.Articles.Add(article);                        
                    }
                    break;
                case "SubClickRoot_Text":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "您点击了子菜单按钮。";
                    }
                    break;
                case "SubClickRoot_News":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Articles.Add(new Article()
                        {
                            Title = "您点击了子菜单图文按钮",
                            Description = "您点击了子菜单图文按钮，这是一条图文信息。",
                            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg",
                            Url = "http://weixin.senparc.com"
                        });
                    }
                    break;
                //case "SubClickRoot_Music":
                //    {
                //        //上传缩略图
                //        var accessToken = CommonAPIs.AccessTokenContainer.TryGetAccessToken(appId, appSecret);
                //        var uploadResult = AdvancedAPIs.MediaApi.UploadTemporaryMedia(accessToken, UploadMediaFileType.thumb,
                //                                                     Server.GetMapPath("~/Images/Logo.jpg"));
                //        //设置音乐信息
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageMusic>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Music.Title = "天籁之音";
                //        strongResponseMessage.Music.Description = "真的是天籁之音";
                //        strongResponseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
                //        strongResponseMessage.Music.HQMusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
                //        strongResponseMessage.Music.ThumbMediaId = uploadResult.thumb_media_id;
                //    }
                //    break;
                //case "SubClickRoot_Image":
                //    {
                //        //上传图片
                //        var accessToken = CommonAPIs.AccessTokenContainer.TryGetAccessToken(appId, appSecret);
                //        var uploadResult = AdvancedAPIs.MediaApi.UploadTemporaryMedia(accessToken, UploadMediaFileType.image,
                //                                                     Server.GetMapPath("~/Images/Logo.jpg"));
                //        //设置图片信息
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Image.MediaId = uploadResult.media_id;
                //    }
                //    break;
                //case "SubClickRoot_Agent"://代理消息
                //    {
                //        //获取返回的XML
                //        DateTime dt1 = DateTime.Now;
                //        reponseMessage = MessageAgent.RequestResponseMessage(this, agentUrl, agentToken, RequestDocument.ToString());
                //        //上面的方法也可以使用扩展方法：this.RequestResponseMessage(this,agentUrl, agentToken, RequestDocument.ToString());

                //        DateTime dt2 = DateTime.Now;

                //        if (reponseMessage is ResponseMessageNews)
                //        {
                //            (reponseMessage as ResponseMessageNews)
                //                .Articles[0]
                //                .Description += string.Format("\r\n\r\n代理过程总耗时：{0}毫秒", (dt2 - dt1).Milliseconds);
                //        }
                //    }
                //    break;
                //case "Member"://托管代理会员信息
                //    {
                //        //原始方法为：MessageAgent.RequestXml(this,agentUrl, agentToken, RequestDocument.ToString());//获取返回的XML
                //        reponseMessage = this.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
                //    }
                //    break;
                //case "OAuth"://OAuth授权测试
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                //        strongResponseMessage.Articles.Add(new Article()
                //        {
                //            Title = "OAuth2.0测试",
                //            Description = "点击【查看全文】进入授权页面。\r\n注意：此页面仅供测试（是专门的一个临时测试账号的授权，并非Senparc.Weixin.MP SDK官方账号，所以如果授权后出现错误页面数正常情况），测试号随时可能过期。请将此DEMO部署到您自己的服务器上，并使用自己的appid和secret。",
                //            Url = "http://weixin.senparc.com/oauth2",
                //            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg"
                //        });
                //        reponseMessage = strongResponseMessage;
                //    }
                //    break;
                //case "Description":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //        strongResponseMessage.Content = GetWelcomeInfo();
                //        reponseMessage = strongResponseMessage;
                //    }
                //    break;
                //case "SubClickRoot_PicPhotoOrAlbum":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Content = "您点击了【微信拍照】按钮。系统将会弹出拍照或者相册发图。";
                //    }
                //    break;
                //case "SubClickRoot_ScancodePush":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Content = "您点击了【微信扫码】按钮。";
                //    }
                //    break;
                //case "ConditionalMenu_Male":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Content = "您点击了个性化菜单按钮，您的微信性别设置为：男。";
                //    }
                //    break;
                //case "ConditionalMenu_Femle":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Content = "您点击了个性化菜单按钮，您的微信性别设置为：女。";
                //    }
                //    break;
                default:
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                    break;
            }

            return reponseMessage;       
        }

        /// <summary>
        /// 默认消息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这是一条默认消息。";
            return responseMessage;
        }
    }
}
