using System.Collections.Generic;
using System.Web.Mvc;
using MVCControls.Extensions;

namespace MVCControls.Controls
{
    /// <summary>
    /// Grid
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class Grid<TModel> : GridBase<TModel>
    {
        /// <summary>
        /// Adds the delete button column.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public GridColumn AddDeleteButtonColumn(string displayText, string headerText = null,
            object htmlAttributes = null)
        {
            var column = AddButtonColumn(displayText, headerText, htmlAttributes, "gridDeleteButton smallGridButton");

            return column;
        }

        /// <summary>
        /// Adds the delete button column.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="headerAsMarkup">if set to <c>true</c> [header as markup].</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="headerHtmlAttributes">The header HTML attributes.</param>
        /// <returns></returns>
        public GridColumn AddDeleteButtonColumn(string displayText, bool headerAsMarkup, string headerText,
            object htmlAttributes = null, object headerHtmlAttributes = null)
        {
            if (headerAsMarkup)
            {
                headerText = GenerateAddButtonMarkup(headerText, headerHtmlAttributes);
            }
            var column = AddButtonColumn(displayText, headerText, htmlAttributes, "gridDeleteButton smallGridButton");

            return column;
        }

        /// <summary>
        /// Adds the edit button column.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public GridColumn AddEditButtonColumn(string displayText, string headerText = null,
            object htmlAttributes = null)
        {
            var column = AddButtonColumn(displayText, headerText, htmlAttributes, "gridEditButton smallGridButton");

            return column;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid{TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyName">Name of the property.</param>
        public Grid(IEnumerable<TModel> model, HtmlHelper<TModel> htmlHelper, string propertyName)
            : base(model, htmlHelper, propertyName)
        {
        }

        /// <summary>
        /// Adds the edit button column.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="headerAsMarkup">if set to <c>true</c> [header as markup].</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="headerHtmlAttributes">The header HTML attributes.</param>
        /// <returns></returns>
        public GridColumn AddEditButtonColumn(string displayText, bool headerAsMarkup, string headerText,
            object htmlAttributes = null, object headerHtmlAttributes = null)
        {
            if (headerAsMarkup)
            {
                headerText = GenerateAddButtonMarkup(headerText, headerHtmlAttributes);
            }
            var column = AddButtonColumn(displayText, headerText, htmlAttributes, "gridEditButton smallGridButton");

            return column;
        }

        private static string GenerateAddButtonMarkup(string headerText, object htmlAttributes)
        {
            var htmlAttributeList = AttributesExtensions.GetAttributeList(htmlAttributes);
            htmlAttributeList.AddIfNot("class", "addButton");
            htmlAttributeList.AddIfNot("href", "#");
            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(htmlAttributeList);
            return string.Format("<a {1}>{0}</a>", headerText, attributes);
        }

    }

}