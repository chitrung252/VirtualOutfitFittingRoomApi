using DataService.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.APIModels
{
    public class BaseResponse<T>
    {
        [JsonProperty("result-code")]
        public int ResultCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }

        public static BaseResponse<string> GetInternalServerError(Exception ex, string messagePrefix)
        {
            return new BaseResponse<string>()
            {
                ResultCode = (int)ResultEnum.InternalError,
                Success = false,
                Message = messagePrefix,
                Data = ex.ToString()
            };
        }
        public static BaseResponse<T> Get(bool success, string mess, T data, ResultEnum resultCode)
        {
            return new BaseResponse<T>()
            {
                Success = success,
                Message = mess,
                Data = data,
                ResultCode = (int)resultCode
            };
        }

        public static BaseResponse<T> Get(bool success, string mess, T data, ResultEnum resultCode, string error)
        {
            return new BaseResponse<T>()
            {
                Success = success,
                Message = mess,
                Data = data,
                ResultCode = (int)resultCode,
                Error = error
            };
        }

    }
}
