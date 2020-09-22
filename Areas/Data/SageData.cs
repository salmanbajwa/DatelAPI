using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace DatelAPI.Areas.Data
{
    public class SageData: ISageData
    {
        private readonly IHubData _data;

        public SageData(IHubData data)
        {
            _data = data;
        }

        public async Task<Dictionary<string, string>> GetDBLoginDetails(string systemID)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ESP_Hub_GetDatelSystemKeys";

                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = systemID;

                    using (SqlDataReader dr = await _data.OpenQueryCTS(cmd))
                    {
                        if (dr.Read())
                        {
                            Result.Add("systemName", dr["systemName"].ToString().Trim());
                            Result.Add("dbServer", dr["dbServer"].ToString().Trim());
                            Result.Add("dbName", dr["dbName"].ToString().Trim());
                            Result.Add("dbLogin", dr["dbLogin"].ToString().Trim());
                            Result.Add("dbPassword", dr["dbPassword"].ToString().Trim());
                            Result.Add("dbScheme", dr["dbScheme"].ToString().Trim());
                            Result.Add("auditMode", dr["auditMode"].ToString().Trim());
                            Result.Add("debugMode", dr["debugMode"].ToString().Trim());
                            Result.Add("orderPrefix", dr["orderPrefix"].ToString().Trim());
                            Result.Add("orderSyskey", dr["orderSyskey"].ToString().Trim());
                            Result.Add("softAllocationOnly", dr["softAllocationOnly"].ToString().Trim());
                            Result.Add("slAllowBatchAppend", dr["slAllowBatchAppend"].ToString().Trim());
                            Result.Add("slAllowPartialAllocation", dr["slAllowPartialAllocation"].ToString().Trim());

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetDBLoginDetails");
            }
            return Result;
        }
    }
}