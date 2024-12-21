using Clarion.Ecom.API.NonEntity;

namespace Clarion.Ecom.API.IRepository
{
    public interface ITravelDurationRepo
    {
        /// <summary>
        /// Retrieves  all Duration details
        /// </summary>
        /// <returns></returns>
        ApiResult<TravelDurationModel> GetAllTravelDurationDetails();
        /// <summary>
        /// Retrieves Duration detail by Id
        /// </summary>
        /// <param name="durationID"></param>
        /// <returns></returns>
        ApiResult<TravelDurationModel> GetTravelDurationDetailsByID(long durationID);
        /// <summary>
        /// Add or update duration detail
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        Task<ApiResult<TravelDurationModel>> AddorupdateTravelDuration(TravelDurationModel duration);
        /// <summary>
        /// Delete duration detail by Id
        /// </summary>
        /// <param name="durationID"></param>
        /// <returns></returns>
        Task<ApiResult<bool>> DeleteTravelDurationDetailsByID(long durationID);
    }
}
