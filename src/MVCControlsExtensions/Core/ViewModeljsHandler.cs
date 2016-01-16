using System;
using System.Web;
using System.Web.Routing;
using MVCControls.Extensions;

namespace MVCControls.Core
{
    /// <summary>
    /// ViewModel JavaScripts Route Handler
    /// </summary>
    public class ViewModeljsRouteHandler : IRouteHandler
    {
        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the request.</param>
        /// <returns>
        /// An object that processes the request.
        /// </returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ViewModeljsHttpHandler(requestContext);
        }
    }

    /// <summary>
    /// ViewModel JavaScripts HttpHandler
    /// </summary>
    public class ViewModeljsHttpHandler : IHttpHandler
    {

        /// <summary>
        /// Gets or sets the request context.
        /// </summary>
        /// <value>
        /// The request context.
        /// </value>
        public RequestContext RequestContext { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModeljsHttpHandler"/> class.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        public ViewModeljsHttpHandler(RequestContext requestContext)
        {
            RequestContext = requestContext;
        }


        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";

            Guid typeId;
            if (!Guid.TryParse(RequestContext.RouteData.Values["typeId"].ToString(), out typeId))
            {
                return;
            }
            var viewModelScripts = KOViewModelBuilder.RenderKOViewModelScripts(typeId);
            var refresh = new TimeSpan(0, 0, 15);
            context.Response.Cache.SetExpires(DateTime.Now.Add(refresh));
            context.Response.Cache.SetMaxAge(refresh);
            context.Response.Cache.SetCacheability(HttpCacheability.Server);
            context.Response.Cache.SetValidUntilExpires(true);
            context.Response.Write(viewModelScripts); 
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        public bool IsReusable { get { return false; } }
    }

    /// <summary>
    /// Ko ViewModels Route Extensions
    /// </summary>
    public static class KoViewModelRouteExts
    {
        /// <summary>
        /// Register ViewModels JavaScripts Bundle
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterJsBundle(this RouteCollection routes)
        {
            var defaults = new RouteValueDictionary { { "typeId", "0000" } };
            var customRoute = new Route("KOViewmodel-{typeId}", defaults, new ViewModeljsRouteHandler());

            routes.Add(customRoute);
        }
    }
}
