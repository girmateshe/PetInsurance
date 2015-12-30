using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Policy.Pets
{
    public static class WebApiConfigeration
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            var kernel = ApiSetup.CreateKernel();

            var resolver = new NinjectDependencyResolver(kernel);

            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            config.Formatters.Add(jsonFormatter);

            //setup dependency resolver for API           
            config.DependencyResolver = resolver;
            //setup dependency for UI controllers
            //DependencyResolver.SetResolver(resolver.GetService, resolver.GetServices);

            foreach (var item in GlobalConfiguration.Configuration.Formatters)
            {
                if (typeof (JsonMediaTypeFormatter) == item.GetType())
                {
                    item.AddQueryStringMapping("responseType", "json", "application/json");
                }
            }
        }
    }
}
