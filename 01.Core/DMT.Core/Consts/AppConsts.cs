#region Using

using System;

#endregion

namespace DMT
{
    public static class AppConsts
    {
        // common properties
        public static string Version = "1";
        public static string Minor = "4";

        public static class Application
        {
            public static class TA
            {
                public static string ApplicationName = @"DMT Toll Admin Application (Plaza)";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = Build = "10060";
                public static DateTime LastUpdate = new DateTime(2021, 06, 24, 06, 00, 00);
            }

            public static class TOD
            {
                public static string ApplicationName = @"DMT Toll of Duty Application (Plaza)";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = Build = "10070";
                public static DateTime LastUpdate = new DateTime(2021, 06, 29, 09, 00, 00);
            }
            
            public static class Account
            {
                public static string ApplicationName = @"DMT Toll Admin Application (Account)";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = Build = "10060";
                public static DateTime LastUpdate = new DateTime(2021, 06, 24, 06, 00, 00);
            }
            
            public static class PlazaConfig
            {
                public static string ApplicationName = @"DMT TOD-TA Plaza Config";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = Build = "10060";
                public static DateTime LastUpdate = new DateTime(2021, 06, 24, 06, 00, 00);
            }

            public static class PlazaSumulator
            {
                public static string ApplicationName = @"DMT TOD-TA Plaza Simulator";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = Build = "10060";
                public static DateTime LastUpdate = new DateTime(2021, 06, 24, 06, 00, 00);
            }

            public static class PlazaWorkBench
            {
                public static string ApplicationName = @"DMT TOD-TA Plaza WorkBench";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = Build = "10060";
                public static DateTime LastUpdate = new DateTime(2021, 06, 24, 06, 00, 00);
            }

            public static class PlazaServiceManager
            {
                public static string ApplicationName = @"DMT TOD-TA Plaza Service Manager";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = Build = "10060";
                public static DateTime LastUpdate = new DateTime(2021, 06, 24, 06, 00, 00);
            }
        }
        /*
        public static class WindowsService
        {
            public static class Local
            {
                public static string ServiceName = "DMT Local REST API Service";
                public static string DisplayName = "DMT Local REST API Service";
                public static string Description = "DMT Local REST API Service";
                public static string ExecutableFileName = @"DMT.Local.Web.Services.exe";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = AppConsts.Build;
                public static DateTime LastUpdate = AppConsts.LastUpdate;
            }
        }
        */
    }
}
