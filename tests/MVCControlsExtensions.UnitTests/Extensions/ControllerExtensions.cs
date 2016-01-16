using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace MVCControlsExtensions.UnitTests.Extensions
{
    public static class ControllerExtensions
    {
        public static void SetHttpContext(this Controller controller,
            RouteData routeData = null,
            NameValueCollection queryString = null,
            HttpCookieCollection cookies = null,
            IPrincipal principal = null)
        {
            controller.ControllerContext = GetMoqControllerContext(controller, routeData,
                queryString, cookies, principal);

        }

        public static ControllerContext GetMoqControllerContext(Controller controller,
            RouteData routeData = null,
            NameValueCollection queryString = null,
            HttpCookieCollection cookies = null,
            IPrincipal principal = null,
            NameValueCollection headers = null)
        {
            cookies = cookies ?? new HttpCookieCollection();
            queryString = queryString ?? new NameValueCollection();
            headers = headers ?? new NameValueCollection();
            routeData = routeData ?? GetDefaultRouteData();

            var moqHttpContext = HttpHelper.GetMoqHttpContextBase(queryString, cookies, principal);

            controller.Url = new UrlHelper(new RequestContext(moqHttpContext, routeData));

            return new ControllerContext(moqHttpContext, routeData, controller);
        }


        private static RouteData GetDefaultRouteData()
        {
            var routeData = new RouteData();
            routeData.Values.Add("action", "Index");
            routeData.Values.Add("controller", "Home");
            return routeData;
        }

        internal class TestPrincipal : IPrincipal
        {
            private readonly TestIdentity _identity = new TestIdentity() { Roles = new[] { "Admin" } };

            public IIdentity Identity
            {
                get { return _identity; }
            }

            public bool IsInRole(string role)
            {
                return true;
            }
        }

        internal class TestIdentity : IIdentity
        {

            public string AuthenticationType
            {
                get { return string.Empty; }
            }

            public bool IsAuthenticated
            {
                get { return true; }
            }

            public string Name
            {
                get { return "TestUser"; }
            }

            public string[] Roles { get; set; }
        }
    }
}