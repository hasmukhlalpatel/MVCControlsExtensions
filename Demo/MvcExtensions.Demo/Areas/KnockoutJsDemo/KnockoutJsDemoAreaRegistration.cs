using System.Web.Mvc;

namespace MvcExtensions.Demo.Areas.KnockoutJsDemo
{
    public class KnockoutJsDemoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "KnockoutJsDemo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "KnockoutJsDemo_default",
                "KnockoutJsDemo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
