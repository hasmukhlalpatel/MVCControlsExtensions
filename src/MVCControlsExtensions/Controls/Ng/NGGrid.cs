using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCControls.Controls.Ng
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class NgGrid<TModel> : Grid<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NgGrid{TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyName">Name of the property.</param>
        public NgGrid(IEnumerable<TModel> model, HtmlHelper<TModel> htmlHelper, string propertyName) : base(model, htmlHelper, propertyName)
        {
        }
    }
}
