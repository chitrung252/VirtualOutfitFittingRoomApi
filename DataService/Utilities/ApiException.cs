using DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Utilities
{
    public class ApiException: SystemException
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public ResultEnum ErrorStatus { get; set; }
        public string ErrorMessage { get; set; }

        public static ApiException Get(bool success, string mess, ResultEnum errorStatus, HttpStatusCode statusCode)
        {
            return new ApiException()
            {
                Success = success,
                ErrorMessage = mess,
                ErrorStatus = errorStatus,
                StatusCode = statusCode
            };
        }
    }
}
