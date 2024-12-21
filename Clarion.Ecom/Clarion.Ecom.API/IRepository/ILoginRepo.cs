using Clarion.Ecom.API.NonEntity;
using Clarion.Ecom.API.Models;
using Clarion.Ecom.API.NonEntity;

namespace Clarion.Ecom.API.IRepository
{
    /// <summary>
    /// Login operations
    /// </summary>
    public interface ILoginRepo
    {
       
        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<ApiResult<LogInResponse>> Authenticate(LogInModel login);
        /// <summary>
        /// ConfirmEmailAsync
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <returns></returns>
       // Task<bool> ConfirmEmailAsync(string token, string email);
       
      
    }

}
