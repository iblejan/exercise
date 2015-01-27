using System.Net.Http.Headers;
using System.Web.Http;

namespace WebApi1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Forcing to JSON. a personal choice.
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "items",
                routeTemplate: "api/{controller}/{name}",
                defaults: new { name = RouteParameter.Optional });

            //config.Routes.MapHttpRoute(
            //    name: "orders",
            //    routeTemplate: "api/{controller}/{action}/{receiptId}");
        }
    }
}
