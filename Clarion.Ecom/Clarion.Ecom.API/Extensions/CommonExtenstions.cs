using Clarion.Ecom.API.NonEntity;
using Microsoft.AspNetCore.Http.Extensions;
using System.Data;
using System.Reflection;
using System.Text.Json;


using System.Collections.Generic;

namespace Clarion.Ecom.API.Extensions
{
    public static class ApiResultExtensions
    {
        public static ApiResult<T> ExceptionResponse<T>(this ApiResult<T> result, string message, Exception ex)
        {
            result.ResponseCode = 0;
            result.Message = message;
            result.ErrorDesc = ex.ToString();
            return result;
        }
        public static ApiResult<T> ValidationErrorResponse<T>(this ApiResult<T> result, string message)
        {
            result.ResponseCode = 2;
            result.Message = message;
            return result;
        }
        public static ApiResult<T> SuccessResponse<T>(this ApiResult<T> result, string message, List<T>? data)
        {
            result.ResponseCode = 1;
            result.Message = message;
            if (data == null || data.Count == 0)
            {
                result.Message = "No records found.";
            }
            result.ResponseData = data ?? new List<T>();
            return result;
        }
        public static ApiResult<T> SuccessResponse<T>(this ApiResult<T> result, string message, T data)
        {
            result.ResponseCode = 1;
            result.Message = message;
            result.ResponseData.Add(data);
            return result;
        }
    }

    public static class CommonExtenstions
    {
        public static void LogErrorDetails<T, U>(this ILogger logger, Exception ex, string message, IHttpContextAccessor httpContextAccessor, T requestData, U responseData)
        {
            logger.LogError(ex, "{IsError}{ErrorCode}{LogMessage}{RequestURI}{RequestBody},{ResponseData}", [true,
                StatusCodes.Status500InternalServerError,
                message,
                httpContextAccessor!.HttpContext!.Request.GetDisplayUrl(),
                JsonSerializer.Serialize(requestData),
                JsonSerializer.Serialize(responseData)]);
        }
        /// <summary>
        /// Converts comma(,) separated string to array of long data type
        /// </summary>
        /// <param name="ids">Comma separated Ids</param>
        /// <returns></returns>
        public static long[] ToLongArray(this string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(ids.Trim()))
                {
                    return new long[] { };
                }
                string[] splittedIds = ids.Split(',');
                List<long> lstLong = new List<long>();
                foreach (var id in splittedIds)
                {
                    lstLong.Add(Convert.ToInt64(id));
                }
                return lstLong.ToArray();
            }
            catch (Exception ex)
            {
                return new long[] { };
            }
        }
        /// <summary>
        /// Converts a string to array of long data type based on the separator
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static long[] ToLongArray(this string ids, char separator)
        {
            try
            {
                if (string.IsNullOrEmpty(ids.Trim()))
                {
                    return new long[] { };
                }
                string[] splittedIds = ids.Split(separator);
                List<long> lstLong = new List<long>();
                foreach (var id in splittedIds)
                {
                    lstLong.Add(Convert.ToInt64(id));
                }
                return lstLong.ToArray();
            }
            catch (Exception ex)
            {
                return new long[] { };
            }
        }
    }


    public abstract class CommonClass
    {

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (dr[column.ColumnName] == DBNull.Value)
                        {
                            pro.SetValue(obj, null, null);
                        }
                        else
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }
    }

}
