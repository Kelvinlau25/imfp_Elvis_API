using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace tms_acl_api.Models
{
    public class Common
    {
        public string _SQLDMSConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["PFR_ACL_MVC"].ConnectionString; }
        }

        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            if (pro.PropertyType.Name.Equals("Boolean"))
                            {
                                if (row[pro.Name].ToString().ToUpper().Equals("TRUE")) { pro.SetValue(objT, true); }
                                else { pro.SetValue(objT, false); }
                            }
                            else if (pro.PropertyType.Name.Equals("Integer"))
                            {
                                pro.SetValue(objT, Convert.ToInt32(row[pro.Name]));
                            }
                            else if (pro.PropertyType.Name.Equals("DateTime"))
                            {
                                pro.SetValue(objT, Convert.ToDateTime(row[pro.Name]));
                            }
                            else { pro.SetValue(objT, row[pro.Name]); }
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }

        public static string Hashpassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }

            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }

            if (password == null)
            {
                throw new ArgumentNullException(password);
            }

            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }

            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            bool compare = buffer3.SequenceEqual(buffer4);
            return buffer3.SequenceEqual(buffer4);
        }
    }

    public class SQLParams
    {
        public string Key { get; set; }
        public dynamic Value { get; set; }
        [System.ComponentModel.DefaultValue(System.Data.ParameterDirection.Input)]
        public System.Data.ParameterDirection Direction { get; set; }
    }
}