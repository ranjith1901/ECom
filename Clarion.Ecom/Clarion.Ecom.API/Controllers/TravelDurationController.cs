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
    public class TravelDurationController : ControllerBase
    {
        private ITravelDurationRepo _travelDurationRepo;
        private readonly ILogger<TravelDurationController> _logger;
        private IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="travelDurationRepo"></param>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        public TravelDurationController(ITravelDurationRepo travelDurationRepo, ILogger<TravelDurationController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _travelDurationRepo = travelDurationRepo;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves all Travel Duration details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllTravelDurationDetails()
        {
            ApiResult<TravelDurationModel> result = new ApiResult<TravelDurationModel>();
            try
            {
                result = _travelDurationRepo.GetAllTravelDurationDetails();
                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, "", result.ExceptionResponse("Error while retriving an travel duration details ", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retriving an travel duration details ", ex)); ;
            }
        }

        /// <summary>
        /// Retrieves travel duration detail  by Id
        /// </summary>
        /// <param name="travelDurationID"></param>
        /// <returns></returns>
        [HttpGet("GetTravelDuration/{travelDurationID}")]
        public IActionResult GetTravelDurationDetailsByID(long travelDurationID)
        {
            ApiResult<TravelDurationModel> result = new ApiResult<TravelDurationModel>();
            try
            {
                result = _travelDurationRepo.GetTravelDurationDetailsByID(travelDurationID);

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }

                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, travelDurationID, result.ExceptionResponse("Error while retrieving a travel duration.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retrieving a travel duration.", ex)); ;
            }
        }

        /// <summary>
        /// Add or update Travel Duration
        /// </summary>
        /// <param name="travelDuration"></param>
        /// <returns></returns>
        [HttpPost("AddorupdateTravelDuration")]
        public async Task<IActionResult> AddorupdateTravelDuration(TravelDurationModel travelDuration)
        {
            ApiResult<TravelDurationModel> result = new ApiResult<TravelDurationModel>();
            try
            {
                result = await _travelDurationRepo.AddorupdateTravelDuration(travelDuration); ; // Await the asynchronous operation

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, "", result.ExceptionResponse("Error while add or update a travel duration.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while add or update a travel duration.", ex)); ;
            }
        }

        /// <summary>
        /// Delete travel duration details by Id
        /// </summary>
        /// <param name="travelDurationID"></param>
        /// <returns></returns>
        [HttpPost("Delete/{travelDurationID}")]
        public async Task<IActionResult> DeleteTravelDurationDetailsByID(long travelDurationID)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            try
            {
                result = await _travelDurationRepo.DeleteTravelDurationDetailsByID(travelDurationID);

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, travelDurationID, result.ExceptionResponse("Error while deleting a travel duration.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while deleting a travel duration.", ex)); ;
            }

        }



    }
}
