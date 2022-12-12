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
                public static string Build = "10217";
                public static DateTime LastUpdate = new DateTime(2022, 12, 12, 14, 00, 00);
            }

            public static class TOD
            {
                public static string ApplicationName = @"DMT Toll of Duty Application (Plaza)";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "10198";
                public static DateTime LastUpdate = new DateTime(2022, 10, 17, 19, 00, 00);
            }

            public static class Account
            {
                public static string ApplicationName = @"DMT Toll Admin Application (Account)";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "10215";
                public static DateTime LastUpdate = new DateTime(2022, 11, 10, 12, 00, 00);
            }

            public static class PlazaConfig
            {
                public static string ApplicationName = @"DMT TOD-TA Plaza Config";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "10100";
                public static DateTime LastUpdate = new DateTime(2021, 09, 21, 17, 00, 00);
            }

            public static class PlazaSumulator
            {
                public static string ApplicationName = @"DMT TOD-TA Plaza Simulator";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "10100";
                public static DateTime LastUpdate = new DateTime(2021, 09, 21, 17, 00, 00);
            }

            public static class PlazaWorkBench
            {
                public static string ApplicationName = @"DMT TOD-TA Plaza WorkBench";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "10100";
                public static DateTime LastUpdate = new DateTime(2021, 09, 21, 17, 00, 00);
            }

            public static class PlazaServiceManager
            {
                public static string ApplicationName = @"DMT TOD-TA Plaza Service Manager";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "10100";
                public static DateTime LastUpdate = new DateTime(2021, 09, 21, 17, 00, 00);
            }

            public static class DCImporter
            {
                public static string ApplicationName = @"DMT DC Importer";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "10000";
                public static DateTime LastUpdate = new DateTime(2022, 07, 17, 12, 10, 00);
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
