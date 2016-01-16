using System.Web.Mvc;

namespace MvcExtensions.Demo.Areas.AngularJsDemo
{
    public class AngularJsDemoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AngularJsDemo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AngularJsDemo_default",
                "AngularJsDemo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
