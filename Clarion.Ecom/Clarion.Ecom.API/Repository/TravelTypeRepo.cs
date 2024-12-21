using Clarion.Ecom.API.DAL;
using Clarion.Ecom.API.Extensions;
using Clarion.Ecom.API.IRepository;
using Clarion.Ecom.API.Models;
using Clarion.Ecom.API.NonEntity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Clarion.Ecom.API.Repository
{
    public class TravelTypeRepo: ITravelTypeRepo
    {
        string spName = string.Empty;
        string sqlConnectionString = string.Empty;
        SqlParameter[] param;

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
        public TravelTypeRepo(ClarionECOMDBContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Retrieves all Travel Type
        /// </summary>
        /// <returns></returns>
        public ApiResult<TravelTypeModel> GetAllTravelTypeDetails()
        {
            ApiResult<TravelTypeModel> result = new ApiResult<TravelTypeModel>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                var typelist = _context.TravelTypes.Where(c => c.TravelTypeStatus != 99 && c.CompanyID == claimData!.CompanyID).Select(c => new TravelTypeModel
                {
                    TravelTypeID = c.TravelTypeID,
                    TravelTypeName = c.TravelTypeName,
                    Remarks = c.Remarks,
                    TravelTypeStatus = c.TravelTypeStatus
                }).ToList();

                if (typelist.Count > 0)
                {
                    return result.SuccessResponse("Success", typelist);
                }
                return result.SuccessResponse("No data found", typelist);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves Travel Types 
        /// </summary>
        /// <param name="travelTypeID"></param>
        /// <returns></returns>
        public ApiResult<TravelTypeModel> GetTravelTypeDetailsByID(long travelTypeID)
        {
            ApiResult<TravelTypeModel> result = new ApiResult<TravelTypeModel>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                var type = _context.TravelTypes.Where(c => c.TravelTypeID == travelTypeID && c.CompanyID == claimData!.CompanyID).Select(c => new TravelTypeModel
                {
                    TravelTypeID = c.TravelTypeID,
                    TravelTypeName = c.TravelTypeName,
                    Remarks = c.Remarks,
                    TravelTypeStatus = c.TravelTypeStatus
                }).ToList();

                if (type.Count > 0)
                {
                    return result.SuccessResponse("Success", type);
                }
                return result.SuccessResponse("No data found", type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add or update Travel Types Details
        /// </summary>
        /// <param name="objtravelType"></param>
        /// <returns></returns>
        public async Task<ApiResult<TravelTypeModel>> AddorupdateTravelType(TravelTypeModel objtravelType)
        {
            ApiResult<TravelTypeModel> result = new ApiResult<TravelTypeModel>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                // Check if the logged-in user's CompanyID matches the CompanyID t
                if (objtravelType.CompanyID != null && objtravelType.CompanyID > 0 && claimData?.CompanyID != objtravelType.CompanyID)
                {
                    return result.ValidationErrorResponse("Invalid company id");
                }
                if (string.IsNullOrEmpty(objtravelType.TravelTypeName))
                {
                    return result.ValidationErrorResponse("Please provide travel type name");
                }

                var checkdTravelTypename = _context.TravelTypes.Where(s => s.TravelTypeName.ToLower() == objtravelType.TravelTypeName.ToLower() && s.TravelTypeID != objtravelType.TravelTypeID && s.CompanyID == claimData!.CompanyID).FirstOrDefault();
                if (checkdTravelTypename != null)
                {
                    return result.ValidationErrorResponse("Travel Type Name Already Exists");
                }

                var typeRequest = _context.TravelTypes.Where(c => c.TravelTypeID == objtravelType.TravelTypeID).FirstOrDefault();
                if (typeRequest == null)
                {
                    var TypeEntity = new TravelType();
                    TypeEntity.TravelTypeID = objtravelType.TravelTypeID;
                    TypeEntity.TravelTypeName = objtravelType.TravelTypeName;
                    TypeEntity.Remarks = objtravelType.Remarks;
                    TypeEntity.CompanyID = claimData!.CompanyID;
                    TypeEntity.CreatedBy = claimData!.UserID;
                    TypeEntity.CreatedOn = DateTime.UtcNow;
                    TypeEntity.TravelTypeStatus = 0; //objtravelType.TravelTypeStatus.Value;

                    _context.TravelTypes.Add(TypeEntity);
                    await _context.SaveChangesAsync();
                    return result.SuccessResponse("Created successfully", objtravelType);
                }
                if (string.IsNullOrWhiteSpace(objtravelType.TravelTypeName))
                {
                    return result.ValidationErrorResponse("Please provide a travel type name");
                }
                else
                {
                    typeRequest.TravelTypeID = objtravelType.TravelTypeID;
                    typeRequest.TravelTypeName = objtravelType.TravelTypeName;
                    typeRequest.Remarks = objtravelType.Remarks;
                    typeRequest.CompanyID = claimData!.CompanyID;
                    typeRequest.LastModifiedBy = claimData!.UserID;
                    typeRequest.LastModifiedOn = DateTime.UtcNow;
                    //typeRequest.TravelTypeStatus = objtravelType.TravelTypeStatus.Value;

                    _context.SaveChanges();
                    return result.SuccessResponse("Updated successfully", objtravelType);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// DeleteTravelTypeDetailsByID
        /// </summary>
        /// <param name="travelTypeID"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> UpdateTravelTypeStatus(TravelTypeModel objtravelType)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                var exisitingtype = _context.TravelTypes.FirstOrDefault(t => t.TravelTypeID == objtravelType.TravelTypeID && t.CompanyID == claimData!.CompanyID);
                if (exisitingtype == null)
                {
                    return result.ValidationErrorResponse("Travel type not found.");
                }
                else
                {
                    exisitingtype.LastModifiedBy = claimData!.UserID;
                    exisitingtype.LastModifiedOn = DateTime.UtcNow;
                    exisitingtype.TravelTypeStatus = objtravelType.TravelTypeStatus.Value;
                    _context.SaveChanges();
                    return result.SuccessResponse("Status Updated Successfully", true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetTravelTypeBySearch
        /// </summary>
        /// <param name="objSearch"></param>
        /// <returns></returns>
        public ApiResult<TravelTypeModel> GetTravelTypeBySearch(TravelTypeModel objSearch)
        {
            var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
            DataAccess objDBContext = new DataAccess(sqlConnectionString);
            try
            {
                string spName = "[dbo].[uspGetTravelTypeBySearch]";
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@CompanyID", claimData.CompanyID);
                param[1] = new SqlParameter("@TravelTypeName", objSearch.TravelTypeName);
                param[2] = new SqlParameter("@TravelTypeStatus", objSearch.TravelTypeStatus);

                DataTable dtResult =  objDBContext.ExecuteReader(spName, param);

                // Assuming ConvertDataTable returns a List<TravelTypeModel>
                List<TravelTypeModel> travelTypeList = CommonClass.ConvertDataTable<TravelTypeModel>(dtResult);

                // Create an ApiResult object and return it
                ApiResult<TravelTypeModel> result = new ApiResult<TravelTypeModel>
                {
                    ResponseCode = 1,
                    ResponseData = travelTypeList
                };

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }






    }


}


