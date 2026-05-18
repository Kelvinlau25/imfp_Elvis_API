using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using tms_acl_api.DAL;
using tms_acl_api.Enum;
using tms_acl_api.Interface;

namespace tms_acl_api.Helpers
{
    public class CommonFunction : Controller,  IDAPPER_DAL
    {
        public CommonFunction(string uSERID = "",string vConnection_String = "")
        {
            USERID = uSERID;
            Connection_String = vConnection_String;
        }

        public string USERID { get; set; }
        public string Connection_String { get; set; }
        #region PSP_COMMON_SQL
        /// <summary>
        /// PSP_COMMON_SQL
        /// Azham 17/1/2023
        /// ORM get data from MSSQL database and pass back the result as database
        /// </summary>

        public async Task<DataTable> PSP_COMMON_SQL(string Query)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, c))
                    {
                        await c.OpenAsync();
                        cmd.CommandTimeout = 0;
                        SqlDataReader reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_SQL(string Query, EnumType.ExecutionType ExecutionType)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, c))
                    {
                        await c.OpenAsync();
                        cmd.CommandTimeout = 0;
                        if (ExecutionType == EnumType.ExecutionType.ExecuteReader)
                        {
                            SqlDataReader reader = await cmd.ExecuteReaderAsync();
                            dt.Load(reader);
                        }
                        else
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }

                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_SQL(string Query, string ConnectionString = "")
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = ConnectionString;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, c))
                    {
                        await c.OpenAsync();
                        cmd.CommandTimeout = 0;
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        dt.Load(reader);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_SQL(string Query, CommandType type = CommandType.Text)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, c))
                    {
                        await c.OpenAsync();
                        cmd.CommandType = type;
                        cmd.CommandTimeout = 0;
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        dt.Load(reader);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_SQL(string Query, CommandType type = CommandType.Text, List<SqlParameter> ListofParam = null)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, c))
                    {
                        await c.OpenAsync();
                        cmd.CommandType = type;
                        cmd.CommandTimeout = 0;

                        if (ListofParam != null && ListofParam.Count > 0)
                        {
                            foreach (SqlParameter param in ListofParam)
                            {
                                cmd.Parameters.Add(param);
                            }
                        }

                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        dt.Load(reader);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_SQL(string Query, EnumType.ExecutionType ExecutionType, CommandType type = CommandType.Text, List<SqlParameter> ListofParam = null)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, c))
                    {
                        await c.OpenAsync();
                        cmd.CommandType = type;
                        cmd.CommandTimeout = 0;

                        if (ListofParam != null && ListofParam.Count > 0)
                        {
                            foreach (SqlParameter param in ListofParam)
                            {
                                cmd.Parameters.Add(param);
                            }
                        }
                        if (ExecutionType == EnumType.ExecutionType.ExecuteReader)
                        {
                            SqlDataReader reader = await cmd.ExecuteReaderAsync();
                            dt.Load(reader);
                        }
                        else
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_SQL(string Query, CommandType type = CommandType.Text, List<SqlParameter> ListofParam = null, string ConnectionString = "")
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = ConnectionString;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, c))
                    {
                        await c.OpenAsync();
                        cmd.CommandType = type;
                        cmd.CommandTimeout = 0;

                        if (ListofParam != null && ListofParam.Count > 0)
                        {
                            foreach (SqlParameter param in ListofParam)
                            {
                                cmd.Parameters.Add(param);
                            }
                        }

                        SqlDataReader reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_SQL(string Query, EnumType.ExecutionType ExecutionType, CommandType type = CommandType.Text, List<SqlParameter> ListofParam = null, string ConnectionString = "")
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = ConnectionString;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, c))
                    {
                        await c.OpenAsync();
                        cmd.CommandType = type;
                        cmd.CommandTimeout = 0;

                        if (ListofParam != null && ListofParam.Count > 0)
                        {
                            foreach (SqlParameter param in ListofParam)
                            {
                                cmd.Parameters.Add(param);
                            }
                        }

                        if (ExecutionType == EnumType.ExecutionType.ExecuteReader)
                        {
                            SqlDataReader reader = await cmd.ExecuteReaderAsync();
                            dt.Load(reader);
                        }
                        else
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        #endregion
        #region PSP_COMMON_DAPPER_SQL
        /// <summary>
        /// Azham 17/1/2023
        /// PSP_COMMON_DAPPER
        /// ORM  get data from database and convert datatable to Model Object 
        /// Require external plugin Dapper   - Download from Microsoft Nuget
        /// Dapper recommended version : v2.0.123     
        /// </summary>
        public async Task<List<T>> PSP_COMMON_DAPPER<T>(string Query)
        {
            List<T> EmpList = null;
            try
            {
                string constr = Connection_String;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    EmpList = (await c.QueryAsync<T>(Query, null, commandType: CommandType.Text).ConfigureAwait(false)).AsList();
                }
                return EmpList;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return EmpList;
                throw;
            }
        }
        public async Task<List<T>> PSP_COMMON_DAPPER<T>(string Query, string ConnectionString)
        {
            List<T> EmpList = null;
            try
            {
                string constr = ConnectionString;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    EmpList = (await c.QueryAsync<T>(Query, null, commandType: CommandType.Text).ConfigureAwait(false)).AsList();
                }
                return EmpList;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return EmpList;
                throw;
            }
        }
        public async Task<List<T>> PSP_COMMON_DAPPER<T>(string Query, CommandType Commandtype)
        {
            List<T> EmpList = null;
            try
            {
                string constr = Connection_String;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    EmpList = (await c.QueryAsync<T>(Query, null, commandType: Commandtype).ConfigureAwait(false)).AsList();
                }
                return EmpList;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return EmpList;
                throw;
            }
        }
        public async Task<List<T>> PSP_COMMON_DAPPER<T>(string Query, CommandType Commandtype, object ListofParam)
        {
            List<T> ObjModel = null;
            try
            {
                string constr = Connection_String;
                using (SqlConnection c = new SqlConnection(constr))
                {
                    ObjModel = (await c.QueryAsync<T>(Query, ListofParam, commandType: Commandtype).ConfigureAwait(false)).AsList();
                }
                return ObjModel;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return ObjModel;
                throw;
            }
        }
        #endregion

        #region PSP_COMMON_ORACLE
        public async Task<DataTable> PSP_COMMON_ORA(string Query)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (OracleConnection cORA = new OracleConnection(constr))
                {
                    using (OracleCommand cmd = new OracleCommand(Query, cORA))
                    {
                        await cORA.OpenAsync();
                        cmd.CommandTimeout = 0;
                        OracleDataReader readerORA = cmd.ExecuteReader();
                        dt.Load(readerORA);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_ORA(string Query, EnumType.ExecutionType ExecutionType)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (OracleConnection cORA = new OracleConnection(constr))
                {
                    using (OracleCommand cmd = new OracleCommand(Query, cORA))
                    {
                        await cORA.OpenAsync();
                        cmd.CommandTimeout = 0;
                        if (ExecutionType == EnumType.ExecutionType.ExecuteReader)
                        {
                            OracleDataReader readerORA = cmd.ExecuteReader();
                            dt.Load(readerORA);
                        }
                        else
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_ORA(string Query, string ConnectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = ConnectionString;
                using (OracleConnection cORA = new OracleConnection(constr))
                {
                    using (OracleCommand cmd = new OracleCommand(Query, cORA))
                    {
                        await cORA.OpenAsync();
                        cmd.CommandTimeout = 0;
                        OracleDataReader readerORA = cmd.ExecuteReader();
                        dt.Load(readerORA);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_ORA(string Query, CommandType type = CommandType.Text)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (OracleConnection cORA = new OracleConnection(constr))
                {
                    using (OracleCommand cmd = new OracleCommand(Query, cORA))
                    {
                        await cORA.OpenAsync();
                        cmd.CommandType = type;
                        cmd.CommandTimeout = 0;
                        OracleDataReader readerORA = cmd.ExecuteReader();
                        dt.Load(readerORA);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_ORA(string Query, CommandType type = CommandType.Text, List<OracleParameter> ListofParam = null)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = Connection_String;
                using (OracleConnection cORA = new OracleConnection(constr))
                {
                    using (OracleCommand cmd = new OracleCommand(Query, cORA))
                    {
                        await cORA.OpenAsync();
                        cmd.CommandType = type;
                        cmd.CommandTimeout = 0;

                        if (ListofParam != null && ListofParam.Count > 0)
                        {
                            foreach (OracleParameter item in ListofParam)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        OracleDataReader readerORA = cmd.ExecuteReader();
                        dt.Load(readerORA);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        public async Task<DataTable> PSP_COMMON_ORA(string Query, CommandType type = CommandType.Text, List<OracleParameter> ListofParam = null, string ConnectionString = "")
        {
            DataTable dt = new DataTable();
            try
            {
                //string constr = await databaseConnection.GetConnectionStringAsync(false);
                string constr = ConnectionString;
                using (OracleConnection cORA = new OracleConnection(constr))
                {
                    using (OracleCommand cmd = new OracleCommand(Query, cORA))
                    {
                        await cORA.OpenAsync();
                        cmd.CommandType = type;
                        cmd.CommandTimeout = 0;

                        if (ListofParam != null && ListofParam.Count > 0)
                        {
                            foreach (OracleParameter item in ListofParam)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        OracleDataReader readerORA = cmd.ExecuteReader();
                        dt.Load(readerORA);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                err.ErrorLog_Add_V2(System.Reflection.MethodBase.GetCurrentMethod().Name, ex, USERID);
                err = null;
                return dt;
                throw;
            }
        }
        #endregion
    }
}