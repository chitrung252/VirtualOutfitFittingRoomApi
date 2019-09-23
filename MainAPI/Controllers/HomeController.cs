using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkyConnect.API.Controllers
{
    public class HomeController : Controller
    {
        public class MomoConfig
        {
            public string Version { get; set; }
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //using (StreamReader sr = new StreamReader(Server.MapPath("~/Configuration/Momo.config.json")))
            //{
            //    var a = JsonConvert.DeserializeObject<MomoConfig>(sr.ReadToEnd());

            //}
            return View();
        }
    }
}
