using System.Configuration;

namespace XSquareCalculationsApi.Configs
{
    public static class JwtSettingsConfig
    {
        public static string SecretKey { get { return GetAppSetting("SecretKey"); } }
        public static string SiteUrl { get { return GetAppSetting("SiteUrl"); } }
        public static double ExpiredDay { get { return double.Parse(GetAppSetting("ExpiredDay")); } }

        private static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}