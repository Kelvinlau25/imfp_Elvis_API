namespace web_app_template.Models
{
    public class LogoutRequest
    {
        public string JWTToken { get; set; }
        public string JWTRefreshToken { get; set; }
    }
}