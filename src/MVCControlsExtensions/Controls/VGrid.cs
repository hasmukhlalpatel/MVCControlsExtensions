using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVCControls.Controls
{
    /// <summary>
    /// Vertical Grid
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class VGrid<TModel> : GridBase<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VGrid{TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyName">Name of the property.</param>
        public VGrid(IEnumerable<TModel> model, HtmlHelper<TModel> htmlHelper, string propertyName)
            : base(model, htmlHelper, propertyName)
        {
        }

        /// <summary>
        /// Gets the body data.
        /// </summary>
        /// <returns></returns>
        protected override string GetBodyData()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the header data.
        /// </summary>
        /// <returns></returns>
        protected override string GetHeaderData()
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var totalRecords = Model.Count() + 1; // +1 for header
            var totalColumns = InternalColumns.Count + 1; //+1 for index

            var generatedMarkupArr = new GeneratedColumnMarkup[totalRecords, totalColumns];

            var generatedHeaqderMarkups = GenerateHeaderColumnsMarkups().ToList();
            var requiredBytes = generatedHeaqderMarkups.Sum(x => x.HtmlMarkup.Length);

            for (var i = 0; i < generatedHeaqderMarkups.Count(); i++)
            {
                generatedMarkupArr[0, i] = generatedHeaqderMarkups[i];
            }

            var modelList = Model.ToList();

            for (var i = 0; i < modelList.Count; i++)
            {
                var model = modelList[i];
                var uid = 1000 + i;
                generatedHeaqderMarkups = GenerateRowColumnsMarkups(model, uid.ToString()).ToList();

                requiredBytes += generatedHeaqderMarkups.Sum(x => x.HtmlMarkup.Length);

                for (var j = 0; j < generatedHeaqderMarkups.Count(); j++)
                {
                    generatedMarkupArr[i + 1, j] = generatedHeaqderMarkups[j];//i_1 is one extra row for header
                }
            }

            var markupDate = new StringBuilder(requiredBytes + (totalColumns * totalRecords * 10) + 1024);// total markup to render + tr + td for each lines and cells + extra 1K

            //loop for columns as row
            for (var i = 0; i < totalColumns; i++)
            {
                markupDate.Append("<tr>");
                for (int j = 0; j < totalRecords; j++)
                {
                    var generatedMarkup = generatedMarkupArr[j, i];
                    if (generatedMarkup != null)
                    {
                        markupDate.Append(generatedMarkup.HtmlMarkup);
                    }
                }
                markupDate.Append("</tr>");
            }
            var tableTag = GetTableTagBuilder();
            tableTag.InnerHtml = markupDate.ToString();
            return tableTag.ToString();
        }

    }

}