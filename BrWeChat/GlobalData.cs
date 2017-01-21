using System.Reflection;
using System.Xml;
using Brightsea.Common;
using BrWeChat.Entities;

namespace BrWeChat
{
    public class GlobalData
    {
        private class Nested
        {
            static Nested()
            {
                
            }
            internal static readonly GlobalData instance = new GlobalData();
        }

        public static GlobalData Instance
        {
            get { return Nested.instance; }
        }

        private readonly GlobalConfigFile _configFile = new GlobalConfigFile();
        public GlobalConfigFile ConfigFile
        {
            get { return _configFile; }            
        }

        //private IWeiXinDal _weiXinDal;
        //public IWeiXinDal WeiXinDal
        //{
        //    get { return _weiXinDal; }        
        //}

        private readonly SiteConfig _siteConfig = new SiteConfig();
        public SiteConfig SiteConfig
        {
            get { return _siteConfig; }
        }
        
        public void Init()
        {
            _configFile.Init();
            XmlDocument xml = new XmlDocument();
            xml.Load(_configFile.ConfigFile);
            //XmlNode dalNode = xml.SelectSingleNode("Config/DalConfig/IWeiXinDal");

            //AssemblyParam p = new AssemblyParam();
            //p.LoadParam(dalNode);
            //string fileName = FileDirPath.GetAbsFileName(p.FileName);
            //Assembly assembly = Assembly.LoadFile(fileName);
            //_weiXinDal = (IWeiXinDal)assembly.CreateInstance(p.ClassName);

            XmlNode scNode = xml.SelectSingleNode("Config/SiteConfig");
            _siteConfig.Init(scNode);

            //string weiXinDalFileDir = Path.GetDirectoryName(fileName) + "\\";
            //string dbConFile = weiXinDalFileDir + "Config.xml";
            //string sqlFile = weiXinDalFileDir + "SqlConfig.xml";

            //XmlDocument dbXml = new XmlDocument(); 
            //dbXml.Load(dbConFile);
            //XmlNode dbConNode = dbXml.DocumentElement.SelectSingleNode("IDatabaseAccess");

            //XmlDocument sqlConXml = new XmlDocument();
            //sqlConXml.Load(sqlFile);
            //XmlNode sqlConNode = sqlConXml.SelectSingleNode("SqlConfig");
            //_weiXinDal.Init(dbConNode, sqlConNode);
        }
    }
}