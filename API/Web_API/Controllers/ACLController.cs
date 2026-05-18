using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tms_acl_api.Helpers;
using tms_acl_api.Methods;
using tms_acl_api.Models;
using web_app_template.Models;
using tms_acl_api.Infrastructure;

namespace tms_acl_api.Controllers
{
    [Authorize]
    [Route("api/ACL")]
    [ApiController]
    public class ACLController : ControllerBase
    {
        string vConn = AppConfiguration.GetConnectionString("PFR_ACL_MVC");

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> Post([FromBody]LoginModel dto)
        //{
        //    DateTime _startDateTime = DateTime.Now;
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        dt = await ACL.GetACLLoginCheck(dto.LoginID, dto.SystemName);
        //        ACL_UserObj obj = new ACL_UserObj();

        //        if(dt.Rows.Count <= 0)
        //        {
        //            return NotFound();
        //        }

        //        DataRow drUser = dt.Rows[0];

        //        if (dt.Rows.Count > 0)
        //        {
        //            string Gethashpassword = Common.Hashpassword(dto.LoginPass);
        //            Boolean passcheck = Common.VerifyHashedPassword(drUser["USR_PASSWORD"].ToString(), dto.LoginPass);

        //            if (dto.LoginID.ToUpper().ToString().Equals(drUser["USER_ID"].ToString().ToUpper()) && passcheck.Equals(true))
        //            {
        //                string newToken = JWTTokenMethod.createToken(dto.LoginID);
        //                string newRefreshToken = JWTTokenMethod.createRefreshToken();

        //                _ = int.TryParse(AppConfiguration.GetAppSetting("JWRefreshTokenExpiryInMinutes").ToString(), out int _refreshTokenExpiryInMinutes);
        //                int isAdded = await SQL.insertSQL("insert into ACL_JWT_TOKEN (JWT_TOKEN, JWT_REFRESH_TOKEN, JWT_REFRESH_TOKEN_EXPIRY) VALUES ('" + newToken + "','" + newRefreshToken + "','" + DateTime.Now.AddMinutes(_refreshTokenExpiryInMinutes).ToString("MM/dd/yyyy HH:mm:ss tt") + "')", CommandType.Text, null);

        //                if(isAdded <= 0)
        //                {
        //                    return BadRequest("Failed to generate JWT token");
        //                }
        //                obj.JWT_TOKEN = newToken;
        //                obj.JWT_REFRESH_TOKEN = newRefreshToken;
        //                obj.ID_ACL_USER = System.Convert.ToInt32(drUser["ID_ACL_USER"]);
        //                obj.ID_ACL_ROLE = System.Convert.ToInt32(drUser["ID_ACL_ROLE"]);
        //                obj.ID_ACL_RESOURCE = System.Convert.ToInt32(drUser["ID_ACL_RESOURCE"]);
        //                obj.USER_ID = drUser["USER_ID"].ToString();
        //                obj.USR_EMAIL = drUser["USR_EMAIL"].ToString();
        //                obj.COMPANY_NAME = drUser["COMPANY"].ToString();
        //                obj.SECTION = drUser["COMPANY"].ToString();
        //                obj.EMP_NO = drUser["EMP_NO"].ToString();
        //                obj.EMP_NAME = drUser["EMP_NAME"].ToString();
        //                obj.ROLE_NAME = drUser["ROLE_NAME"].ToString();
        //                obj.ROLE_DESC = drUser["ROLE_DESC"].ToString();
        //            } else
        //            {
        //                return NotFound();
        //            }
        //        }

