using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSoup.Common
{
    public class Constants
    {
        public const String API_SUBSCRIPTION_TYPE_NO_LIMIT = "NO_LIMIT";
        public const String API_SUBSCRIPTION_TYPE_MONTHLY_RESET = "MONTHLY_RESET";
        public const String API_SUBSCRIPTION_TYPE_DAILY_RESET = "DAILY_RESET";
        public const int API_ACCESS_METRIC_NO_LIMIT = -1;
        public const int API_ACCESS_METRIC_LIMIT_DAILY = 20000;
        public const int API_ACCESS_METRIC_LIMIT_MINUTE = 200;

        public static String ROLES_ADMINISTRATOR = "administrator";
        public static List<String> SystemRoles()
        {
            return new List<String>()
            {
                ROLES_ADMINISTRATOR
            };
        }


    }
}