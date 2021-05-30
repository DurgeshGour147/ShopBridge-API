using System;

namespace Shop_Bridge.Common
{

    public class CommonConstant
    {

        public const string AppName = "ShopBridge";
    }
    public class ErrorConstant
    {
        public const string ConfigurationKeyMissing = "Are you crazy, this configuration key doesn't exists!!";
        public const string PermissionDenied = "You don't have permission to this action";
        public const string AccessTokenNotFound = "AccessToken not found";
        public const string InvalidAccessToken = "Invalid access token";
        public const string InValidRequest = "Invalid request";
        public const string InValidToken = "Invalid token";
        public const string AuthenticationDenied = "Authentication denied";
        public const string ExpiryToken = "Token has been expiry";
        public const string NoDataFound = "No data found";
        public const string InValidPassword = "Password has been wrong";
    }
    public class AppSettingConstant
    {
        public const string IsStaging = "IsStaging";
        public const string CONNECTION_STRING_STAGING = "CONNECTION_STRING_STAGING";
        public const string CONNECTION_STRING_PRODUCTION = "CONNECTION_STRING_PRODUCTION";
        public const string EncryptionKey = "EncryptionKey";
    }
    public class CacheConstant
    {
        public const int CacheExpireTimeInHours = 10;
        public const string RoleTableCache = "RoleTableCache";
    }

    public class AccessTokenKey
    {
        public const string UserName = "UserName";
        public const string Roles = "Roles";
        public const string IssueTime = "IssueTime";
        public const string ExpiryTime = "ExpiryTime";
        public const string AppName = "AppName";
    }
}
