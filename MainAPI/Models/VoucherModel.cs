using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyConnect.API.Controllers.Connect.Promotion.Metee;

namespace SkyConnect.API.Models
{
    public class VoucherModel
    {
        public string voucherCode { get; set; }
        public ConfirmVoucherData confirmVoucher { get; set; }
    }
}