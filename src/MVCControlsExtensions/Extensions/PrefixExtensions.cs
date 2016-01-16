using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MVCControls.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class PrefixExtensions
    {
        #region prefix controls

        /// <summary>
        /// Validations the message for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            string prefix,
            Expression<Func<TModel, TProperty>>
                expression)
        {
            return ValidationMessageFor(htmlHelper, prefix, expression, null, new RouteValueDictionary());
        }

        /// <summary>
        /// Validations the message for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="validationMessage">The validation message.</param>
        /// <returns></returns>
        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            string prefix,
            Expression<Func<TModel, TProperty>>
                expression, string validationMessage)
        {
            return ValidationMessageFor(htmlHelper, prefix, expression, validationMessage, new RouteValueDictionary());
        }

        /// <summary>
        /// Validations the message for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="validationMessage">The validation message.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            string prefix,
            Expression<Func<TModel, TProperty>>
                expression, string validationMessage,
            object htmlAttributes)
        {
            return ValidationMessageFor(htmlHelper, prefix, expression, validationMessage,
                new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Validations the message for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="validationMessage">The validation message.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            string prefix,
            Expression<Func<TModel, TProperty>>
                expression, string validationMessage,
            IDictionary<string, object> htmlAttributes)
        {
            return
                htmlHelper.ValidationMessage(
                    String.Format("{0}.{1}", prefix, ExpressionHelper.GetExpressionText(expression)),
                    validationMessage,
                    htmlAttributes);
        }

        /// <summary>
        /// Hiddens for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression)
        {
            return HiddenFor(htmlHelper, prefix, expression, null);
        }

        /// <summary>
        /// Hiddens for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            return HiddenFor(htmlHelper, prefix, expression, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Hiddens for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression,
            IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Hidden(String.Format("{0}.{1}", prefix, ExpressionHelper.GetExpressionText(expression)),
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model,
                htmlAttributes);
        }

        /// <summary>
        /// Texts the area for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression)
        {
            return TextAreaFor(htmlHelper, prefix, expression, null);
        }

        /// <summary>
        /// Texts the area for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            return TextAreaFor(htmlHelper, prefix, expression, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Texts the area for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static MvcHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression,
            IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string value;
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (modelMetadata.Model != null)
                value = modelMetadata.Model.ToString();
            else
                value = String.Empty;

            return htmlHelper.TextArea(String.Format("{0}.{1}", prefix, ExpressionHelper.GetExpressionText(expression)),
                value,
                htmlAttributes);
        }

        /// <summary>
        /// Texts the box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression)
        {
            return TextBoxFor(htmlHelper, prefix, expression, null);
        }

        /// <summary>
        /// Texts the box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            return TextBoxFor(htmlHelper, prefix, expression, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Texts the box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, TProperty>> expression,
            IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.TextBox(String.Format("{0}.{1}", prefix, ExpressionHelper.GetExpressionText(expression)),
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model,
                htmlAttributes);
        }

        /// <summary>
        /// CheckBoxes for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, string prefix,
            Expression<Func<TModel, bool>> expression,
            IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.CheckBox(String.Format("{0}.{1}", prefix, ExpressionHelper.GetExpressionText(expression)),
                (bool) ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model,
                htmlAttributes);
        }

        #endregion

        #region grid prifix controls

        private static string GetGridControlPrifix(string prefix, string uniqueId)
        {
            return String.Format("{0}[{1}]", prefix, uniqueId);
        }

        private static string GetGridControlName(string prefix, string uniqueId, string name)
        {
            //take only last name after "."
            name = name.Split('.').Last();

            return String.Format("{0}[{1}].{2}", prefix, uniqueId, name);
        }

        /// <summary>
        /// Grids the index.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="uniqueId">The unique identifier.</param>
        /// <returns></returns>
        public static MvcHtmlString GridIndex<TModel>(this HtmlHelper<TModel> htmlHelper,
            string prefix, string uniqueId)
        {
            return
                MvcHtmlString.Create(string.Format("<input type=\"hidden\" name=\"{0}.Index\" value=\"{1}\" />", prefix,
                    uniqueId));
        }

        /// <summary>
        /// Grids the CheckBox for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="uniqueId">The unique identifier.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString GridCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression,
            string prefix, string uniqueId,
            IDictionary<string, object> htmlAttributes = null)
        {
            return htmlHelper.CheckBox(
                GetGridControlName(prefix, uniqueId, ExpressionHelper.GetExpressionText(expression)),
                (bool) ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model,
                htmlAttributes);
        }

        /// <summary>
        /// Grids the hidden for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="uniqueId">The unique identifier.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString GridHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string prefix, string uniqueId,
            IDictionary<string, object> htmlAttributes = null)
        {
            return
                htmlHelper.Hidden(GetGridControlName(prefix, uniqueId, ExpressionHelper.GetExpressionText(expression)),
                    ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model,
                    htmlAttributes);
        }

        #endregion
    }
}