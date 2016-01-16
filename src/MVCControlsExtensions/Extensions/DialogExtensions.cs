using System;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCControls.Extensions
{
    /// <summary>
    /// DialogExtensions
    /// </summary>
    public static class DialogExtensions
    {
        /// <summary>
        /// Dialogs the link with iframe.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="url">The URL.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValue">The route value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static IHtmlString DialogLinkWithIframe<TModel>(this HtmlHelper<TModel> htmlHelper, UrlHelper url,
                                                               string linkText,
                                                               string actionName, string controllerName = null,
                                                               object routeValue = null,
                                                               object htmlAttributes = null,
                                                               DialogOptions options = null)
        {

            var routValueAttributes = GetDialogAttributes(url, actionName, controllerName,
                                                          routeValue, htmlAttributes, options);
            routValueAttributes.Add("onclick", "showDialogWithIframe(this);");

            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(routValueAttributes);

            return MvcHtmlString.Create(string.Format("<a {0}>{1}</a>", attributes, linkText));
        }

        /// <summary>
        /// Dialogs the link with URL.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="url">The URL.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValue">The route value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static MvcHtmlString DialogLinkWithUrl<TModel>(this HtmlHelper<TModel> htmlHelper, UrlHelper url,
                                                               string linkText,
                                                               string actionName, string controllerName = null,
                                                               object routeValue = null,
                                                               object htmlAttributes = null,
                                                               DialogOptions options = null)
        {

            var routValueAttributes = GetDialogAttributes(url, actionName, controllerName,
                                                          routeValue, htmlAttributes, options);
            routValueAttributes.Add("onclick", "showDialogWithUrl(this);");

            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(routValueAttributes);

            return MvcHtmlString.Create(string.Format("<a {0}>{1}</a>", attributes, linkText));
        }

        /// <summary>
        /// Dialogs the button with iframe.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="url">The URL.</param>
        /// <param name="text">The text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValue">The route value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="options">The options.</param>
        /// <param name="buttonMarkup">if set to <c>true</c> [button markup].</param>
        /// <returns></returns>
        public static IHtmlString DialogButtonWithIframe<TModel>(this HtmlHelper<TModel> htmlHelper, UrlHelper url,
                                                                 string text,
                                                                 string actionName, string controllerName = null,
                                                                 object routeValue = null,
                                                                 object htmlAttributes = null,
                                                                 DialogOptions options = null,
                                                                 bool buttonMarkup = false)
        {
            const string htmlInputMarkup = "<input id=\"{0}\" type=\"button\" value=\"{1}\" {2}/>";
            const string htmlButtomMarkup = "<button id=\"{0}\" type=\"button\" {2}/>{1}</button>";

            var routValueAttributes = GetDialogAttributes(url, actionName, controllerName,
                                                          routeValue, htmlAttributes, options);
            routValueAttributes.Add("onclick", "showDialogWithIframe(this);");

            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(routValueAttributes);
            var buttonId = string.Format("btn{0}{1}", controllerName, actionName);
            return MvcHtmlString.Create(string.Format(buttonMarkup ? htmlButtomMarkup : htmlInputMarkup,
                buttonId, text, attributes));
        }
        /// <summary>
        /// Dialogs the button with URL.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="url">The URL.</param>
        /// <param name="text">The text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValue">The route value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static MvcHtmlString DialogButtonWithUrl<TModel>(this HtmlHelper<TModel> htmlHelper, UrlHelper url,
                                                              string text,
                                                              string actionName, string controllerName = null,
                                                              object routeValue = null,
                                                              object htmlAttributes = null,
                                                              DialogOptions options = null)
        {
            var routValueAttributes = GetDialogAttributes(url, actionName, controllerName,
                                                          routeValue, htmlAttributes, options);
            routValueAttributes.Add("onclick", "showDialogWithUrl(this);");

            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(routValueAttributes);

            return MvcHtmlString.Create(string.Format("<input type=\"button\" value=\"{0}\" {1}/>", text, attributes));
        }

        /// <summary>
        /// create a hidden view. to show dialog call JavaScript "pushElementAsDialog("#{PropertyName}Dialog");"
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static MvcHtmlString DialogControlFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                                                                        Expression<Func<TModel, TProperty>> expression,
                                                                        string partialViewName, DialogOptions options)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var dialogId = name + "Dialog";

            var dialogControlData = new StringBuilder(1024);
            dialogControlData.AppendFormat("<div id='{0}' class='MainOverlayDIV'>", dialogId);
            dialogControlData.AppendFormat("\t<div class='{0}'>", options.DialogClass);
            dialogControlData.AppendFormat(
                "\t\t<a style ='float:right' href='#' id='closeButton' class='dialogCloseButton' onclick='popDialog({{element:this,removeElement:false}})' data-linkDivId='{0}'>X</a>"
                , dialogId);

            dialogControlData.Append(helper.PartialFor(expression, partialViewName));

            dialogControlData.Append("\t</div>");

            dialogControlData.Append("</div>");

            return MvcHtmlString.Create(dialogControlData.ToString());
        }

        private static RouteValueDictionary GetDialogAttributes(UrlHelper url, string actionName, string controllerName,
                                                                object routeValue, object htmlAttributes, DialogOptions dialogptions)
        {
            var routValueAttributes = AttributesExtensions.GetAttributeList(htmlAttributes);

            routValueAttributes.Add("data-url", url.Action(actionName, controllerName, routeValue));

            if (dialogptions != null && !string.IsNullOrEmpty(dialogptions.DialogClass))
                routValueAttributes.Add("data-dialogClass", dialogptions.DialogClass);

            if (dialogptions != null && !string.IsNullOrEmpty(dialogptions.DialogTitle))
                routValueAttributes.Add("data-dialogTitle", dialogptions.DialogTitle);

            if (dialogptions != null && !string.IsNullOrEmpty(dialogptions.OnDialogCancel))
                routValueAttributes.Add("data-ondialogCancel", dialogptions.OnDialogCancel);

            if (dialogptions != null && !string.IsNullOrEmpty(dialogptions.OnDialogClose))
                routValueAttributes.Add("data-ondialogClose", dialogptions.OnDialogClose);

            return routValueAttributes;
        }
    }

    /// <summary>
    /// DialogOptions
    /// </summary>
    public class DialogOptions
    {
        /// <summary>
        /// Gets or sets the dialog title.
        /// </summary>
        /// <value>
        /// The dialog title.
        /// </value>
        public string DialogTitle { get; set; }
        /// <summary>
        /// Gets or sets the dialog class.
        /// </summary>
        /// <value>
        /// The dialog class.
        /// </value>
        public string DialogClass { get; set; }

        /// <summary>
        /// Gets or sets the on dialog close.
        /// </summary>
        /// <value>
        /// The on dialog close.
        /// </value>
        public string OnDialogClose { get; set; }
        /// <summary>
        /// Gets or sets the on dialog cancel.
        /// </summary>
        /// <value>
        /// The on dialog cancel.
        /// </value>
        public string OnDialogCancel { get; set; }
    }

}