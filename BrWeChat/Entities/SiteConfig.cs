using System.Xml;
using Brightsea.Common;

namespace BrWeChat.Entities
{
    public class WeiXinConfig
    {
        private string _corpId;
        /// <summary>
        /// 企业Id
        /// </summary>
        public string CorpID
        {
            get { return _corpId; }
            set { _corpId = value; }
        }

        private string _corpSecret;
        public string CorpSecret
        {
            get { return _corpSecret; }
            set { _corpSecret = value; }
        }

        

        public void LoadFromXmlNode(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            _corpId = XMLProcess.GetXmlNodeValue(node, "CorpId", "");
            _corpSecret = XMLProcess.GetXmlNodeValue(node, "CorpSecret", "");
        }
    }

    public class WeiXinCallBackConfig
    {
        private string _token;
        /// <summary>
        /// 企业Id
        /// </summary>
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        private string _encodingAESKey;
        public string EncodingAESKey
        {
            get { return _encodingAESKey; }
            set { _encodingAESKey = value; }
        }



        public void LoadFromXmlNode(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            _token = XMLProcess.GetXmlNodeValue(node, "Token", "");
            _encodingAESKey = XMLProcess.GetXmlNodeValue(node, "EncodingAESKey", "");
        }
    }



    public class SiteConfig
    {
        private readonly WeiXinConfig _weiXinConfig = new WeiXinConfig();
        public WeiXinConfig WeiXinConfig
        {
            get { return _weiXinConfig; }
        }

        private readonly WeiXinCallBackConfig _weiXinCallBackConfig = new WeiXinCallBackConfig();
        public WeiXinCallBackConfig WeiXinCallBackConfig
        {
            get { return _weiXinCallBackConfig; }
        }
        
        public void Init(XmlNode node)
        {
            XmlNode wxNode = node.SelectSingleNode("WeiXinConfig");
            _weiXinConfig.LoadFromXmlNode(wxNode);

            _weiXinCallBackConfig.LoadFromXmlNode(wxNode);
        }
    }
}