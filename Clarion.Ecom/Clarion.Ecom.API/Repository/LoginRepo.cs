using Microsoft.EntityFrameworkCore;
using Clarion.Ecom.API.IRepository;
using Clarion.Ecom.API.Models;
using Clarion.Ecom.API.Services;
using Clarion.Ecom.API.Extensions;
using Clarion.Ecom.API.NonEntity;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics.Metrics;
using System.ComponentModel.Design;

namespace Clarion.Ecom.API.Repository
{
    /// <summary>
    /// Login Operations
    /// </summary>
    public class LoginRepo : ILoginRepo
    {
        /// <summary>
        /// DB Context
        /// </summary>
        private ClarionECOMDBContext _context;
        /// <summary>
        /// Jwt Service class
        /// </summary>
        private readonly JwtServices _jwtServices;
        /// <summary>
        /// Current Http context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Configuration
        /// </summary>
        private IConfiguration _configuration;
        /// <summary>
        /// Email
        /// </summary>
        private IEmail _email;
        /// <summary>
        /// Crypto Service class
        /// </summary>
       

        private readonly IUrlHelper _urlHelper;

        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jwtservices"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="configuration"></param>
        public LoginRepo(ClarionECOMDBContext context, JwtServices jwtservices, IEmail email, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _jwtServices = jwtservices;
            _email = email;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

        }

        ///// <summary>
        ///// Email Configuration
        ///// </summary>
        ///// <param name="token"></param>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public async Task<bool> ConfirmEmailAsync(string token, string email)
        //{
        //    if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
        //    {
        //        return false;
        //    }

        //    var user = await _context.UserMasters.FirstOrDefaultAsync(u => u.UserEmail == email);

        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    if (user.EmailConfirmationToken != token)
        //    {
        //        return false;
        //    }



        //    // Update user's email confirmation status 
        //    user.EmailConfirmation = 1;
        //    user.EmailConfirmationOn = DateTime.UtcNow; // Set confirmation date/time
        //    user.EmailConfirmationToken = null; // Clear the token

        //    _context.Update(user);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<LogInResponse>> Authenticate(LogInModel login)
        {
            ApiResult<LogInResponse> result = new ApiResult<LogInResponse>();
            try
            {
                bool isTempPwdLogin = false;
                if (login == null)
                {
                    return result.ValidationErrorResponse("Please specify credentials");
                }
                if (string.IsNullOrEmpty(login.Username.Trim()))
                {
                    return result.ValidationErrorResponse("Please specify username");
                }
                if (string.IsNullOrEmpty(login.Password.Trim()))
                {
                    return result.ValidationErrorResponse("Please specify password.");
                }
                var userResult = _context.UserMasters.AsNoTracking().FirstOrDefault(u => (u.LoginName == login.Username || u.Email == login.Username && u.LoginPassword == login.Password));


                if (userResult == null)
                {
                    return result.ValidationErrorResponse("Invalid credentials.");
                }
               
                LogInResponse userInfo = new LogInResponse
                {
                    UserName = userResult.LoginName,
                    Email = userResult.Email,
                    UserID = userResult.UserID,
                    RoleID = userResult.UserRoleID,
                    MobileNo = userResult.MobileNo,
                    CompanyID = userResult.CompanyID,
                    IsTempPasswordLogin = isTempPwdLogin
                };

                userInfo.Token = _jwtServices.GenerateToken(userInfo);
                result.SuccessResponse("Login successful", userInfo);
                return result;
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenKeyWrapException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }

}
