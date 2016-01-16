using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MVCControls.Extensions
{
    public static class GridExtensions1
    {
        public static MvcHtmlString Grid<TModel>(this HtmlHelper<TModel> html)
        {
            var model = html.ViewData.Model as IEnumerable<dynamic>;

            //if model is not a list then throw an exception
            if (model == null)
            {
                throw new InvalidOperationException("Model type invalid.");
            }

            var modelType = model.GetType();
            var genericArgumentType = modelType.GetGenericArguments()[0];

            var grid = new WebGrid(model); //, defaultSort: "Name"

            var proprities = genericArgumentType.GetProperties();

            //generate columns base on type, ignore id field 
            var gridColumns = new List<WebGridColumn>()
                {
                    grid.Column(format: (item) => html.ActionLink("Details", "Details", new {id = item.Id},
                                                                  htmlAttributes:
                                                                      new {@class = "gridDetailsButton gridButton"})),
                    grid.Column(format: (item) => html.ActionLink("Edit", "Edit", new {id = item.Id},
                                                                  htmlAttributes:
                                                                      new {@class = "gridEditButton gridButton"})),
                    grid.Column(format: (item) => html.ActionLink("Delete", "Delete", new {id = item.Id},
                                                                  htmlAttributes:
                                                                      new {@class = "gridDeleteButton gridButton"}))
                };
            var dynamicGridColumns = proprities
                .Where(x => String.Compare(x.Name, "Id", StringComparison.OrdinalIgnoreCase) != 0)
                .Select(x =>
                    {
                        if (x.PropertyType != typeof (bool))
                            return grid.Column(x.Name);
                        var propertyName = x.Name;
                        return grid.Column(x.Name,
                                           format: (item) =>
                                                   InputExtensions.CheckBox(html, x.Name, isChecked: item[propertyName],
                                                                            htmlAttributes: new {@disabled = "disabled"}));
                    })
                .ToList();

            gridColumns.AddRange(dynamicGridColumns);

            var gridString = grid.GetHtml(columns: gridColumns,
                                          tableStyle: "gridTable",
                                          headerStyle: "gridHeader",
                                          footerStyle: "gridFooter",
                                          rowStyle: "gridRow",
                                          alternatingRowStyle: "gridAlternatingRow"
                );

            return new MvcHtmlString(gridString.ToHtmlString());
        }
    }
}