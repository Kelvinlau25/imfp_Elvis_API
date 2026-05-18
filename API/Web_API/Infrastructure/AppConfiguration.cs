using Microsoft.Extensions.Configuration;

namespace tms_acl_api.Infrastructure
{
    public static class AppConfiguration
    {
        public static IConfiguration Current { get; set; }

        public static string GetAppSetting(string key)
            => Current?[$"AppSettings:{key}"];

        public static string GetConnectionString(string name)
            => Current?.GetConnectionString(name);
    }
}
