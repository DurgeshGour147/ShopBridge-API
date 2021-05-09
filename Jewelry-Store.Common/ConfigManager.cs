using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jewelry_Store.Common
{
    public class ConfigManager
    {
        public static IConfiguration Configuration { get; set; }
        private static ConfigManager _instance = null;
        private ConfigManager()
        {
        }
        public static ConfigManager Instance
        {
            get
            {
                if (_instance.IsNull())
                    _instance = new ConfigManager();
                return _instance;
            }
        }
        private string GetStringValue(string key)
        {
            string keyValue = Configuration["AppSettings:" + key];
            if (string.IsNullOrEmpty(keyValue))
                throw new Exception(ErrorConstant.ConfigurationKeyMissing);

            return keyValue;
        }

        private bool GetBoolValue(string key) => bool.Parse(GetStringValue(key));

        public bool IsStaging => GetBoolValue(AppSettingConstant.IsStaging);
        public string JewelryStoreConnectionString => Configuration.GetConnectionString(IsStaging ? AppSettingConstant.CONNECTION_STRING_STAGING : AppSettingConstant.CONNECTION_STRING_PRODUCTION);

        public string EncryptionKey => GetStringValue(AppSettingConstant.EncryptionKey);

    }
}
