using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace tms_acl_api.Methods
{
    public class JWTTokenMethod
    {
        private static string securityCode = ConfigurationManager.AppSettings["JWTSecurityKey"].ToString();
        private static string jwtTokenExpiryInMinutes = ConfigurationManager.AppSettings["JWTokenExpiryInMinutes"].ToString();
        private static string jwtTokenIssuer = ConfigurationManager.AppSettings["JWTIssuer"].ToString();
        private static string jwtTokenAudience = ConfigurationManager.AppSettings["JWTAudience"].ToString();

        public static string createToken(string username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            _ = int.TryParse(jwtTokenExpiryInMinutes, out int tokenValidityInMinutes);
            DateTime expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            //ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.Name, username)
            //});
            List<Claim> _claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username)
            };
            
            //"401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            //var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(securityCode));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token = GenerateToken(_claims);
                //(JwtSecurityToken)
                //    tokenHandler.CreateJwtSecurityToken(issuer: jwtTokenIssuer, audience: jwtTokenAudience,
                //        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public static string createRefreshToken()
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public static JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityCode));
            _ = int.TryParse(jwtTokenExpiryInMinutes, out int tokenValidityInMinutes);
            DateTime issuedAt = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                issuer: jwtTokenIssuer,
                audience: jwtTokenAudience,
                notBefore: issuedAt,
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return token;
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityCode)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);


            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            { // || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            }
            
            return principal;
        }
    }
}