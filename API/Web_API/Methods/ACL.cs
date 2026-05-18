using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using tms_acl_api.Models;

namespace tms_acl_api.Methods
{
    public class ACL
    {
        public static string vConn = new Common()._SQLDMSConnectionString.ToString();
        public static string vSystemName = ConfigurationManager.AppSettings["SystemName"].ToString();

        public static async Task<DataTable> GetACLLoginCheck(string username, string pSystemName)
        {
            DataTable dt = new DataTable();

            string query = "PSP_ACL_USER_SEL";

            List<SQLParams> _p = new List<SQLParams>()
            {
                new SQLParams { Key = "pUserID", Value = username },
                new SQLParams { Key = "pSystemName", Value = pSystemName ?? vSystemName }
            };
                
            dt = await SQL.querySQL(query, CommandType.StoredProcedure, _p);

            return dt;
        }

        public static async Task<DataTable> sideBarDB(Int64 roleID, string SystemName)
        {
            DataTable dt = new DataTable();

            string query = "PSP_ACL_SIDEBAR";

            List<SQLParams> _p = new List<SQLParams>()
            {
                new SQLParams { Key = "ID_ACL_ROLE", Value = roleID },
                new SQLParams { Key = "pSystemName", Value = SystemName ?? vSystemName }
            };

            dt = await SQL.querySQL(query, CommandType.StoredProcedure, _p);

            return dt;
        }

        public static bool ValidatePassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            var isValidated = hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password) && hasSymbols.IsMatch(password);
            return isValidated;
        }
    }
}