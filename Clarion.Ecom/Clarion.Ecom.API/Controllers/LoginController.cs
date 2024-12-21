using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Clarion.Ecom.API.IRepository;
using Clarion.Ecom.API.Models;
using Clarion.Ecom.API.Extensions;
using Clarion.Ecom.API.NonEntity;
using System.Diagnostics.Metrics;

namespace Clarion.Ecom.API.Controllers
{
    /// <summary>
    /// Login operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginRepo _loginRepo;
        private readonly ILogger<LoginController> _logger;
        private IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loginRepo"></param>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        public LoginController(ILoginRepo loginRepo, ILogger<LoginController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _loginRepo = loginRepo;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Login(LogInModel loginModel)
        {
            ApiResult<LogInResponse> result = new ApiResult<LogInResponse>();
            try
            {
                result = await _loginRepo.Authenticate(loginModel);
                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authentication user");
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error authentication user", ex));
            }
        }


        
    }
}
