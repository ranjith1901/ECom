using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Clarion.Ecom.API.IRepository;
using Clarion.Ecom.API.Models;
using Clarion.Ecom.API.Extensions;
using Clarion.Ecom.API.NonEntity;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;

namespace Clarion.Ecom.API.Controllers
{
    /// <summary>
    /// Login operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TravelTypeController : ControllerBase
    {
        private ITravelTypeRepo _travelTypeRepo;
        private readonly ILogger<TravelTypeController> _logger;
        private IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="travelTypeRepo"></param>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        public TravelTypeController(ITravelTypeRepo travelTypeRepo, ILogger<TravelTypeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _travelTypeRepo = travelTypeRepo;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves all Travel Type details
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllTravelTypeDetails")]
        public IActionResult GetAllTravelTypeDetails()
        {
            ApiResult<TravelTypeModel> result = new ApiResult<TravelTypeModel>();
            try
            {
                result = _travelTypeRepo.GetAllTravelTypeDetails();
                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, "", result.ExceptionResponse("Error while retriving an travel type details ", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retriving an travel type details ", ex)); ;
            }
        }

        /// <summary>
        /// Retrieves travel type detail  by Id
        /// </summary>
        /// <param name="travelTypeID"></param>
        /// <returns></returns>
        [HttpGet("GetTravelType/{travelTypeID}")]
        public IActionResult GetTravelTypeDetailsByID(long travelTypeID)
        {
            ApiResult<TravelTypeModel> result = new ApiResult<TravelTypeModel>();
            try
            {

                result = _travelTypeRepo.GetTravelTypeDetailsByID(travelTypeID);

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }

                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, travelTypeID, result.ExceptionResponse("Error while retrieving a travel type.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retrieving a travel type.", ex)); ;
            }
        }

        /// <summary>
        /// Add or update Travel Type
        /// </summary>
        /// <param name="travelType"></param>
        /// <returns></returns>
        [HttpPost("AddorupdateTravelType")]
        public async Task<IActionResult> AddorupdateTravelType(TravelTypeModel travelType)
        {
            ApiResult<TravelTypeModel> result = new ApiResult<TravelTypeModel>();
            try
            {
                result = await _travelTypeRepo.AddorupdateTravelType(travelType); ; // Await the asynchronous operation

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, "", result.ExceptionResponse("Error while add or update a travel type.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while add or update a travel type.", ex)); ;
            }
        }

        /// <summary>
        /// Update travel type status
        /// </summary>
        /// <param name="travelType"></param>
        /// <returns></returns>
        [HttpPost("UpdateTravelTypeStatus")]
        public async Task<IActionResult> UpdateTravelTypeStatus(TravelTypeModel travelType)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            try
            {
                result = await _travelTypeRepo.UpdateTravelTypeStatus(travelType);

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, travelType, result.ExceptionResponse("Error while Update Travel Type Status.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while Update Travel Type Status.", ex)); ;
            }

        }

        /// <summary>
        /// GetTravelTypeBySearch
        /// </summary>
        /// <param name="travelType"></param>
        /// <returns></returns>
        [HttpPost("GetTravelTypeBySearch")]
        public IActionResult GetTravelTypeBySearch(TravelTypeModel travelType)
        {
            ApiResult<TravelTypeModel> result = new ApiResult<TravelTypeModel>();
            try
            {

                result = _travelTypeRepo.GetTravelTypeBySearch(travelType);

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }

                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, travelType, result.ExceptionResponse("Error while retrieving a travel type.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retrieving a travel type.", ex)); ;
            }
        }


    }
}
