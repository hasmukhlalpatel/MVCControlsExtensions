using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MVCControls.Extensions.Ng
{
    /// <summary>
    /// 
    /// </summary>
    public static class NgControlExtensions
    {
        private const string KeyNgApp = "data-ng-app";
        private const string KeyNgController = "data-ng-controller";
        private const string KeyNgModel = "data-ng-model";
        private const string KeyNgBind = "data-ng-bind";
        private const string KeyNgRepeat = "data-ng-repeat";
        private const string KeyNgDisabled = "data-ng-disabled";
        private const string KeyNgShow = "data-ng-show";
        private const string KeyNgHide = "data-ng-hide";
        private const string KeyNgClick = "data-ng-click";

        static NgControlExtensions()
        {
            SetNgBindingWithModelNameFromConfig();
        }

        /// <summary>
        /// The Ng display for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="tagName">html Tag Name</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null, NgFilters filters = null, 
            string tagName = "span")
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            var value = htmlHelper.DisplayFor(expression, attributeList);

            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(attributeList);

            AddDataBinding(htmlHelper, expression, KeyNgBind, true, attributeList, filters);

            return MvcHtmlString.Create(string.Format("<{0} {1}>{2}</{0}>", tagName, attributes, value));
        }

        /// <summary>
        /// ng textbox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null,
            bool bindingWithModelName = true,
            NgFilters filters = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, KeyNgModel, bindingWithModelName, attributeList, filters);

            return htmlHelper.TextBoxFor(expression, attributeList);
        }

        /// <summary>
        /// ng password textbox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                    object htmlAttributes = null,
                                                                    bool bindingWithModelName = true,
            NgFilters filters = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, KeyNgModel, bindingWithModelName, attributeList, filters);

            return htmlHelper.PasswordFor(expression, attributeList);
        }

        /// <summary>
        /// ng TextArea for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            int rows = 2, int columns = 20,
            object htmlAttributes = null,
            bool bindingWithModelName = true,
            NgFilters filters = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);
            attributeList.Add("rows", rows);
            attributeList.Add("cols", columns);
            AddDataBinding(htmlHelper, expression, KeyNgModel, bindingWithModelName, attributeList, filters);

            return htmlHelper.TextAreaFor(expression, attributeList);
        }

        /// <summary>
        /// ng date TextBox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgDateTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null,
            bool bindingWithModelName = true,
            NgFilters filters = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, KeyNgModel, bindingWithModelName, attributeList, filters);

            return htmlHelper.DateTextBoxFor(expression, htmlAttributes);
        }

        /// <summary>
        /// ng CheckBox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="checkboxAttributes">The checkbox attributes.</param>
        /// <param name="labelAttributes">The label attributes.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression,
            object checkboxAttributes = null,
            object labelAttributes = null,
            bool bindingWithModelName = true,
            NgFilters filters = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(checkboxAttributes);

            AddDataBinding(htmlHelper, expression, KeyNgModel, bindingWithModelName, attributeList, filters);
            return htmlHelper.LabelCheckBoxFor(expression, attributeList, labelAttributes);
        }

        /// <summary>
        /// ng RadioButton for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="groupHtmlAttributes">The group HTML attributes.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="displayVertical">if set to <c>true</c> [display vertical].</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null,
            object groupHtmlAttributes = null,
            bool bindingWithModelName = true,
            bool displayVertical = false,
            NgFilters filters = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, KeyNgModel, bindingWithModelName, attributeList, filters);

            return htmlHelper.LabelRadioButtonFor(expression, attributeList, groupHtmlAttributes, displayVertical);
        }


        /// <summary>
        /// Ng DropDownList for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectListItems">The select list items.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                          Expression<Func<TModel, TProperty>> expression,
                                                                          IEnumerable<SelectListItem> selectListItems = null,
                                                                          object htmlAttributes = null,
                                                                          bool bindingWithModelName = true,
                                                                          NgFilters filters = null)
        {
            selectListItems = selectListItems ?? ControlExtensions.GetSelectListItems(htmlHelper, expression).ToList();

            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, KeyNgModel, bindingWithModelName, attributeList, filters);

            return htmlHelper.DropDownListFor(expression, selectListItems, attributeList);
        }


        /// <summary>
        /// Ng hidden field for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="filters">ng filters</param>
        /// <returns></returns>
        public static MvcHtmlString NgHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                   Expression<Func<TModel, TProperty>> expression,
                                                                   bool bindingWithModelName = true,
                                                                   NgFilters filters = null)
        {
            var attributeList = new RouteValueDictionary();
            AddDataBinding(htmlHelper, expression, KeyNgModel, bindingWithModelName, attributeList, filters);

            return InputExtensions.HiddenFor(htmlHelper, expression, attributeList);
        }


        /// <summary>
        /// Sets the name of the ko binding with model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="withModelName">if set to <c>true</c> [with model name].</param>
        public static void SetNgBindingWithModelName<TModel>(this HtmlHelper<TModel> htmlHelper, bool withModelName)
        {
            HttpContext.Current.Items[NgBindingWithModelNameString] = withModelName;
        }

        /// <summary>
        /// Sets the name of the ko binding with model value from config.
        /// </summary>
        public static void SetNgBindingWithModelNameFromConfig()
        {
            bool result;
            bool.TryParse(ConfigurationManager.AppSettings[NgBindingWithModelNameString], out result);

            HttpContext.Current.Cache[NgBindingWithModelNameString] = result;
        }

        private const string NgBindingWithModelNameString = "NgBindingWithModelName";

        /// <summary>
        /// Gets a value indicating whether [Knockout binding with model name] from the current HttpContext.
        /// </summary>
        /// <value>
        /// <c>true</c> if [ko binding with model name]; otherwise, <c>false</c>.
        /// </value>
        public static bool NgBindingWithModelName
        {
            get
            {
                var httpContext = HttpContext.Current;
                if (!httpContext.Items.Contains(NgBindingWithModelNameString))
                {
                    if (HttpContext.Current.Cache[NgBindingWithModelNameString] == null)
                    {
                        SetNgBindingWithModelNameFromConfig();
                    }

                    return  (bool)HttpContext.Current.Cache[NgBindingWithModelNameString];
                }
                return (bool)httpContext.Items[NgBindingWithModelNameString];
            }
        }

        #region private /internal methods

        internal static void AddDataBinding(string propertyName,
            string directive,
            IDictionary<string, object> attributeList,
            NgFilters filters = null)
        {

            if (attributeList.ContainsKey(directive))
            {
                var existingKoBinding = attributeList[directive];
                propertyName = string.Format("{0}|{1}", propertyName, existingKoBinding);
                attributeList.Remove(directive);
            }
            if (filters != null)
            {
                propertyName += filters.ToParams();
            }

            attributeList.Add(directive, propertyName);
        }
        

        private static void AddDataBinding<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
                                                      Expression<Func<TModel, TProperty>> expression,
                                                      string directive,
                                                      bool bindingWithModelName,
                                                      IDictionary<string, object> attributeList,
                                                      NgFilters filters = null)
        {
            var propertyName = GetDataBindingPath(htmlHelper, expression, bindingWithModelName);

            AddDataBinding(propertyName, directive, attributeList, filters);
        }

        private static string GetDataBindingPath<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
                                                            Expression<Func<TModel, TProperty>> expression,
                                                            bool bindingWithModelName)
        {
            var propertyName = ControlExtensions.GetPropertyName(expression);

            if (bindingWithModelName && NgBindingWithModelName)
            {
                var modelName = htmlHelper.GetModelName();
                return string.Format("{0}.{1}", modelName, propertyName);
            }
            return propertyName;
        }


        #endregion

        private const string RenderingViewModelString = "RenderingViewModel";

        /// <summary>
        /// Gets or sets the rendering view model.
        /// </summary>
        /// <value>
        /// The rendering view model.
        /// </value>
        public static string RenderingViewModel
        {
            get
            {
                var httpContext = HttpContext.Current;
                if (httpContext.Items.Contains(RenderingViewModelString))
                {
                    return httpContext.Items[RenderingViewModelString] as string;
                }
                return string.Empty;
            }
            set
            {
                var httpContext = HttpContext.Current;
                if (!httpContext.Items.Contains(RenderingViewModelString))
                {
                    httpContext.Items.Add(RenderingViewModelString, value);
                }
                else
                {
                    httpContext.Items[RenderingViewModelString] = value;
                }
            }
        }

        public static string GetModelName<TModel>(this HtmlHelper<TModel> helper)
        {
            if (string.IsNullOrEmpty(RenderingViewModel))
            {
                RenderingViewModel = KOViewModelBuilder.ViewModelNameFactory(helper.ViewData.Model.GetType());
            }

            return RenderingViewModel;
        }
    }

    public class NgFilters
    {
        public bool DisplayCurrency { get; set; }
        public bool DisplayLowercase { get; set; }
        public bool DisplayUppercase { get; set; }
        public string OrderBy { get; set; }
        public string Filter { get; set; }

        public string ToParams()
        {
            var sb = new StringBuilder(128);
            if (DisplayCurrency) sb.Append("| currency");
            if (DisplayLowercase) sb.Append("| lowercase");
            if (DisplayUppercase) sb.Append("| uppercase");
            if (!string.IsNullOrEmpty(OrderBy)) sb.AppendFormat("| orderBy:'{0}'", OrderBy);
            if (!string.IsNullOrEmpty(Filter)) sb.AppendFormat("| filter:{0}", Filter);

            return sb.ToString();
        }
    }
}
