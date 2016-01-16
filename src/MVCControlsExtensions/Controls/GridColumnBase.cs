using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MVCControls.Extensions;

namespace MVCControls.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class GridColumnBase<TModel> : GridColumn
    {
        /// <summary>
        /// Gets or sets the HTML helper.
        /// </summary>
        /// <value>
        /// The HTML helper.
        /// </value>
        public HtmlHelper<TModel> HtmlHelper { get; set; }

        /// <summary>
        /// Gets or sets the view data.
        /// </summary>
        /// <value>
        /// The view data.
        /// </value>
        public ViewDataDictionary<TModel> ViewData { get; set; }

        /// <summary>
        /// HtmlAttributes Factory
        /// </summary>
        public Func<TModel, object> HtmlAttributesFactory { get; set; }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public TModel Model { get { return ViewData.Model; } }

        /// <summary>
        /// Get HTML Attributes
        /// </summary>
        /// <returns></returns>
        protected IDictionary<string, object> GetHtmlAttributes()
        {
            if (HtmlAttributesFactory == null) return HtmlAttributes;
            var htmlAttributes = HtmlAttributesFactory(Model);
            return htmlAttributes != null
                ? AttributesExtensions.GetAttributeList(htmlAttributes)
                : null;
        }
    }
}