        //        return Ok(obj);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.ToString());
        //    }
        //}


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Post([FromBody]LoginModel dto)
        {
            try
            {
                DataTable dt = new DataTable();
                string systemName = AppConfiguration.GetAppSetting("SystemName_ACL");
                string companyCode = AppConfiguration.GetAppSetting("CompanyCode_ACL");
                string DatabaseType = AppConfiguration.GetAppSetting("DBTYPE");

                LoginRequest loginmodel = new LoginRequest();
                loginmodel.LoginID = dto.LoginID;
                loginmodel.LoginPass = dto.LoginPass;
                loginmodel.SystemName = systemName;
                loginmodel.CompanyCode = companyCode;
                loginmodel.DatabaseType = DatabaseType;

                ByteArrayContent clientbodystr = new StringContent(JsonConvert.SerializeObject(loginmodel), Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(AppConfiguration.GetAppSetting("ACL_API") + "/api/v1/login/login", clientbodystr);

                ACL_UserObj obj = new ACL_UserObj();
                if (response.IsSuccessStatusCode == true)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LoginResponse>(responseBody);

                    obj = ACLHelper.JWTTokenToACLObj(result.JWTToken);
                    obj.JWT_TOKEN = result.JWTToken;
                    obj.JWT_REFRESH_TOKEN = result.JWTRefreshToken;

                    return Ok(obj);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]LoginJWTTokenRefreshModel dto)
        {
            DateTime _startDateTime = DateTime.Now;
            try
            {
                DataTable dt = await SQL.querySQL("SELECT * FROM ACL_JWT_TOKEN WHERE JWT_TOKEN = '" + dto.JWT_TOKEN + "'", CommandType.Text, null);

                if(dt.Rows.Count <= 0)
                {
                    return NotFound();
                }

                _ = DateTime.TryParse(dt.Rows[0]["JWT_REFRESH_TOKEN_EXPIRY"].ToString(), out DateTime _refreshTokenExpiry);
                if (_refreshTokenExpiry < DateTime.Now)
                {
                    return BadRequest("Refresh Token expired");
                }

                ClaimsPrincipal _oldTokenPrinciple = JWTTokenMethod.GetPrincipalFromExpiredToken(dt.Rows[0]["JWT_TOKEN"].ToString());

                string newToken = JWTTokenMethod.createToken(dto.UserID);
                string newRefreshToken = JWTTokenMethod.createRefreshToken();

                _ = int.TryParse(AppConfiguration.GetAppSetting("JWRefreshTokenExpiryInMinutes").ToString(), out int _refreshTokenExpiryInMinutes);
                int isAdded = await SQL.insertSQL("insert into ACL_JWT_TOKEN (JWT_TOKEN, JWT_REFRESH_TOKEN, JWT_REFRESH_TOKEN_EXPIRY) VALUES ('" + newToken + "','" + newRefreshToken + "','" + DateTime.Now.AddMinutes(_refreshTokenExpiryInMinutes).ToString("MM/dd/yyyy HH:mm:ss tt") + "')", CommandType.Text, null);
                int isDeleted = await SQL.insertSQL("delete from ACL_JWT_TOKEN where JWT_TOKEN = '" + dto.JWT_TOKEN + "'", CommandType.Text, null);

                if(isAdded <= 0 || isDeleted <= 0)
                {
                    return BadRequest("Failed to refresh JWT token");
                }

                LoginJWTTokenRefreshModel _newToken = new LoginJWTTokenRefreshModel()
                {
                    JWT_TOKEN = newToken,
                    JWT_REFRESH_TOKEN = newRefreshToken,
                    UserID = dto.UserID
                };

                return Ok(_newToken);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody]RevokeJWTTokenRefreshModel dto)
        {
            DateTime _startDateTime = DateTime.Now;
            try
            {
                int isDeleted = await SQL.insertSQL("delete from ACL_JWT_TOKEN where JWT_TOKEN = '" + dto.JWT_TOKEN + "'", CommandType.Text, null);

                if (isDeleted <= 0)
                {
                    return BadRequest("Failed to revoke JWT token");
                }

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetCompany/{pCompCode}")]
        public async Task<IActionResult> GetCompanyByCompCode(string pCompCode)
        {
            try
            {
                string query = "select * from PVIEW_ACL_COMPANY where company_code = '" + pCompCode + "'";
                DataTable dt = await SQL.querySQL(query, CommandType.Text, null);

                List<CompanyModel> CompanyList = new List<CompanyModel>();
                CompanyList = Common.ConvertToList<CompanyModel>(dt);

                return Ok(CompanyList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel dto)
        {
            try
            {
                List<SQLParams> _params = new List<SQLParams>()
                {
                    new SQLParams() { Key = "userID", Value = dto.USER_ID }
                };

                DataTable _dt1 = await SQL.querySQL("PSP_ACL_CHANGE_PASSWORD", CommandType.StoredProcedure, _params);
                if (_dt1.Rows.Count <= 0)
                {
                    return NotFound();
                }

                if (Common.VerifyHashedPassword(_dt1.Rows[0]["USR_PASSWORD"].ToString(), dto.OLD_PASSWORD))
                {
                    List<SQLParams> _params2 = new List<SQLParams>()
                    {
                        new SQLParams() { Key = "userID", Value = dto.USER_ID },
                        new SQLParams() { Key = "newPassword", Value = Common.Hashpassword(dto.NEW_PASSWORD) },
                        new SQLParams() { Key = "returnID", Value = dto.USER_ID, Direction = ParameterDirection.Output }
                    };

                    if(!ACL.ValidatePassword(dto.NEW_PASSWORD) || dto.NEW_PASSWORD != dto.CONFIRM_NEW_PASSWORD || dto.OLD_PASSWORD == dto.NEW_PASSWORD)
                    {
                        return BadRequest("Invalid password format");
                    }

                    string _rtn = await SQL.querySQL("PSP_ACL_CHANGE_PASSWORD_MAINT", CommandType.StoredProcedure, _params2, "returnID");
                    if(_rtn != "Y")
                    {
                        return BadRequest("Failed to update password");
                    }
                }
                else
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }            
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel dto)
        {
            try
            {
                DataTable _dt1 = await SQL.querySQL("SELECT * FROM ACL_User WHERE USR_EMAIL = '" + dto.EMAIL_ID + "' AND STATUS_IND = 'Active'", CommandType.Text, null);
                if (_dt1.Rows.Count <= 0)
                {
                    return NotFound();
                }
    
                if (!ACL.ValidatePassword(dto.NEW_PASSWORD) || dto.NEW_PASSWORD != dto.CONFIRM_NEW_PASSWORD)
                {
                    return BadRequest("Invalid password format");
                }

                int _rtn = await SQL.insertSQL("UPDATE ACL_User SET USR_PASSWORD = '" + Common.Hashpassword(dto.NEW_PASSWORD) + "' WHERE USR_EMAIL = '" + dto.EMAIL_ID + "' AND STATUS_IND = 'Active'", CommandType.Text, null);
                if (_rtn <= 0)
                {
                    return BadRequest("Failed to update password");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("RetrieveResourceByName/{pACLRoleID}/{pSystemName}")]
        public async Task<IActionResult> RetrieveResourceByName(string pACLRoleID, string pSystemName)
        {
            try
            {
                List<SQLParams> _params = new List<SQLParams>()
                {
                    new SQLParams() { Key = "ID_ACL_ROLE", Value = pACLRoleID },
                    new SQLParams() { Key = "pSystemName", Value = pSystemName }
                };

                string query = "PSP_ACL_SIDEBAR_PERMISSION";
                DataTable dt = await SQL.querySQL(query, CommandType.StoredProcedure, _params, "");

                List<ACLResourceName> CompanyList = new List<ACLResourceName>();
                CompanyList = Common.ConvertToList<ACLResourceName>(dt);

                return Ok(CompanyList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        //Anis try add retrieve sidebar
        [AllowAnonymous]
        [HttpGet]
        [Route("RetrieveResource/RoleName={pUserRole}/SystemName={pSystemName}/UserID={pUserID}/ParentID={pParentID}")]
        public async Task<IActionResult> RetrieveResource(string pUserRole, string pSystemName, string pUserID, int pParentID)
        {
            try
            {
                List<SQLParams> _params = new List<SQLParams>()
                {
                    new SQLParams() { Key = "pUserRole", Value = pUserRole },
                    new SQLParams() { Key = "pSystemName", Value = pSystemName },
                    new SQLParams() { Key = "pUserID", Value = pUserID },
                    new SQLParams() { Key = "pParentID", Value = 0 }
                };

                string query = "PSP_ACL_MENU_LST3";
                DataTable dt = await SQL.querySQL(query, CommandType.StoredProcedure, _params, "");

                List<ACLResourceName> sidebarList = new List<ACLResourceName>();
                sidebarList = Common.ConvertToList<ACLResourceName>(dt);

                return Ok(sidebarList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        //RetrieveApplicationIDByName
        [HttpGet]
        [Route("RetrieveApplicationIDByName/{pName}")]
        public async Task<IActionResult> RetrieveApplicationIDByName(string pName)
        {
            try
            {
                string query = "SELECT * FROM ACL_RESOURCE WHERE RESOURCE_NAME = '" + pName + "'";
                DataTable dt = await SQL.querySQL(query, CommandType.Text, null, "");

                List<ACLResourceName> CompanyList = new List<ACLResourceName>();
                CompanyList = Common.ConvertToList<ACLResourceName>(dt);

                return Ok(CompanyList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        //UserInfo
        [HttpGet]
        [Route("UserInfo/{pCompCode}/{pUserID}")]
        public async Task<IActionResult> UserInfo(string pCompCode, string pUserID)
        {
            try
            {
                string query = "SELECT * FROM ACL_User WHERE COMPANY = '" + pCompCode + "' AND USER_ID = '" + pUserID + "'";
                DataTable dt = await SQL.querySQL(query, CommandType.Text, null, "");

                List<ACL_USER> CompanyList = new List<ACL_USER>();
                CompanyList = Common.ConvertToList<ACL_USER>(dt);

                return Ok(CompanyList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        [Route("Validate")]
        public async Task<IActionResult> Validate(LoginModel2 dto)
        {
            try
            {
                DataTable _dt1 = await SQL.querySQL("SELECT * FROM ACL_User WHERE USER_ID = '" + dto.LoginID + "'", CommandType.Text, null);
                if (_dt1.Rows.Count <= 0)
                {
                    return Ok(false);
                }

                Boolean passcheck = Common.VerifyHashedPassword(_dt1.Rows[0]["USR_PASSWORD"].ToString(), dto.LoginPass);

                return Ok(passcheck);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("ValidateWithCompany")]
        public async Task<IActionResult> ValidateWithCompany(LoginModel4 dto)
        {
            try
            {
                DataTable _dt1 = await SQL.querySQL("SELECT * FROM ACL_User WHERE USER_ID = '" + dto.LoginID + "' AND COMPANY = '" + dto.CompanyCode + "'", CommandType.Text, null);
                if (_dt1.Rows.Count <= 0)
                {
                    return Ok(false);
                }

                Boolean passcheck = Common.VerifyHashedPassword(_dt1.Rows[0]["USR_PASSWORD"].ToString(), dto.LoginPass);

                return Ok(passcheck);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("ValidateWithRetrieveUsernCompany")]
        public async Task<IActionResult> ValidateWithRetrieveUsernCompany(LoginModel3 dto)
        {
            try
            {
                DataTable _dt1 = await SQL.querySQL("SELECT " +
                                                        "A.ID_ACL_USER, A.USER_ID, A.USR_EMAIL, A.COMPANY, A.EMP_NO, A.EMP_NAME, " +
                                                        "C.ID_ACL_ROLE, C.ROLE_NAME, C.ROLE_DESC, " +
                                                        "D.ID_ACL_RESOURCE, D.RESOURCE_NAME, D.RESOURCE_DESC, USR_PASSWORD " +
                                                    "FROM ACL_User A " +
                                                    "LEFT JOIN ACL_USR_ROLE B ON B.ID_ACL_USER = A.ID_ACL_USER " +
                                                    "LEFT JOIN ACL_ROLE C ON C.ID_ACL_ROLE = B.ID_ACL_ROLE AND C.ROLE_STATUS = 'Active' " +
                                                    "LEFT JOIN ACL_RESOURCE D ON D.ID_ACL_RESOURCE = C.ID_ACL_RESOURCE AND D.RESOURCE_STATUS = 'Active' " +
                                                    "WHERE A.USER_ID = '" + dto.LoginID + "' AND D.RESOURCE_NAME = '" + dto.SystemName +  "' AND A.COMPANY = '" + dto.CompanyCode + "' " +
                                                    "AND A.STATUS_IND = 'Active'", CommandType.Text, null);
                if (_dt1.Rows.Count <= 0)
                {
                    return NotFound();
                }

                Boolean passcheck = Common.VerifyHashedPassword(_dt1.Rows[0]["USR_PASSWORD"].ToString(), dto.LoginPass);

                if(!passcheck) { return NotFound(); }

                List<ACL_USER> _list = new List<ACL_USER>();
                _list = Common.ConvertToList<ACL_USER>(_dt1);

                return Ok(_list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("GetResourceAccessControl")]
        public async Task<IActionResult> GetResourceAccessControl(RESOURCE_ACCESS_CHK_MODEL dto)
        {
            try
            {
                List<SQLParams> _params = new List<SQLParams>()
                {
                    new SQLParams() { Key = "P_ROLEID", Value = dto.P_ROLEID },
                    new SQLParams() { Key = "P_SYSTEM_NAME", Value = dto.P_SYSTEM_NAME },
                    new SQLParams() { Key = "P_CONTROLLER", Value = dto.P_CONTROLLER },
                    new SQLParams() { Key = "P_ACTION", Value = dto.P_ACTION },
                };

                DataTable dt1 = await SQL.querySQL("PSP_ACL_GET_ACTION_ACCESS", CommandType.StoredProcedure, _params, "");

                return Ok(dt1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }           
        }

        [HttpPost]
        [Route("ACLUsageTracking")]
        public async Task<IActionResult> ACLUsageTracking(ACL_USAGE_TRACKING dto)
        {
            try
            {
                DataTable dt = await SQL.querySQL("SELECT * FROM ACL_PAGE_INFO WHERE PAGE_NAME = '" + dto.PAGE_NAME + "' AND MODULE_NAME = '" + dto.MODULE_NAME + "'", CommandType.Text, null);
                if (dt.Rows.Count <= 0)
                {
                    return NotFound();
                }

                int _cnt = await SQL.insertSQL("INSERT INTO ACL_USAGE_TRACKING(USERID, PAGEID, ACCESS_TIME) VALUES ('" + dto.USERID + "', " + dt.Rows[0]["ID_ACL_PAGE"] + ", GETDATE())", CommandType.Text, null);
                if (_cnt <= 0) { return BadRequest(); }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
