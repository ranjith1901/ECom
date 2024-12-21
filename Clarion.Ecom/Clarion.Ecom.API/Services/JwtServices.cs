using Clarion.Ecom.API.NonEntity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clarion.Ecom.API.Services
{
    /// <summary>
    /// Token Validation and Generation
    /// </summary>
    public class JwtServices
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _signingKey = string.Empty;
        private readonly string _encyptionKey = string.Empty;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpContextAccessor"></param>
        public JwtServices(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _signingKey = _config["JWT:SigningKey"] ?? "SignSectK3yis$Clarion.Ecom@2k24kEy";
            _encyptionKey = _config["JWT:EncryptionKey"] ?? "EncSecrtK3yis$Clarion3c0m@2k24kEy";
        }
        /// <summary>
        /// Get claims from the Auth Token
        /// </summary>
        /// <returns></returns>
        public ClaimData GetClaimData()
        {
            ClaimData claimData = new ClaimData();
            try
            {
                var symmetricSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_signingKey));
                var symmetricEncKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_encyptionKey));
                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = _config["JWT:Audience"],
                    ValidIssuer = _config["JWT:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricSigningKey,
                    TokenDecryptionKey = symmetricEncKey
                };

                // Get the HttpContext from IHttpContextAccessor
                var httpContext = _httpContextAccessor.HttpContext;

                // Extract the token from the Authorization header
                var authorizationHeader = httpContext?.Request.Headers["Authorization"].FirstOrDefault();
                var tokenString = authorizationHeader?.Replace("Bearer", "").Trim();

                // Validate the token and retrieve the claims
                ClaimsPrincipal objClaims = handler.ValidateToken(tokenString, validationParameters, out securityToken);

                claimData = new ClaimData
                {
                    UserID = Convert.ToInt64(objClaims.Claims.First(claim => claim.Type == "UserID").Value),
                    UserRoleID = Convert.ToInt64(objClaims.Claims.First(claim => claim.Type == "RoleID").Value),
                    Username = objClaims.Claims.First(claim => claim.Type == "UserName")?.Value ?? "NA",
                    UserEmail = objClaims.Claims.First(claim => claim.Type == "Email")?.Value ?? "NA",
                    CompanyID = Convert.ToInt64(objClaims.Claims.First(claim => claim.Type == "CompanyID").Value),
                };

                // Set the ClaimData in HttpContext
                httpContext!.Items["ClaimData"] = claimData;
            }
            catch (SecurityTokenExpiredException)
            {
                //throw new Exception("Security Token expired");
            }
            return claimData;
        }

        /// <summary>
        /// Generates the JWT for the logged in user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateToken(LogInResponse user)
        {
            var symmetricSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_signingKey));
            var symmetricEncKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_encyptionKey));

            var objClaims = new List<Claim>
        {
            new Claim("UserName", user.UserName),
            new Claim("CompanyID", user.CompanyID.ToString()),
            new Claim("UserID",user.UserID.ToString()),
            new Claim("RoleID", user.RoleID.ToString()),
            new Claim("Email", user.Email),
            // new Claim("UserPrivileges",JsonSerializer.Serialize(user.UserPrivileges)),

            new Claim(JwtRegisteredClaimNames.Iss, _config!["JWT:Issuer"] ?? "ClarionECOMAPI"),
            new Claim(JwtRegisteredClaimNames.Aud, _config !["JWT:Audience"] ?? "ClarionECOMUsers"),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(objClaims),
                Expires = DateTime.UtcNow.AddHours(24),
                IssuedAt = DateTime.UtcNow,
                Audience = _config["JWT:Audience"],
                Issuer = _config["JWT:Issuer"],
                SigningCredentials = new SigningCredentials(symmetricSigningKey, SecurityAlgorithms.HmacSha256Signature),
                EncryptingCredentials = new EncryptingCredentials(symmetricEncKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes128CbcHmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

    }
}
