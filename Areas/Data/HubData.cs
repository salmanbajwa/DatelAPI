﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Serilog;
using System.IO;
using System.Collections.Generic;
using System.Text;
using DatelAPI.Areas.Config;

namespace DatelAPI.Areas.Data
{
    public class HubData : IHubData
    {
        private readonly IConfigProvider _config;

        public HubData(IConfigProvider config)
        {
            _config = config;        
        }

        public async Task<SqlDataReader> OpenQuerySage(SqlCommand _cmd)
        {
            SqlDataReader dr = null;
            SqlConnection conn = new SqlConnection(_config.SageConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                {
                    {
                        conn.Open();

                        cmd.CommandText = _cmd.CommandText;
                        cmd.CommandType = _cmd.CommandType;
                        cmd.CommandTimeout = 600;

                        cmd.Connection = conn;

                        foreach (SqlParameter pr in _cmd.Parameters)
                        {
                            cmd.Parameters.Add(pr.ParameterName, pr.SqlDbType).Value = pr.Value;
                        }

                        dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error OpenQuerySage");
                throw ex;
            }
            finally
            {

            }

            return dr;

        }

        public async Task RunQuerySage(SqlCommand _cmd)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.SageConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.CommandText = _cmd.CommandText;
                        cmd.CommandType = _cmd.CommandType;
                        cmd.CommandTimeout = 3600;

                        cmd.Connection = conn;

                        foreach (SqlParameter pr in _cmd.Parameters)
                        {
                            cmd.Parameters.Add(pr.ParameterName, pr.SqlDbType).Value = pr.Value;
                        }

                        await cmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error RunQuerySage");
                throw ex;
            }
            finally
            {

            }

        }

        public async Task<SqlDataReader> OpenQueryCTS(SqlCommand _cmd)
        {
            SqlDataReader dr = null;
            SqlConnection conn = new SqlConnection(_config.CTSConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                {
                    {
                        conn.Open();

                        cmd.CommandText = _cmd.CommandText;
                        cmd.CommandType = _cmd.CommandType;
                        cmd.CommandTimeout = 3600;

                        cmd.Connection = conn;

                        foreach (SqlParameter pr in _cmd.Parameters)
                        {
                            cmd.Parameters.Add(pr.ParameterName, pr.SqlDbType).Value = pr.Value;
                        }

                        dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error OpenQueryCTS");
                throw ex;

            }
            finally
            {

            }

            return dr;

        }

        public async Task RunQueryCTS(SqlCommand _cmd)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.CTSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.CommandText = _cmd.CommandText;
                        cmd.CommandType = _cmd.CommandType;
                        cmd.CommandTimeout = 3600;

                        cmd.Connection = conn;

                        foreach (SqlParameter pr in _cmd.Parameters)
                        {
                            cmd.Parameters.Add(pr.ParameterName, pr.SqlDbType).Value = pr.Value;
                        }

                        await cmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error RunQueryCTS");
                throw ex;
            }
            finally
            {

            }

        }


        public async Task<SqlDataReader> OpenQuerySageLive(SqlCommand _cmd)
        {
            SqlDataReader dr = null;
            SqlConnection conn = new SqlConnection(_config.SageConnectionStringLive);
            SqlCommand cmd = new SqlCommand();

            try
            {
                {
                    {
                        conn.Open();

                        cmd.CommandText = _cmd.CommandText;
                        cmd.CommandType = _cmd.CommandType;
                        cmd.CommandTimeout = 600;

                        cmd.Connection = conn;

                        foreach (SqlParameter pr in _cmd.Parameters)
                        {
                            cmd.Parameters.Add(pr.ParameterName, pr.SqlDbType).Value = pr.Value;
                        }

                        dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error OpenQuerySageLive");
                throw ex;
            }
            finally
            {

            }

            return dr;

        }

        public async Task RunQuerySageLive(SqlCommand _cmd)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.SageConnectionStringLive))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.CommandText = _cmd.CommandText;
                        cmd.CommandType = _cmd.CommandType;
                        cmd.CommandTimeout = 3600;

                        cmd.Connection = conn;

                        foreach (SqlParameter pr in _cmd.Parameters)
                        {
                            cmd.Parameters.Add(pr.ParameterName, pr.SqlDbType).Value = pr.Value;
                        }

                        await cmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error RunQuerySageLive");
                throw ex;
            }
            finally
            {

            }

        }

        public async Task<SqlDataReader> OpenQueryCTSLive(SqlCommand _cmd)
        {
            SqlDataReader dr = null;
            SqlConnection conn = new SqlConnection(_config.CTSConnectionStringLive);
            SqlCommand cmd = new SqlCommand();

            try
            {
                {
                    {
                        conn.Open();

                        cmd.CommandText = _cmd.CommandText;
                        cmd.CommandType = _cmd.CommandType;
                        cmd.CommandTimeout = 3600;

                        cmd.Connection = conn;

                        foreach (SqlParameter pr in _cmd.Parameters)
                        {
                            cmd.Parameters.Add(pr.ParameterName, pr.SqlDbType).Value = pr.Value;
                        }

                        dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error OpenQueryCTSLive");
                throw ex;

            }
            finally
            {

            }

            return dr;

        }

        public async Task RunQueryCTSLive(SqlCommand _cmd)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.CTSConnectionStringLive))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        conn.Open();

                        cmd.CommandText = _cmd.CommandText;
                        cmd.CommandType = _cmd.CommandType;
                        cmd.CommandTimeout = 3600;

                        cmd.Connection = conn;

                        foreach (SqlParameter pr in _cmd.Parameters)
                        {
                            cmd.Parameters.Add(pr.ParameterName, pr.SqlDbType).Value = pr.Value;
                        }

                        await cmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error RunQueryCTSLive");
                throw ex;
            }
            finally
            {

            }

        }


    }
}
