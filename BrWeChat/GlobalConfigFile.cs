namespace BrWeChat
{
    public class GlobalConfigFile
    {
        private readonly string _globalConfigDir = Brightsea.Common.FileDirPath.AppPath() + "Config\\";

        public string GlobalConfigDir
        {
            get { return _globalConfigDir; }
        }

        private string _configFile;

        public string ConfigFile
        {
            get { return _configFile; }
        }

        public void Init()
        {
            _configFile = _globalConfigDir + "Config.xml";
        }
    }
}