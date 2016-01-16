using System.Web.Mvc;
using System.Web.Routing;
using MVCControls.Core;

namespace MvcExtensions.Demo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RegisterJsBundle();
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { area ="", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}