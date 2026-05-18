using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using tms_acl_api.Models;

namespace tms_acl_api.Methods
{
    public class SQL
    {
        public static async Task<dynamic> querySQL(string pQuery, System.Data.CommandType pType, List<SQLParams> pParams, string returnParam = "")
        {
            try
            {
                DataTable dt = new DataTable();
                string pConn = new Common()._SQLDMSConnectionString.ToString();
                using (var conn = new SqlConnection(pConn))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = pQuery;
                        cmd.CommandType = pType;

                        if (pParams != null && pParams.Count > 0)
                        {
                            foreach (SQLParams _p in pParams)
                            {
                                cmd.Parameters.Add(new SqlParameter("@" + _p.Key, _p.Value)).Direction = _p.Direction;
                            }
                        }

                        using (SqlDataReader readerAsync = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(readerAsync);
                        }

                        if(returnParam != "")
                        {
                            return cmd.Parameters[string.Concat("@", returnParam)].Value.ToString();
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<int> insertSQL(string pQuery, System.Data.CommandType pType, List<SQLParams> pParams)
        {
            try
            {
                int rowAffected;
                string pConn = new Common()._SQLDMSConnectionString.ToString();

                using (var conn = new SqlConnection(pConn))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = pQuery;
                        cmd.CommandType = pType;

                        if (pParams != null && pParams.Count > 0)
                        {
                            foreach (SQLParams _p in pParams)
                            {
                                cmd.Parameters.Add(new SqlParameter("@" + _p.Key, _p.Value)).Direction = System.Data.ParameterDirection.Input;
                            }
                        }

                        rowAffected = await cmd.ExecuteNonQueryAsync();
                    }
                }

                return rowAffected;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}