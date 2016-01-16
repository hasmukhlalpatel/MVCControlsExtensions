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

namespace MVCControls.Extensions.Ko
{
    /// <summary>
    /// Knockout extensions.
    /// </summary>
    public static class KOControlExtensions
    {
        static KOControlExtensions()
        {
            SetKOBindingWithModelNameFromConfig();
        }

        private const string KODivMarkup = "<div {1}>{0}</div>";

        /// <summary>
        /// Knockout the partial for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <returns></returns>
        public static MvcHtmlString KOPartialFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                    string partialViewName)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;

            var oldModelName = RenderingViewModel;

            RenderingViewModel = string.IsNullOrEmpty(oldModelName)
                                     ? name
                                     : string.Format("{0}.{1}", oldModelName, name);

            var mvcHtmlString = helper.Partial(partialViewName, model, name);

            RenderingViewModel = oldModelName;

            return MvcHtmlString.Create(string.Format(KODivMarkup, mvcHtmlString, helper.KOWithBindingFor(expression)));
        }

        private const string ConstKOWithBinding = "data-bind=\"with: {0}\"";

        /// <summary>
        /// Knockout with binding for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <returns></returns>
        public static MvcHtmlString KOWithBindingFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                                                                        Expression<Func<TModel, TProperty>> expression,
                                                                        bool bindingWithModelName = true)
        {
            var bindingPath = string.Format(ConstKOWithBinding, GetDataBindingPath(helper, expression, bindingWithModelName));
            return MvcHtmlString.Create(bindingPath);
        }

        private const string ConstKOForeEachBinding = "data-bind=\"foreach: {0}\"";

        /// <summary>
        /// Knockout fore each binding for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <returns></returns>
        public static MvcHtmlString KOForeEachBindingFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                                                                            Expression<Func<TModel, TProperty>>
                                                                                expression,
                                                                            bool bindingWithModelName = true)
        {
            var bindingPath = string.Format(ConstKOForeEachBinding, GetDataBindingPath(helper, expression, bindingWithModelName));
            return MvcHtmlString.Create(bindingPath);
        }

        /// <summary>
        /// Knockout DropDownList for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectListItems">The select list items.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static MvcHtmlString KODropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                          Expression<Func<TModel, TProperty>> expression,
                                                                          IEnumerable<SelectListItem> selectListItems = null,
                                                                          object htmlAttributes = null,
                                                                          string binding = "value",
                                                                          bool bindingWithModelName = true,
                                                                          KOBindingOptions options = null)
        {
            selectListItems = selectListItems ?? ControlExtensions.GetSelectListItems(htmlHelper, expression).ToList();

            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options:options);

            return htmlHelper.DropDownListFor(expression, selectListItems, attributeList);
        }

        /// <summary>
        /// Knockout textbox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KOTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                    object htmlAttributes = null,
                                                                    string binding = "value",
                                                                    bool bindingWithModelName = true,
                                                                    KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options: options);

            return htmlHelper.TextBoxFor(expression, attributeList);
        }

        /// <summary>
        /// Knockout password textbox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KOPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                    object htmlAttributes = null,
                                                                    string binding = "value",
                                                                    bool bindingWithModelName = true,
                                                                    KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options: options);

            return htmlHelper.PasswordFor(expression, attributeList);
        }

        /// <summary>
        /// Knockout TextArea for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KOTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                     Expression<Func<TModel, TProperty>> expression,
                                                                     int rows = 2, int columns = 20,
                                                                     object htmlAttributes = null,
                                                                     string binding = "value",
                                                                     bool bindingWithModelName = true,
                                                                     KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);
            attributeList.Add("rows", rows);
            attributeList.Add("cols", columns);
            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options: options);

            return htmlHelper.TextAreaFor(expression, attributeList);
        }

        /// <summary>
        /// Knockout date TextBox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KODateTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                        Expression<Func<TModel, TProperty>> expression,
                                                                        object htmlAttributes = null,
                                                                        string binding = "value",
                                                                        bool bindingWithModelName = true,
                                                                        KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options: options);

            return htmlHelper.DateTextBoxFor(expression, htmlAttributes);
        }

        /// <summary>
        /// Knockout CheckBox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="checkboxAttributes">The checkbox attributes.</param>
        /// <param name="labelAttributes">The label attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KOCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
                                                          Expression<Func<TModel, bool>> expression,
                                                          object checkboxAttributes = null,
                                                          object labelAttributes = null,
                                                          string binding = "checked",
                                                          bool bindingWithModelName = true,
                                                          KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(checkboxAttributes);

            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options: options);
            return htmlHelper.LabelCheckBoxFor(expression, attributeList, labelAttributes);
        }

        /// <summary>
        /// Knockout RadioButton for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="groupHtmlAttributes">The group HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="uniqueName">if set to <c>true</c> [unique name].</param>
        /// <param name="displayVertical">if set to <c>true</c> [display vertical].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KORadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                        Expression<Func<TModel, TProperty>> expression,
                                                                        object htmlAttributes = null,
                                                                        object groupHtmlAttributes = null,
                                                                        string binding = "checked",
                                                                        bool bindingWithModelName = true,
                                                                        bool uniqueName = false,
                                                                        bool displayVertical = false,
                                                                        KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, uniqueName, options: options);

            return htmlHelper.LabelRadioButtonFor(expression, attributeList, groupHtmlAttributes, displayVertical);
        }

        /// <summary>
        /// Knockout hidden field for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KOHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                   Expression<Func<TModel, TProperty>> expression,
                                                                   string binding = "value",
                                                                   bool bindingWithModelName = true,
                                                                   KOBindingOptions options = null)
        {
            var attributeList = new RouteValueDictionary();
            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options: options);

            return InputExtensions.HiddenFor(htmlHelper, expression, attributeList);
        }

        /// <summary>
        /// Knockout display text for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KODisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                    object htmlAttributes = null,
                                                                    string binding = "text",
                                                                    bool bindingWithModelName = true,
                                                                    KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options: options);

            var value = htmlHelper.DisplayFor(expression, attributeList);

            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(attributeList);

            return MvcHtmlString.Create(string.Format("<span {1}>{0}</span>", value, attributes));
        }

        /// <summary>
        /// Knockout display label and text value for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="options">KOBindingOptions</param>
        /// <returns></returns>
        public static MvcHtmlString KODisplayLabelValueFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                              Expression<Func<TModel, TProperty>> expression,
                                                                              object htmlAttributes = null,
                                                                              string binding = "text",
                                                                              bool bindingWithModelName = true,
                                                                              KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, binding, bindingWithModelName, attributeList, options: options);

            var display = htmlHelper.DisplayNameFor(expression);
            var value = htmlHelper.DisplayFor(expression, attributeList);

            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(attributeList);

            return MvcHtmlString.Create(string.Format("{0} : <span {2}>{1}</span>", display, value, attributes));
        }


        /// <summary>
        /// Sets the name of the ko binding with model value from config.
        /// </summary>
        public static void SetKOBindingWithModelNameFromConfig()
        {
            bool result;
            bool.TryParse(ConfigurationManager.AppSettings[KOBindingWithModelNameString], out result);

            HttpContext.Current.Cache[KOBindingWithModelNameString] = result;
        }

        /// <summary>
        /// Sets the name of the ko binding with model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="withModelName">if set to <c>true</c> [with model name].</param>
        public static void SetKOBindingWithModelName<TModel>(this HtmlHelper<TModel> htmlHelper, bool withModelName)
        {
            HttpContext.Current.Items[KOBindingWithModelNameString] = withModelName;
        }

        private const string KOBindingWithModelNameString = "KOBindingWithModelName";

        /// <summary>
        /// Gets a value indicating whether [Knockout binding with model name] from the current HttpContext.
        /// </summary>
        /// <value>
        /// <c>true</c> if [ko binding with model name]; otherwise, <c>false</c>.
        /// </value>
        public static bool KOBindingWithModelName
        {
            get
            {
                var httpContext = HttpContext.Current;
                if (!httpContext.Items.Contains(KOBindingWithModelNameString))
                {
                    if (HttpContext.Current.Cache[KOBindingWithModelNameString] == null)
                    {
                        SetKOBindingWithModelNameFromConfig();
                    }

                    return (bool)HttpContext.Current.Cache[KOBindingWithModelNameString];
                }
                return (bool)httpContext.Items[KOBindingWithModelNameString];
            }
        }

        /// <summary>
        /// Sets the ko binding options.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static MvcHtmlString SetKOBindingOptions(this HtmlHelper htmlHelper, KOBindingOptions options)
        {
            return options != null
                       ? MvcHtmlString.Create(options.ToBindingString())
                       : MvcHtmlString.Empty;
        }



        internal static void AddDataBinding(string propertyName,
                                            string binding,
                                            IDictionary<string, object> attributeList,
                                            bool uniqueName = false,
                                            KOBindingOptions options = null)
        {
            if (options != null)
            {
                attributeList.Add(KOBindingOptions.KODataBind, options.ToParams());
            }

            if (attributeList.ContainsKey(KOBindingOptions.KODataBind))
            {
                var existingKoBinding = attributeList[KOBindingOptions.KODataBind];
                propertyName = string.Format("{0},{1}", propertyName, existingKoBinding);
                attributeList.Remove(KOBindingOptions.KODataBind);
            }

            attributeList.Add(KOBindingOptions.KODataBind,
                              uniqueName
                                  ? string.Format("{0}: {1}, uniqueName: true", binding, propertyName)
                                  : string.Format("{0}: {1}", binding, propertyName));
        }

        /// <summary>
        /// Adds the data binding.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="uniqueName">if set to <c>true</c> [unique name].</param>
        /// <param name="options">The options.</param>
        private static IDictionary<string, object> UpdateDataBinding<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
                                                              Expression<Func<TModel, TProperty>> expression,
                                                              string binding,
                                                              bool bindingWithModelName,
                                                              object htmlAttributes =null,
                                                              bool uniqueName = false,
                                                              KOBindingOptions options = null)
        {
            var attributeList = AttributesExtensions.GetAttributeList(htmlAttributes);

            AddDataBinding(htmlHelper, expression, binding,bindingWithModelName, attributeList, uniqueName, options);
            return attributeList;
        }

        /// <summary>
        /// Adds the data binding.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <param name="attributeList">The attribute list.</param>
        /// <param name="uniqueName">if set to <c>true</c> [unique name].</param>
        /// <param name="options">The options.</param>
        private static void AddDataBinding<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
                                                              Expression<Func<TModel, TProperty>> expression,
                                                              string binding,
                                                              bool bindingWithModelName,
                                                              IDictionary<string, object> attributeList,
                                                              bool uniqueName = false,
                                                              KOBindingOptions options = null)
        {
            var propertyName = GetDataBindingPath(htmlHelper, expression, bindingWithModelName);

            AddDataBinding(propertyName, binding, attributeList, uniqueName, options);
        }

        /// <summary>
        /// Gets the data binding path.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="bindingWithModelName">if set to <c>true</c> [binding with model name].</param>
        /// <returns></returns>
        private static string GetDataBindingPath<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                    bool bindingWithModelName)
        {
            var propertyName = ControlExtensions.GetPropertyName(expression);

            if (bindingWithModelName && KOBindingWithModelName)
            {
                var modelName = htmlHelper.GetKOModelName();
                return string.Format("{0}.{1}", modelName, propertyName);
            }
            return propertyName;
        }

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
        
        /// <summary>
        /// Gets the name of the ko model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <returns></returns>
        public static string GetKOModelName<TModel>(this HtmlHelper<TModel> helper)
        {
            if (string.IsNullOrEmpty(RenderingViewModel))
            {
                RenderingViewModel = KOViewModelBuilder.ViewModelNameFactory(helper.ViewData.Model.GetType());
            }

            return RenderingViewModel;
        }
    }

    /// <summary>
    /// Knockout binding options.
    /// </summary>
    public class KOBindingOptions
    {
        /// <summary>
        /// The ko data bind
        /// </summary>
        public const string KODataBind = "data-bind";

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the checked.
        /// </summary>
        /// <value>
        /// The checked.
        /// </value>
        public string Checked { get; set; }
        /// <summary>
        /// Gets or sets the date string.
        /// </summary>
        /// <value>
        /// The date string.
        /// </value>
        public string DateString { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the CSS.
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        public string Css { get; set; }

        /// <summary>
        /// Gets or sets the click.
        /// </summary>
        /// <value>
        /// The click.
        /// </value>
        public string Click { get; set; }
        /// <summary>
        /// Gets or sets the visible.
        /// </summary>
        /// <value>
        /// The visible.
        /// </value>
        public string Visible { get; set; }
        /// <summary>
        /// Gets or sets the enable.
        /// </summary>
        /// <value>
        /// The enable.
        /// </value>
        public string Enable { get; set; }
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public string Options { get; set; }
        /// <summary>
        /// Gets or sets the options text.
        /// </summary>
        /// <value>
        /// The options text.
        /// </value>
        public string OptionsText { get; set; }
        /// <summary>
        /// Gets or sets the options value.
        /// </summary>
        /// <value>
        /// The options value.
        /// </value>
        public string OptionsValue { get; set; }

        /// <summary>
        /// Gets or sets the DisableClick.
        /// </summary>
        /// <value>
        /// The DisableClick value.
        /// </value>
        public string DisableClick { get; set; }

        /// <summary>
        /// Gets or sets the ExtraParameters value.
        /// </summary>
        /// <value>
        /// The ExtraParameters value.
        /// </value>
        public object ExtraParameters { get; set; }


        private const string KOBindingString = "data-bind=\"{0}\"";

        /// <summary>
        /// To the binding string.
        /// </summary>
        /// <returns></returns>
        public string ToBindingString()
        {
            return string.Format(KOBindingString, ToParams());
        }

        /// <summary>
        /// Converts the parameters to Html markup attributes.
        /// </summary>
        /// <returns></returns>
        public string ToParams()
        {
            var sb = new StringBuilder(128);
            if (!string.IsNullOrEmpty(Value)) sb.AppendFormat("value:{0},", Value);
            if (!string.IsNullOrEmpty(Checked)) sb.AppendFormat("checked:{0}, ", Checked);
            if (!string.IsNullOrEmpty(DateString)) sb.AppendFormat("dateString:{0}, ", DateString);
            if (!string.IsNullOrEmpty(Text)) sb.AppendFormat("text:{0}, ", Text);
            if (!string.IsNullOrEmpty(Css)) sb.AppendFormat("css:{0}, ", Css);
            if (!string.IsNullOrEmpty(Click)) sb.AppendFormat("click:{0}, ", Click);
            if (!string.IsNullOrEmpty(Visible)) sb.AppendFormat("visible:{0}, ", Visible);
            if (!string.IsNullOrEmpty(Enable)) sb.AppendFormat("enable:{0}, ", Enable);
            if (!string.IsNullOrEmpty(Options)) sb.AppendFormat("options:{0}, ", Options);
            if (!string.IsNullOrEmpty(OptionsText)) sb.AppendFormat("optionsText:{0}, ", OptionsText);
            if (!string.IsNullOrEmpty(OptionsValue)) sb.AppendFormat("optionsValue:{0}, ", OptionsValue);
            if (!string.IsNullOrEmpty(DisableClick)) sb.AppendFormat("disableClick:{0}, ", DisableClick);

            //extract  extra parameters from anonymous object
            if (ExtraParameters != null)
            {
                var parameters = AttributesExtensions.GetAttributeList(ExtraParameters);
                foreach (var param in parameters)
                {
                    sb.AppendFormat("{0}:{1}, ", param.Key, param.Value);
                }
            }

            //remove last ', '
            if (sb.Length > 2)
            {
                sb.Remove(sb.Length - 2, 2);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToBindingString();
        }
    }
}