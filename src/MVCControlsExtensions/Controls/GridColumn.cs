using System.Collections.Generic;
using System.Linq;
using MVCControls.Extensions;

namespace MVCControls.Controls
{
    /// <summary>
    /// Grid basic column
    /// </summary>
    public class GridColumn
    {
        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>
        /// The header text.
        /// </value>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the data text.
        /// </summary>
        /// <value>
        /// The data text.
        /// </value>
        public string DataText { get; set; }

        /// <summary>
        /// Gets or sets the HTML attributes.
        /// </summary>
        /// <value>
        /// The HTML attributes.
        /// </value>
        public IDictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets the cell HTML attributes.
        /// </summary>
        /// <value>
        /// The cell HTML attributes.
        /// </value>
        public IDictionary<string, object> CellHtmlAttributes { get; set; }

        /// <summary>
        /// Is ReadOnly
        /// </summary>
        public virtual bool IsReadOnly { get { return true; } }

        /// <summary>
        /// Is Hidden
        /// </summary>
        public virtual bool IsHidden { get { return false; } }

        /// <summary>
        /// Renders the column data.
        /// </summary>
        /// <returns></returns>
        public virtual string Render()
        {
            if (HtmlAttributes != null && HtmlAttributes.Any())
            {
                var attributes = AttributesExtensions.ConvertDictionaryToAttributes(HtmlAttributes);
                return string.Format("<span {1}>{0}</span>", DataText, attributes);
            }
            return DataText;
        }

        /// <summary>
        /// Renders the header text.
        /// </summary>
        /// <returns></returns>
        public virtual string RenderHeader()
        {
            return HeaderText;
        }
    }
}