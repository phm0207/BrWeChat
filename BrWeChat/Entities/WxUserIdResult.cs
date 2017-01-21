namespace BrWeChat.Entities
{
    public class WxUserIdResult
    {
        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _deviceId;
        public string DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }
    }

    public class WxOpenIdResult
    {
        private string _openId;
        public string OpenId
        {
            get { return _openId; }
            set { _openId = value; }
        }

        private string _deviceId;
        public string DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }
    }
}