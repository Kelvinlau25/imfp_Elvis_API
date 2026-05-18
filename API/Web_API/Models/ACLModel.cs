using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tms_acl_api.Models
{
    public class LoginModel
    {
        public string LoginID { get; set; }
        public string LoginPass { get; set; }
        public string SystemName { get; set; }
    }

    public class LoginModel2
    {
        public string LoginID { get; set; }
        public string LoginPass { get; set; }
    }

    public class LoginModel3
    {
        public string LoginID { get; set; }
        public string LoginPass { get; set; }
        public string SystemName { get; set; }
        public string CompanyCode { get; set; }
    }

    public class LoginModel4
    {
        public string LoginID { get; set; }
        public string LoginPass { get; set; }
        public string CompanyCode { get; set; }
    }

    public class LoginJWTTokenRefreshModel
    {
        public string UserID { get; set; }
        public string JWT_TOKEN { get; set; }
        public string JWT_REFRESH_TOKEN { get; set; }
    }

    public class RevokeJWTTokenRefreshModel
    {
        public string JWT_TOKEN { get; set; }
    }

    public class ACL_UserObj
    {
        public string JWT_TOKEN { get; set; }
        public string JWT_REFRESH_TOKEN { get; set; }
        public int ID_MM_EMPLOYEE { get; set; }
        public int ID_ACL_USER { get; set; }
        public int ID_ACL_ROLE { get; set; }
        public int ID_ACL_RESOURCE { get; set; }
        public string USER_ID { get; set; }
        public string USR_EMAIL { get; set; }
        public string COMPANY { get; set; }
        public string COMPANY_NAME { get; set; }
        public string EMP_NO { get; set; }
        public string EMP_NAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string SECTION { get; set; }
        public string ROLE_NAME { get; set; }
        public string ROLE_DESC { get; set; }
        public string RESOURCE_NAME { get; set; }
        public string RESOURCE_DESC { get; set; }
        public string TITLE_MODULE { get; set; }
    }

    public class CompanyModel
    {
        public Int64 ID_ACL_COMPANY { get; set; }
        public string COMPANY_CODE { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_TYPE { get; set; }
        public string USER_ID { get; set; }
        public string STATUS_IND { get; set; }
        public string RECORD_TYP { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_LOC { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime UPDATED_DATE { get; set; }
        public string UPDATED_LOC { get; set; }
    }

    public class ChangePasswordModel
    {
        public string USER_ID { get; set; }
        public string OLD_PASSWORD { get; set; }
        public string NEW_PASSWORD { get; set; }
        public string CONFIRM_NEW_PASSWORD { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string EMAIL_ID { get; set; }
        public string NEW_PASSWORD { get; set; }
        public string CONFIRM_NEW_PASSWORD { get; set; }
    }

    public class ACLResourceName
    {
        public int ID_ACL_RESOURCE { get; set; }
        public int RESOURCE_PARENT_ID { get; set; }
        public string RESOURCE_NAME { get; set; }
        public string RESOURCE_VIEW { get; set; }
        public string RESOURCE_CONTROLLER { get; set; }
        public int LAYER { get; set; }
        public int ACTION { get; set; }
    }

    public class ACL_USER
    {
        public int ID_ACL_USER { get; set; }
        public string USER_ID { get; set; }
        public string USR_PASSWORD { get; set; }
        public string USR_EMAIL { get; set; }
        public string RECORD_TYP { get; set; }
        public string STATUS_IND { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string CREATED_LOC { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_DATE { get; set; }
        public string UPDATED_LOC { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_NO { get; set; }
        public string COMPANY { get; set; }
    }

    public class RESOURCE_ACCESS_CHK_MODEL
    {
        public int P_ROLEID { get; set; }
        public string P_SYSTEM_NAME { get; set; }
        public string P_CONTROLLER { get; set; }
        public string P_ACTION { get; set; }
    }

    public class ACL_USAGE_TRACKING
    {
        public string USERID { get; set; }
        public string PAGE_NAME { get; set; }
        public string MODULE_NAME { get; set; }
    }
}