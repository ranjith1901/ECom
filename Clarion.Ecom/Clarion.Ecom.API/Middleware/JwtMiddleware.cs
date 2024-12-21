using Clarion.Ecom.API.NonEntity;
using Clarion.Ecom.API.Services;

namespace Clarion.Ecom.API.Middleware
{
    /// <summary>
    /// JWT Middleware
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// Invoke Action
        /// </summary>
        /// <param name="context">App Context</param>
        /// <param name="jwtServices">JWT Service</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, JwtServices jwtServices)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = authorizationHeader?.Replace("Bearer ", string.Empty);

            if (!string.IsNullOrEmpty(token))
            {
                ClaimData claimData = jwtServices.GetClaimData();
                context.Items["ClaimData"] = claimData;
            }

            await _next(context);
        }
    }
}
