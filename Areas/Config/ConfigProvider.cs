using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatelAPI.Areas.Config
{
    public class ConfigProvider: IConfigProvider
    {
        public ConfigProvider(string ctsConnection, string sageConnection, bool sageLogging, string datelSystemID)
        {
            CTSConnectionString = ctsConnection;
            SageConnectionString = sageConnection;
            SageLogging = sageLogging;
            DatelSystemID = datelSystemID;
        }

        public string CTSConnectionString { get; set; }

        public string SageConnectionString { get; set; }

        public bool SageLogging { get; set; }

        public string DatelSystemID { get; set; }
    }
}