using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatelAPI.Areas.Config
{
    public interface IConfigProvider
    {
        string CTSConnectionString { get; set; }

        string SageConnectionString { get; set; }

        bool SageLogging { get; set; }

        string CTSConnectionStringLive { get; set; }

        string SageConnectionStringLive { get; set; }

        string DatelSystemID { get; set; }

    }
}
