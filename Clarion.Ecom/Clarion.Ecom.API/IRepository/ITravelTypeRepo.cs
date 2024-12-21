using Clarion.Ecom.API.NonEntity;

namespace Clarion.Ecom.API.IRepository
{
    public interface ITravelTypeRepo
    {
        /// <summary>
        /// Retrieves all Travel Type details
        /// </summary>
        /// <returns></returns>
        ApiResult<TravelTypeModel> GetAllTravelTypeDetails();
        /// <summary>
        /// Retrieves Travel Type detail by Id
        /// </summary>
        /// <param name="travelTypeID"></param>
        /// <returns></returns>
        ApiResult<TravelTypeModel> GetTravelTypeDetailsByID(long travelTypeID);
        /// <summary>
        /// Add or update Travel Type detail
        /// </summary>
        /// <param name="objtravelType"></param>
        /// <returns></returns>
        Task<ApiResult<TravelTypeModel>> AddorupdateTravelType(TravelTypeModel objtravelType);
        /// <summary>
        /// Update Travel Type status
        /// </summary>
        /// <param name="objtravelType"></param>
        /// <returns></returns>
        Task<ApiResult<bool>> UpdateTravelTypeStatus(TravelTypeModel objtravelType);

        /// <summary>
        /// GetTravelTypeBySearch
        /// </summary>
        /// <param name="objSearch"></param>
        /// <returns></returns>
        ApiResult<TravelTypeModel> GetTravelTypeBySearch(TravelTypeModel objSearch);

    }
}
