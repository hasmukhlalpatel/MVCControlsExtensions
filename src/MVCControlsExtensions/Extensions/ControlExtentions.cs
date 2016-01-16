using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MVCControls.Core;
using System.Web.Routing;
using MVCControls.Models;

namespace MVCControls.Extensions
{
    /// <summary>
    /// Control Extensions
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Drops down list for the expression property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The property expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes=null)
        {
            var attributeList = htmlAttributes != null
                ? AttributesExtensions.GetAttributeList(htmlAttributes)
                : new RouteValueDictionary();

            return DropDownListFor(htmlHelper, expression, attributeList);
        }

        /// <summary>
        /// Drops down list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IDictionary<string, object> htmlAttributes)
        {
            var selectListItems = GetSelectListItems(htmlHelper, expression);

            return htmlHelper.DropDownListFor(expression, selectListItems, htmlAttributes);
        }

        /// <summary>
        /// Merges the table.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="partialResult1">The partial result1.</param>
        /// <param name="partialResult2">The partial result2.</param>
        /// <returns></returns>
        public static MvcHtmlString MergeTable<TModel>(this HtmlHelper<TModel> helper,
                                                                  MvcHtmlString partialResult1,
                                                                  MvcHtmlString partialResult2)
        {
            if (partialResult1 == null && partialResult2 == null)
                return MvcHtmlString.Empty;

            if (partialResult1 == null)
                return partialResult2;
            if (partialResult2 == null)
                return partialResult1;

            var result1 = partialResult1.ToString();
            var lastClosingtable = result1.LastIndexOf("</table>", StringComparison.OrdinalIgnoreCase);

            var result2 = partialResult2.ToString();
            var firstOpeningtable = result2.LastIndexOf("<table", StringComparison.OrdinalIgnoreCase);

            if (lastClosingtable >= 0 && firstOpeningtable >= 0)
            {
                result1 = result1.Substring(0, lastClosingtable); //no need any things after </table>
                result2 = result2.Substring(firstOpeningtable + 7); // remove <table>
            }

            return MvcHtmlString.Create(result1 + result2);
        }

        /// <summary>
        /// Partial for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <returns></returns>
        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, string partialViewName)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            return Partial(helper, partialViewName, model, name);
        }

        /// <summary>
        /// Partials for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <param name="htmlFieldPrefix">The HTML field prefix.</param>
        /// <returns></returns>
        public static MvcHtmlString PartialFor<TModel>(this HtmlHelper<TModel> helper,string partialViewName, string htmlFieldPrefix)
        {
            return Partial(helper, partialViewName, helper.ViewData.Model, htmlFieldPrefix);
        }


        /// <summary>
        /// Partials the specified helper.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <param name="model">The model.</param>
        /// <param name="htmlFieldPrefix">The HTML field prefix.</param>
        /// <returns></returns>
        public static MvcHtmlString Partial<TModel>(this HtmlHelper<TModel> helper, string partialViewName, object model, string htmlFieldPrefix)
        {
            var viewData = new ViewDataDictionary
            {
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = htmlFieldPrefix
                }
            };

            return helper.Partial(partialViewName, model, viewData);
        }


        /// <summary>
        /// Labels the CheckBox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="checkboxAttributes">The check box attributes.</param>
        /// <param name="labelAttributes">The label attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString LabelCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
                                                            Expression<Func<TModel, bool>> expression,
                                                            object checkboxAttributes = null,
                                                            object labelAttributes = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(checkboxAttributes);

            return LabelCheckBoxFor(htmlHelper, expression, attributeList, labelAttributes);
        }


        /// <summary>
        /// Labels the CheckBox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="checkboxAttributes">The check box attributes.</param>
        /// <param name="labelAttributes">The label attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString LabelCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
                                                          Expression<Func<TModel, bool>> expression,
                                                          IDictionary<string, object> checkboxAttributes,
                                                          object labelAttributes = null)
        {

            var htmlString = htmlHelper.CheckBoxFor(expression, checkboxAttributes);

            var displayName = GetDisplayName(htmlHelper, expression);

            var lblAttributes = AttributesExtensions.ConvertAnonymousObjectToHtmlAttributes(labelAttributes);

            return MvcHtmlString.Create(string.Format("<{0} {3}>{1}{2}</{0}>",
                                                      "label", htmlString, displayName, lblAttributes));
        }


        /// <summary>
        /// Labels the RadioButton for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="groupHtmlAttributes">The group HTML attributes.</param>
        /// <param name="displayVertical">if set to <c>true</c> [display vertical].</param>
        /// <returns></returns>
        public static MvcHtmlString LabelRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                          Expression<Func<TModel, TProperty>> expression,
                                                                          object htmlAttributes = null,
                                                                          object groupHtmlAttributes = null,
                                                                          bool displayVertical = false)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            return LabelRadioButtonFor(htmlHelper, expression, attributeList, groupHtmlAttributes);
        }


        /// <summary>
        /// Labels the RadioButton for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="groupHtmlAttributes">The group HTML attributes.</param>
        /// <param name="displayVertical">if set to <c>true</c> [display vertical].</param>
        /// <returns></returns>
        public static MvcHtmlString LabelRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                          Expression<Func<TModel, TProperty>> expression,
                                                                          IDictionary<string, object> htmlAttributes,
                                                                          object groupHtmlAttributes = null,
                                                                          bool displayVertical = false )
        {
            if(htmlAttributes == null) 
                htmlAttributes = new Dictionary<string, object>();

            var selectListItems = GetSelectListItems(htmlHelper, expression).ToList();

            //select first if none of are selected.
            if (selectListItems.Any() && !selectListItems.Any(x => x.Selected))
            {
                selectListItems.First().Selected = true;
            }

            var radioButtons = selectListItems
                .Select(item =>
                    {
                        var attributes = new RouteValueDictionary(htmlAttributes);
                        if (item.Selected)
                        {
                            attributes.Add("checked", "checked");
                        }
                        return string.Format("<label>{0}{1}</label>",
                                             htmlHelper.RadioButtonFor(expression, item.Value, attributes)
                                             , item.Text);
                    }).ToList();


            var groupHtmlAttributeString = AttributesExtensions.ConvertAnonymousObjectToHtmlAttributes(groupHtmlAttributes);

            var htmlStringbuilder = new StringBuilder(1024);
            htmlStringbuilder.AppendFormat("<table {0}>", groupHtmlAttributeString);

            if (displayVertical)
            {
                foreach (var button in radioButtons)
                {
                    htmlStringbuilder.AppendFormat("<tr><td>{0}</td></tr>", button);
                }
            }
            else
            {
                htmlStringbuilder.Append("<tr>");
                foreach (var button in radioButtons)
                {
                    htmlStringbuilder.AppendFormat("<td>{0}</td>", button);
                }
                htmlStringbuilder.Append("</tr>");
            }

            htmlStringbuilder.Append("</table>");

            return MvcHtmlString.Create(htmlStringbuilder.ToString());
        }

        /// <summary>
        /// Dates the text box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString DateTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                   Expression<Func<TModel, TProperty>> expression,
                                                                   object htmlAttributes = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);          
            attributeList.AddIfNot("class", "date");
 
            //TODO; format should read form current culture or settings.
            return htmlHelper.TextBoxFor(expression, "{0:dd/MM/yyyy}", attributeList);
        }

        /// <summary>
        /// Actions the URL.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="url">The URL.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValue">The route value.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionUrl<TModel>(this HtmlHelper<TModel> htmlHelper, UrlHelper url, string actionName = null, string controllerName = null, object routeValue = null)
        {
             return MvcHtmlString.Create(url.Action(actionName, controllerName, routeValue));
        }

        /// <summary>
        /// Actions the button.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="url">The URL.</param>
        /// <param name="buttonText">The button text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValue">The route value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionButton<TModel>(this HtmlHelper<TModel> htmlHelper, UrlHelper url,
                                               string buttonText,string actionName = null, string controllerName = null,
                                               object routeValue = null, object htmlAttributes = null)
        {
            const string buttonHtml = "<button {1}>{0}</button>";

            var navigateUrl = url.Action(actionName, controllerName, routeValue);
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);
            attributeList.AddIfNot("onclick", string.Format("navigateTo('{0}')", navigateUrl));

            return MvcHtmlString.Create(string.Format(buttonHtml, buttonText, attributeList.ToHtmlAttributes()));
        }

        /// <summary>
        /// Displays the text value for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString DisplayTextValueFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                               Expression<Func<TModel, TValue>> expression)
        {
            var propertyInfo = GetPropertyInfo(expression);
            var attributes = propertyInfo.GetCustomAttributes(typeof(ServiceLookupAttribute)).ToList();
            if (attributes.Any() || propertyInfo.PropertyType.IsEnum)
            {
                var lookuplist = GetSelectListItems(html, expression);
                var selectedLookup = lookuplist.FirstOrDefault(x => x.Selected);
                var value = selectedLookup != null ? selectedLookup.Text : string.Empty;
                return MvcHtmlString.Create(value);
            }
            return html.DisplayFor(expression);
        }


        /// <summary>
        /// Display Or TextBoxFor based on flag
        /// </summary>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="canEdit"></param>
        /// <param name="htmlAttributes"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static MvcHtmlString DisplayOrTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            bool canEdit, object htmlAttributes = null)
        {
            return canEdit
                ? html.TextBoxFor(expression, htmlAttributes)
                : new MvcHtmlString(html.DisplayFor(expression, htmlAttributes).ToHtmlString() +
                                    html.HiddenFor(expression).ToHtmlString());
        }

        #region static methods

        internal static string GetDisplayName<TModel>(HtmlHelper<TModel> htmlHelper,
                                                      Expression<Func<TModel, bool>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return metadata.GetDisplayName();
        }

        /// <summary>
        /// Gets the select list items.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectListItems<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var propertyInfo = GetPropertyInfo(expression);

            var selectListItems = GetLookupItems(propertyInfo).ToList();

            var propertyValue = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            var intValue = 0;
            if (propertyInfo.PropertyType.IsEnum)
            {
                intValue = (int)propertyValue;
                //add if missing current value
                if (!selectListItems.Any(x => x.Key == propertyValue.ToString() || x.Key == intValue.ToString()))
                {
                    var enumItems = LookupProvider.GetEnumLookup(propertyInfo.PropertyType).ToList();
                    var currentItem = enumItems
                        .FirstOrDefault(x => x.Key == propertyValue.ToString() || x.Key == intValue.ToString());
                    if (currentItem != null)
                    {
                        selectListItems.Add(currentItem);
                    }
                }
            }

            return selectListItems
                .Select(x => new SelectListItem
                {
                    Text = x.Description,
                    Value = x.Key,
                    Selected = propertyValue != null && (x.Key == propertyValue.ToString() || x.Key == intValue.ToString()),
                });
        }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        internal static PropertyInfo GetPropertyInfo<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = (MemberExpression) expression.Body;
            var propertyName = memberExpression.Member is PropertyInfo ? memberExpression.Member.Name : null;
            var containerType = memberExpression.Expression.Type;
            var propertyInfo = containerType.GetProperty(propertyName);
            return propertyInfo;
        }

        /// <summary>
        /// Gets the look items.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns></returns>
        internal static IEnumerable<Lookup> GetLookupItems(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(ServiceLookupAttribute)).ToList();

            var selectListItems = new List<Lookup>();

            if (attributes.Any())
            {
                var serviceLookup = (ServiceLookupAttribute) attributes.First();
                var serviceObj = GetOrSetDependencyObject(serviceLookup.LookupType);
                var methodInfo = serviceLookup.LookupType.GetMethod(serviceLookup.MethodName);
                var returnObj = methodInfo.Invoke(serviceObj, null) as IEnumerable<Lookup>;

                selectListItems = returnObj.ToList();
            }
            else if (propertyInfo.PropertyType.IsEnum)
            {
                selectListItems = LookupProvider.GetEnumLookup(propertyInfo.PropertyType).ToList();
            }
            return selectListItems;
        }

        internal static string GetLookupText(PropertyInfo propertyInfo, object lookupValue)
        {
            var lookupItems = GetLookupItems(propertyInfo);

            int lookupIntValue = 0;
            if (propertyInfo.PropertyType.IsEnum)
            {
                lookupIntValue = (int)lookupValue;
            }
            var lookup = propertyInfo.PropertyType.IsEnum ?
                lookupItems.FirstOrDefault(x => x.Key == lookupValue.ToString() || x.Key == lookupIntValue.ToString()) :
                lookupItems.FirstOrDefault(x => x.Key == lookupValue.ToString());

            if (lookup != null)
            {
                return lookup.Description;
            }
            if (propertyInfo.PropertyType.IsEnum)
            {
                var lookupIem = LookupProvider.GetEnumLookup(propertyInfo.PropertyType)
                    .FirstOrDefault(x => x.Key == lookupValue.ToString() || x.Key == lookupIntValue.ToString());
                return lookupIem != null ? lookupIem.Description : "N/K";

            }
            return lookupValue.ToString();
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        internal static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            var propertyName = memberExpression.Member is PropertyInfo ? memberExpression.Member.Name : null;
            return propertyName;
        }

        internal const string GetOrSetDependencyObjectString = "GetOrSetDependencyObject";

        /// <summary>
        /// Gets the or set dependency object in the current http context.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static object GetOrSetDependencyObject(Type type)
        {
            ConcurrentDictionary<Type, object> doDictionary;
            if (!HttpContext.Current.Items.Contains(GetOrSetDependencyObjectString))
            {
                doDictionary = new ConcurrentDictionary<Type, object>();
                HttpContext.Current.Items[GetOrSetDependencyObjectString] = doDictionary;
            }
            else
            {
                doDictionary = (ConcurrentDictionary<Type, object>)HttpContext.Current.Items[GetOrSetDependencyObjectString];
            }

            return doDictionary.GetOrAdd(type, x => DependencyResolver.Current.GetService(x));
        }

        /// <summary>
        /// Gets the name of the model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <returns></returns>
        public static string GetModelName<TModel>(this HtmlHelper<TModel> helper)
        {
            var modelName = helper.ViewData.Model.GetType().Name;
            var prefix = helper.ViewData.TemplateInfo.HtmlFieldPrefix;
            return string.IsNullOrEmpty(prefix) ? modelName : string.Format("{0}.{1}", modelName, prefix);
        }

        #endregion
    }
}