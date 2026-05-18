namespace web_app_template.Models
{
    public class VerifyOTPRequest
    {
        public string EmailAddress { get; set; }
        public string OTP { get; set; }
        public string CompanyCode { get; set; }
        public string DatabaseType { get; set; }
    }
}