using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace tms_acl_api.Interface
{
    interface IDAPPER_DAL
    {
        Task<List<T>> PSP_COMMON_DAPPER<T>(string Query);
        Task<List<T>> PSP_COMMON_DAPPER<T>(string Query, string ConnectionString);
        Task<List<T>> PSP_COMMON_DAPPER<T>(string Query, System.Data.CommandType Commandtype);
        Task<List<T>> PSP_COMMON_DAPPER<T>(string Query, System.Data.CommandType Commandtype, object ListofParam);
    }
}