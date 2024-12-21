namespace Clarion.Ecom.API.NonEntity
{
    public class ApiResult<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiResult()
        {
            ResponseCode = -1;
            Message = string.Empty;
            ErrorDesc = string.Empty;
            ResponseData = new List<T>();
        }
        /// <summary>
        /// Response Code
        /// 0 - for Exception
        /// 1 - for Success response
        /// 2 - for Validation errors
        /// </summary>
        public int ResponseCode { get; set; }
        /// <summary>
        /// Response Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Error Description/Details. Usually the exception trace.
        /// </summary>
        public string ErrorDesc { get; set; }
        /// <summary>
        /// Response Data
        /// </summary>
        public List<T> ResponseData { get; set; }
    }

}
