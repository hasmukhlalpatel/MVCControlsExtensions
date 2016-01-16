using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Web.Mvc;
using MVCControls.Controls;
using MVCControls.Controls.Ko;

namespace MVCControls.Extensions
{
    /// <summary>
    /// The grid control extensions
    /// </summary>
    public static class GridExtensions
    {
        /// <summary>
        /// create Grid for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="columnFactories">The column factories.</param>
        /// <returns></returns>
        public static MvcHtmlString GridFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TValue>>> expression,
            params Action<Grid<TValue>>[] columnFactories)
        {
            return GridFor(html, expression, null, columnFactories);
        }

        /// <summary>
        /// create Grid for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="options">Grid Options</param>
        /// <param name="columnFactories">The column factories.</param>
        /// <returns></returns>
        public static MvcHtmlString GridFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TValue>>> expression,
            GridOptions options = null,
            params Action<Grid<TValue>>[] columnFactories)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var htmlHelper = CreateHtmlHelper<TValue>(html.ViewContext.Controller);

            var grid = new Grid<TValue>(modelMetadata.Model as IEnumerable<TValue>,
                htmlHelper, modelMetadata.PropertyName)
            {
                Options = options
            };

            foreach (var factory in columnFactories)
            {
                factory(grid);
            }

            return grid.ToHtmlString();
        }

        /// <summary>
        /// Create Knockout grid for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="columnFactories">The column factories.</param>
        /// <returns></returns>
        public static MvcHtmlString KOGridFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TValue>>> expression,
            params Action<KOGrid<TValue>>[] columnFactories)
        {
            return KOGridFor(html, expression, null, columnFactories);
        }

        /// <summary>
        /// Create Knockout grid for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="options">Grid Options</param>
        /// <param name="columnFactories">The column factories.</param>
        /// <returns></returns>
        public static MvcHtmlString KOGridFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TValue>>> expression,
            GridOptions options = null,
            params Action<KOGrid<TValue>>[] columnFactories)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlHelper = CreateHtmlHelper<TValue>(html.ViewContext.Controller);

            var grid = new KOGrid<TValue>(modelMetadata.Model as IEnumerable<TValue>,
                htmlHelper,
                modelMetadata.PropertyName)
            {
                Options = options
            };

            foreach (var factory in columnFactories)
            {
                factory(grid);
            }

            return grid.ToHtmlString();
        }

        /// <summary>
        /// create Vertical Grid For.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="columnFactories">The column factories.</param>
        /// <returns></returns>
        public static MvcHtmlString VerticalGridFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TValue>>> expression,
            params Action<VGrid<TValue>>[] columnFactories)
        {
            return VerticalGridFor(html, expression, null, columnFactories);
        }

        /// <summary>
        /// create Vertical Grid For.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="options">Grid Options</param>
        /// <param name="columnFactories">The column factories.</param>
        /// <returns></returns>
        public static MvcHtmlString VerticalGridFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TValue>>> expression,
            GridOptions options = null,
            params Action<VGrid<TValue>>[] columnFactories)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var htmlHelper = CreateHtmlHelper<TValue>(html.ViewContext.Controller);

            var grid = new VGrid<TValue>(modelMetadata.Model as IEnumerable<TValue>,
                htmlHelper,
                modelMetadata.PropertyName)
            {
                Options = options
            };

            foreach (var factory in columnFactories)
            {
                factory(grid);
            }

            return grid.ToHtmlString();
        }

        /// <summary>
        /// Creates the HTML helper.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static HtmlHelper<TModel> CreateHtmlHelper<TModel>(ControllerBase controller)
        {
            var viewContext = new ViewContext(controller.ControllerContext,
                new FakeView(),
                new ViewDataDictionary<TModel>(), //controller.ViewData,
                controller.TempData,
                TextWriter.Null);

            return new HtmlHelper<TModel>(viewContext, new ViewPage());
        }
    }


    /// <summary>
    /// FakeView
    /// </summary>
    public class FakeView : IView
    {
        /// <summary>
        /// Renders the specified view context by using the specified the writer object.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        /// <param name="writer">The writer object.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new InvalidOperationException();
        }
    }
}