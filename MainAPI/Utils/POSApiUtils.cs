using DataService.Models;
using DataService.Utilities;
using Newtonsoft.Json;
using SkyConnect.API.Models;
using System;
using System.Net;
using System.Net.Http;
namespace SkyConnect.API.Utils
{
    public class POSApiUtils
    {
        public static HttpResponseMessage MissingPartnerMessage()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotImplemented,
                Content = new JsonContent(new
                {
                    statusCode = (int)ResultEnum.MissingPartner,
                    message = "Đối tác này chưa sẵn sàng.",
                    success = false
                })
            };
        }
        public static HttpResponseMessage InternalServerErrorMessage(Exception ex)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new JsonContent(new
                {
                    statusCode = (int)ResultEnum.InternalError,
                    message = "Có lỗi hệ thống POS Api.",
                    success = false,
                    data = ex.ToString()
                })
            };
        }
    }

}