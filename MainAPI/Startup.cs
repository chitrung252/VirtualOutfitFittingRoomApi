using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SkyConnect.API.Startup))]

namespace SkyConnect.API
{
    public partial class Startup
    {
        public static string InternalDesKey = "/+YM2GWzH3fLzsSXImj5gSeizxpx+XSg";
        public static string InternalDesIV = "AIiqPZdEHgs=";
        public void Configuration(IAppBuilder app)
        {
            // PublicKey and PrivateKey Generation
            //SkyConnect.API.Key.KeysForPGPEncryptionDecryption.GenerateKey("server", WebConfigurationManager.AppSettings["PassShare"], Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\");

            ConfigureAuth(app);
        }
    }
}
