using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace DatelAPI.Areas.Data
{
    public interface IHubData
    {
        Task<SqlDataReader> OpenQuerySage(SqlCommand _cmd);

        Task RunQuerySage(SqlCommand _cmd);

        Task<SqlDataReader> OpenQueryCTS(SqlCommand _cmd);

        Task RunQueryCTS(SqlCommand _cmd);
    }
}
