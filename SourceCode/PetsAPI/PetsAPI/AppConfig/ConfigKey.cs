using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PetsAPI.AppConfig
{
    public static class ConfigKey
    {
        public static string EMAIL = ConfigurationManager.AppSettings["EMAIL"];
        public static string PASSWORD = ConfigurationManager.AppSettings["PASSWORD"];
        public static string MAILTITLE = ConfigurationManager.AppSettings["MAILTITLE"];
    }
}