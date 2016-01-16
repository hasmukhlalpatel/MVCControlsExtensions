using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MVCControls.Extensions
{
    /// <summary>
    /// Form Extensions
    /// </summary>
    public static class FormExts
    {
        /// <summary>
        /// Begins the form with query string.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns></returns>
        public static MvcForm BeginFormWithQueryString(this HtmlHelper htmlHelper)
        {
            var routValue = new RouteValueDictionary();
            if (htmlHelper.ViewContext.HttpContext.Request.Url != null
                && !string.IsNullOrEmpty(htmlHelper.ViewContext.HttpContext.Request.Url.Query))
            {
                var queryStrs = htmlHelper.ViewContext.HttpContext.Request.Url.Query.Replace("?", "").Split('&');
                foreach (var queryStrArr in queryStrs
                    .Select(q => q.Split('='))
                    .Where(q => !routValue.ContainsKey(q[0])))
                {
                    routValue.Add(queryStrArr[0], queryStrArr[1]);
                }
            }

            return htmlHelper.BeginForm(routValue);
        }

        /// <summary>
        /// Displays the errors.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="defaultMessage">The default message.</param>
        /// <returns></returns>
        public static MvcHtmlString DisplayErrors<TModel>(this HtmlHelper<TModel> htmlHelper, string defaultMessage = null)
        {
            if (htmlHelper.ViewData.ModelState.IsValid)
            {
                return new MvcHtmlString(string.Format("<{0}>{1}</{0}>", "span", defaultMessage));
            }
            var errors = (from kp in htmlHelper.ViewData.ModelState
                          where kp.Value.Errors.Count > 0
                          select string.Format("<{0} class=\"field-validation-error\" for=\"{1}\">{2}</{0}>",
                              "span", kp.Key, kp.Value.Errors.First().ErrorMessage))
                .ToList();

            return new MvcHtmlString(errors.Aggregate((c, n) => c + n));
        }

    }
}
