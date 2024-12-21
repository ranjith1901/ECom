using Clarion.Ecom.API.Extensions;
using Clarion.Ecom.API.IRepository;
using Clarion.Ecom.API.Models;
using Clarion.Ecom.API.NonEntity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Clarion.Ecom.API.Repository
{
    public class TravelDurationRepo: ITravelDurationRepo
    {

        /// <summary>
        /// DBContext
        /// </summary>
        private ClarionECOMDBContext _context;

        /// <summary>
        /// Current Http ContextAccessor
        /// </summary>
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contextAccessor"></param>
        public TravelDurationRepo(ClarionECOMDBContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Retrieves all Travel Duration
        /// </summary>
        /// <returns></returns>
        public ApiResult<TravelDurationModel> GetAllTravelDurationDetails()
        {
            ApiResult<TravelDurationModel> result = new ApiResult<TravelDurationModel>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                var durationlist = _context.TravelDurations.Where(c => c.DurationStatus != 99 && c.CompanyID == claimData!.CompanyID).Select(c => new TravelDurationModel
                {
                    DurationID = c.DurationID,
                    DurationName = c.DurationName,
                    Remarks = c.Remarks,
                    CompanyID = c.CompanyID
                }).ToList();

                if (durationlist.Count > 0)
                {
                    return result.SuccessResponse("Success", durationlist);
                }
                return result.SuccessResponse("No data found", durationlist);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves Travel Duration 
        /// </summary>
        /// <param name="durationID"></param>
        /// <returns></returns>
        public ApiResult<TravelDurationModel> GetTravelDurationDetailsByID(long durationID)
        {
            ApiResult<TravelDurationModel> result = new ApiResult<TravelDurationModel>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                var duration = _context.TravelDurations.Where(c => c.DurationStatus != 99 && c.DurationID == durationID && c.CompanyID == claimData!.CompanyID).Select(c => new TravelDurationModel
                {
                    DurationID = c.DurationID,
                    DurationName = c.DurationName,
                    Remarks = c.Remarks,
                    CompanyID = c.CompanyID
                }).ToList();

                if (duration.Count > 0)
                {
                    return result.SuccessResponse("Success", duration);
                }
                return result.SuccessResponse("No data found", duration);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add or update Travel Duration Details
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public async Task<ApiResult<TravelDurationModel>> AddorupdateTravelDuration(TravelDurationModel duration)
        {
            ApiResult<TravelDurationModel> result = new ApiResult<TravelDurationModel>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                // Check if the logged-in user's CompanyID matches the CompanyID t
                if (duration.CompanyID != null && duration.CompanyID > 0 && claimData?.CompanyID != duration.CompanyID)
                {
                    return result.ValidationErrorResponse("Invalid company id");
                }
                if (string.IsNullOrEmpty(duration.DurationName))
                {
                    return result.ValidationErrorResponse("Please provide duration name");
                }

                var checkdurationname = _context.TravelDurations.Where(s => s.DurationName.ToLower() == duration.DurationName.ToLower() && s.DurationID != duration.DurationID && s.CompanyID == claimData!.CompanyID).FirstOrDefault();
                if (checkdurationname != null)
                {
                    return result.ValidationErrorResponse("Duration Name Already Exists");
                }

                var durationRequest = _context.TravelDurations.Where(c => c.DurationID == duration.DurationID).FirstOrDefault();
                if (durationRequest == null)
                {
                    var DurationEntity = new TravelDuration();
                    DurationEntity.DurationID = duration.DurationID;
                    DurationEntity.DurationName = duration.DurationName;
                    DurationEntity.Remarks = duration.Remarks;
                    DurationEntity.CompanyID = claimData!.CompanyID;
                    DurationEntity.CreatedBy = claimData!.UserID;
                    DurationEntity.CreatedOn = DateTime.UtcNow;
                    DurationEntity.DurationStatus = duration.DurationStatus;

                    _context.TravelDurations.Add(DurationEntity);
                    await _context.SaveChangesAsync();
                    return result.SuccessResponse("Created successfully", duration);
                }
                if (string.IsNullOrWhiteSpace(duration.DurationName))
                {
                    return result.ValidationErrorResponse("Please provide a duration name");
                }
                else
                {
                    durationRequest.DurationID = duration.DurationID;
                    durationRequest.DurationName = duration.DurationName;
                    durationRequest.Remarks = duration.Remarks;
                    durationRequest.CompanyID = claimData!.CompanyID;
                    durationRequest.LastModifiedBy = claimData!.UserID;
                    durationRequest.LastModifiedOn = DateTime.UtcNow;
                    durationRequest.DurationStatus = duration.DurationStatus;

                    _context.SaveChanges();
                    return result.SuccessResponse("Updated successfully", duration);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// DeleteTravelDurationDetailsByID
        /// </summary>
        /// <param name="durationID"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> DeleteTravelDurationDetailsByID(long durationID)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                var exisitingduration = _context.TravelDurations.FirstOrDefault(t => t.DurationID == durationID && t.DurationStatus != 99 && t.CompanyID == claimData!.CompanyID);
                if (exisitingduration == null)
                {
                    return result.ValidationErrorResponse("Travel duration not found.");
                }
                else
                {
                    exisitingduration.LastModifiedBy = claimData!.UserID;
                    exisitingduration.LastModifiedOn = DateTime.UtcNow;
                    exisitingduration.DurationStatus = 99;
                    _context.SaveChanges();
                    return result.SuccessResponse("Deleted Successfully", true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }

    
}


