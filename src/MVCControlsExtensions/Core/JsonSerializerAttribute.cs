using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MVCControls.Core
{
    /// <summary>
    /// JSON Serialize Attribute
    /// </summary>
    public class JsonSerializerAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionParameters = filterContext.ActionDescriptor.GetParameters();
            if (actionParameters.Length == 0)
                return;
            var parameterName = actionParameters[0].ParameterName;
            var parameterType = actionParameters[0].ParameterType;

            var request = filterContext.HttpContext.Request;
            request.InputStream.Position = 0;
            using (var sr = new StreamReader(filterContext.HttpContext.Request.InputStream))
            {
                var inputContent = sr.ReadToEnd();
                var jsonSerializer = new JavaScriptSerializer();
                var result = jsonSerializer.Deserialize(inputContent, parameterType);
                filterContext.ActionParameters[parameterName] = result;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
