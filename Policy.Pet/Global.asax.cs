using System;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Policy.Pets
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfigeration.Register);

                HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
                config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }
    }
}
