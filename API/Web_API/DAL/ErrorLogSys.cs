using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace tms_acl_api.DAL
{
    public class ErrorLogSys 
    {
        public async Task<Boolean> ErrorLog_Add(string FunctionName, string ErrorMsg, string CreatedBy)
        {
            try
            {
                string constr = "";

                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("PSP_ERROR_LOGS", con))
                    {
                        SqlDataReader reader = null;

                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@pFunctionName", FunctionName)).Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new SqlParameter("@pErrorMSG", ErrorMsg)).Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new SqlParameter("@pCreateBy", CreatedBy)).Direction = System.Data.ParameterDirection.Input;
                        reader = cmd.ExecuteReader();
                        reader.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
                return false;
            }
        }
        public async Task<Boolean> ErrorLog_Add_V2(string FunctionName, Exception exData, string CreatedBy, string OptnalRef = "")
        {
            try
            {
                string constr = "";

                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("PSP_ERROR_LOGS", con))
                    {
                        SqlDataReader reader = null;

                            await con.OpenAsync();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@pFunctionName", FunctionName)).Direction = System.Data.ParameterDirection.Input;
                            cmd.Parameters.Add(new SqlParameter("@pErrorMSG", OptnalRef + exData.Message.ToString())).Direction = System.Data.ParameterDirection.Input;
                            cmd.Parameters.Add(new SqlParameter("@pSource", exData.Source.ToString())).Direction = System.Data.ParameterDirection.Input;
                            cmd.Parameters.Add(new SqlParameter("@pStackTrace", exData.StackTrace.ToString())).Direction = System.Data.ParameterDirection.Input;
                            cmd.Parameters.Add(new SqlParameter("@pTargetSite", exData.TargetSite.ToString())).Direction = System.Data.ParameterDirection.Input;
                            cmd.Parameters.Add(new SqlParameter("@pCreateBy", CreatedBy)).Direction = System.Data.ParameterDirection.Input;
                            reader = cmd.ExecuteReader();
                            reader.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
                return false;
            }
        }
    }
}