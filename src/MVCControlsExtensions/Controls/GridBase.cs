using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MVCControls.Extensions;

namespace MVCControls.Controls
{
    /// <summary>
    /// base class for grid control.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract class GridBase<TModel>
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; protected set; }

        /// <summary>
        /// The internal columns
        /// </summary>
        protected readonly List<GridColumn> InternalColumns = new List<GridColumn>();

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public IEnumerable<TModel> Model { get; private set; }

        /// <summary>
        /// Gets or sets the view data.
        /// </summary>
        /// <value>
        /// The view data.
        /// </value>
        public ViewDataDictionary<TModel> ViewData { get; set; }

        /// <summary>
        /// Gets the HTML helper.
        /// </summary>
        /// <value>
        /// The HTML helper.
        /// </value>
        public HtmlHelper<TModel> HtmlHelper { get; private set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public GridOptions Options { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridBase{TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected GridBase(IEnumerable<TModel> model, HtmlHelper<TModel> htmlHelper, string propertyName)
        {
            Model = model;
            HtmlHelper = htmlHelper;
            ViewData = htmlHelper.ViewData; //new ViewDataDictionary<TModel>();
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public IEnumerable<GridColumn> Columns
        {
            get { return InternalColumns; }
        }

        #region add columns

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="dataText">The data text.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public GridColumn AddColumn(string dataText, string headerText, object htmlAttributes = null)
        {
            var column = new GridColumn
            {
                DataText = dataText,
                HeaderText = headerText,
                HtmlAttributes = AttributesExtensions.GetAttributeList(htmlAttributes)
            };
            InternalColumns.Add(column);
            return column;
        }

        /// <summary>
        /// Adds the partial column.
        /// </summary>
        /// <param name="expr">The expression.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public GridPartialColumn<TModel> AddPartialColumn(Expression<Func<TModel>> expr,
            string partialViewName,
            string headerText = null, object htmlAttributes = null)
        {
            var column = new GridPartialColumn<TModel>(partialViewName)
            {
                HtmlHelper = HtmlHelper,
                ViewData = ViewData,
                HeaderText = headerText,
                HtmlAttributes = AttributesExtensions.GetAttributeList(htmlAttributes)
            };
            InternalColumns.Add(column);
            return column;
        }

        /// <summary>
        /// Adds the partial column.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public GridColumn<TModel, TValue> AddPartialColumn<TValue>(Expression<Func<TModel, TValue>> expr,
            string partialViewName,
            string headerText = null, object htmlAttributes = null)
        {
            var column = new GridPartialColumn<TModel, TValue>(partialViewName)
            {
                HtmlHelper = HtmlHelper,
                ViewData = ViewData,
                ColumnExpression = expr,
                HeaderText = headerText,
                HtmlAttributes = AttributesExtensions.GetAttributeList(htmlAttributes)
            };
            InternalColumns.Add(column);
            return column;
        }

        /// <summary>
        /// Adds the link column.
        /// </summary>
        /// <param name="dataText">The data text.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public GridColumn AddLinkColumn(string dataText, string headerText, object htmlAttributes = null)
        {
            var column = new GridLinkColumn
            {
                DataText = dataText,
                HeaderText = headerText,
                HtmlAttributes = AttributesExtensions.GetAttributeList(htmlAttributes)
            };
            InternalColumns.Add(column);
            return column;
        }

        /// <summary>
        /// Adds the action link column.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridActionLinkColumn<TModel> AddActionLinkColumn(string linkText, string actionName, string controllerName = null,
            object routeValues = null,
            string headerText = null,
            object htmlAttributes = null,
            object cellHtmlAttributes = null)
        {
            return AddGridActionLinkColumn(linkText, actionName, controllerName,
                routeValues, htmlAttributes, null, null, cellHtmlAttributes);
        }

        /// <summary>
        /// Adds the action link column.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributesFactory">The HTML attributes factory.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridActionLinkColumn<TModel> AddActionLinkColumn(string linkText, string actionName, string controllerName = null,
            object routeValues = null, string headerText = null,
            Func<TModel, object> htmlAttributesFactory = null,
            object cellHtmlAttributes = null)
        {
            return AddGridActionLinkColumn(linkText, actionName, controllerName,
                routeValues, null, null, htmlAttributesFactory, cellHtmlAttributes);
        }

        /// <summary>
        /// Adds the action link column.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValuesFactory">The route values factory.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridActionLinkColumn<TModel> AddActionLinkColumn(string linkText, string actionName, string controllerName = null,
            Func<TModel, object> routeValuesFactory = null, string headerText = null,
            object htmlAttributes = null,
            object cellHtmlAttributes = null)
        {
            return AddGridActionLinkColumn(linkText, actionName, controllerName,
                null, htmlAttributes, routeValuesFactory, null, cellHtmlAttributes);
        }

        /// <summary>
        /// Adds the action link column.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValuesFactory">The route values factory.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributesFactory">The HTML attributes factory.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridActionLinkColumn<TModel> AddActionLinkColumn(string linkText, string actionName, string controllerName = null,
            Func<TModel, object> routeValuesFactory = null, string headerText = null,
            Func<TModel, object> htmlAttributesFactory = null,
            object cellHtmlAttributes = null)
        {
            return AddGridActionLinkColumn(linkText, actionName, controllerName,
                null, null, routeValuesFactory, htmlAttributesFactory, cellHtmlAttributes);
        }

        /// <summary>
        /// Adds the grid action link column.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="routeValuesFactory">The route values factory.</param>
        /// <param name="htmlAttributesFactory">The HTML attributes factory.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        protected GridActionLinkColumn<TModel> AddGridActionLinkColumn(string linkText, string actionName, string controllerName,
            object routeValues, object htmlAttributes,
            Func<TModel, object> routeValuesFactory,
            Func<TModel, object> htmlAttributesFactory,
            object cellHtmlAttributes)
        {
            var column = new GridActionLinkColumn<TModel>
            {
                LinkText = linkText,
                ControllerName = controllerName,
                ActionName = actionName,
                RouteValues = routeValues,
                HtmlAttributes = AttributesExtensions.GetAttributeList(htmlAttributes),
                RouteValuesFactory = routeValuesFactory,
                HtmlAttributesFactory = htmlAttributesFactory
            };

            AddColumn(column, cellHtmlAttributes);
            return column;
        }


        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="dataFunc">The data function.</param>
        /// <param name="headerText">The header text.</param>
        /// <returns></returns>
        public GridColumn<TModel> AddColumn(Func<TModel, string> dataFunc, string headerText = null)
        {
            Func<TModel, MvcHtmlString> wrapperFunc = x => MvcHtmlString.Create(dataFunc(x));
            return AddColumn(wrapperFunc, headerText);
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="dataFunc">The data function.</param>
        /// <param name="headerText">The header text.</param>
        /// <returns></returns>
        public GridColumn<TModel> AddColumn(Func<TModel, MvcHtmlString> dataFunc, string headerText = null)
        {
            var column = new GridColumn<TModel>(dataFunc) { HeaderText = headerText };
            AddColumn(column, null);
            return column;
        }


        /// <summary>
        /// Adds the column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public GridColumn<TModel, TValue> AddColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                               string headerText = null,
                                                               object htmlAttributes = null,
                                                               object cellHtmlAttributes = null,
                                                               string templateName = null)
        {
            var column = new GridColumn<TModel, TValue>(templateName: templateName)
            {
                ColumnExpression = expr,
                HeaderText = headerText
            };
            AddColumn(column, htmlAttributes, cellHtmlAttributes);
            return column;
        }

        /// <summary>
        /// Adds the column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributesFactory">The HTML attributes factory.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public GridColumn<TModel, TValue> AddColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                               string headerText,
                                                               Func<TModel, object> htmlAttributesFactory,
                                                               object cellHtmlAttributes = null,
                                                               string templateName = null)
        {
            var column = new GridColumn<TModel, TValue>(templateName: templateName)
            {
                ColumnExpression = expr,
                HeaderText = headerText,
                HtmlAttributesFactory = htmlAttributesFactory
            };
            AddColumn(column, null, cellHtmlAttributes);
            return column;
        }

        /// <summary>
        /// Adds the hidden column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <returns></returns>
        public GridHiddenColumn<TModel, TValue> AddHiddenColumnFor<TValue>(Expression<Func<TModel, TValue>> expr)
        {
            var column = new GridHiddenColumn<TModel, TValue>
            {
                ColumnExpression = expr,
            };
            AddColumn(column);
            return column;
        }

        /// <summary>
        /// Adds the text column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridTextColumn<TModel, TValue> AddTextColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                       string headerText = null,
                                                                       object htmlAttributes = null,
                                                                       object cellHtmlAttributes = null)
        {
            var column = new GridTextColumn<TModel, TValue>
            {
                ColumnExpression = expr,
                HeaderText = headerText
            };
            AddColumn(column, htmlAttributes, cellHtmlAttributes);
            return column;
        }

        /// <summary>
        /// Adds the popup text column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="displayAttributes">The display attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridPopupTextColumn<TModel, TValue> AddPopupTextColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                       string headerText = null,
                                                                       object htmlAttributes = null,
                                                                       object displayAttributes = null,
                                                                       object cellHtmlAttributes = null)
        {
            var column = new GridPopupTextColumn<TModel, TValue>
            {
                ColumnExpression = expr,
                HeaderText = headerText
            };
            AddPopupColumn(column, htmlAttributes, displayAttributes, cellHtmlAttributes);

            return column;
        }

        /// <summary>
        /// Adds the popup text area column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="displayAttributes">The display attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridPopupTextAreaColumn<TModel, TValue> AddPopupTextAreaColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                       string headerText = null,
                                                                       object htmlAttributes = null,
                                                                       object displayAttributes = null,
                                                                       object cellHtmlAttributes = null)
        {
            var column = new GridPopupTextAreaColumn<TModel, TValue>
            {
                ColumnExpression = expr,
                HeaderText = headerText
            };
            AddPopupColumn(column, htmlAttributes, displayAttributes, cellHtmlAttributes);

            return column;
        }

        /// <summary>
        /// Adds the popup column.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="column">The column.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="displayAttributes">The display attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        protected void AddPopupColumn<TValue>(GridPopupTextColumn<TModel, TValue> column,
            object htmlAttributes = null,
            object displayAttributes = null,
            object cellHtmlAttributes = null)
        {
            column.DisplayAttributes = AttributesExtensions.GetAttributeList(displayAttributes);

            AddColumn(column, htmlAttributes, cellHtmlAttributes);
        }

        /// <summary>
        /// Adds the text column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributesFactory">The HTML attributes factory.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridTextColumn<TModel, TValue> AddTextColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                       string headerText,
                                                                       Func<TModel, object> htmlAttributesFactory,
                                                                       object cellHtmlAttributes = null)
        {
            var column = new GridTextColumn<TModel, TValue>
            {
                ColumnExpression = expr,
                HeaderText = headerText,
                HtmlAttributesFactory = htmlAttributesFactory
            };
            AddColumn(column, null, cellHtmlAttributes);
            return column;
        }

        /// <summary>
        /// Adds the CheckBox column for.
        /// </summary>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridCheckBoxColumn<TModel> AddCheckBoxColumnFor(Expression<Func<TModel, bool>> expr,
                                                               string headerText = null,
                                                               object htmlAttributes = null,
                                                               object cellHtmlAttributes = null)
        {
            var column = new GridCheckBoxColumn<TModel>
            {
                ColumnExpression = expr,
                HeaderText = headerText,
            };
            AddColumn(column, htmlAttributes, cellHtmlAttributes);
            return column;
        }

        /// <summary>
        /// Adds the CheckBox column for.
        /// </summary>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributesFactory">The HTML attributes factory.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridCheckBoxColumn<TModel> AddCheckBoxColumnFor(Expression<Func<TModel, bool>> expr,
                                                               string headerText,
                                                               Func<TModel, object> htmlAttributesFactory,
                                                               object cellHtmlAttributes = null)
        {
            var column = new GridCheckBoxColumn<TModel>
            {
                ColumnExpression = expr,
                HeaderText = headerText,
                HtmlAttributesFactory = htmlAttributesFactory
            };
            AddColumn(column, null, cellHtmlAttributes);
            return column;
        }

        /// <summary>
        /// Adds the drop down column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridDropDownColumn<TModel, TValue> AddDropDownColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                               string headerText = null,
                                                                               object htmlAttributes = null,
                                                                               object cellHtmlAttributes = null)
        {
            var column = new GridDropDownColumn<TModel, TValue>
            {
                ColumnExpression = expr,
                HeaderText = headerText
            };
            AddColumn(column, htmlAttributes, cellHtmlAttributes);
            return column;
        }

        /// <summary>
        /// Adds the drop down column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributesFactory">The HTML attributes factory.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridDropDownColumn<TModel, TValue> AddDropDownColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                               string headerText,
                                                                               Func<TModel, object> htmlAttributesFactory,
                                                                               object cellHtmlAttributes = null)
        {
            var column = new GridDropDownColumn<TModel, TValue>
            {
                ColumnExpression = expr,
                HeaderText = headerText,
                HtmlAttributesFactory = htmlAttributesFactory
            };

            AddColumn(column, null, cellHtmlAttributes);
            return column;
        }
        
        /// <summary>
        /// Adds the Radio button column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridRadioButtonColumn<TModel, TValue> AddRadioButtonColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                               string headerText = null,
                                                                               object htmlAttributes = null,
                                                                               object cellHtmlAttributes = null)
        {
            var column = new GridRadioButtonColumn<TModel, TValue>
            {
                ColumnExpression = expr,
                HeaderText = headerText
            };
            AddColumn(column, htmlAttributes, cellHtmlAttributes);
            return column;
        }

        /// <summary>
        /// Adds the RadioButton column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributesFactory">The HTML attributes factory.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <returns></returns>
        public GridRadioButtonColumn<TModel, TValue> AddRadioButtonColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                               string headerText,
                                                                               Func<TModel, object> htmlAttributesFactory,
                                                                               object cellHtmlAttributes = null)
        {
            var column = new GridRadioButtonColumn<TModel, TValue>
            {
                ColumnExpression = expr,
                HeaderText = headerText,
                HtmlAttributesFactory = htmlAttributesFactory
            };
            AddColumn(column, null, cellHtmlAttributes);
            return column;
        }

        #endregion

        #region protected members

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="column">The column.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        protected void AddColumn<TValue>(GridColumn<TModel, TValue> column,
                                         object htmlAttributes = null,
                                         object cellHtmlAttributes = null)
        {
            column.HtmlHelper = HtmlHelper;
            column.ViewData = ViewData;
            column.HtmlAttributes = AttributesExtensions.GetAttributeList(htmlAttributes);
            column.CellHtmlAttributes = AttributesExtensions.GetAttributeList(cellHtmlAttributes);
            InternalColumns.Add(column);
        }

        private void AddColumn(GridColumnBase<TModel> column, object cellHtmlAttributes)
        {
            column.HtmlHelper = HtmlHelper;
            column.ViewData = ViewData;
            column.CellHtmlAttributes = AttributesExtensions.GetAttributeList(cellHtmlAttributes);
            InternalColumns.Add(column);
        }

        /// <summary>
        /// Gets the model metadata.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        protected ModelMetadata GetModelMetadata<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return ModelMetadata.FromLambdaExpression(expression, ViewData);
        }

        /// <summary>
        /// The table tag
        /// </summary>
        protected const string TableTag = "\n\t<table style=\"width: 100%\" class=\"gridTable\">{0}\n\t</table>";

        /// <summary>
        /// The table body tag
        /// </summary>
        protected const string TableBodyTag = "\n\t\t<tbody data-template=\"{1}\">{0}\n\t\t</tbody>";


        /// <summary>
        /// The table Header tag start
        /// </summary>
        protected const string TableHeaderTag = "\n\t\t<thead>\n\t\t\t<tr class=\"gridHeader\">";

        /// <summary>
        /// The table Header tag end
        /// </summary>
        protected const string TableHeaderTagEnd = "\n\t\t\t</tr>\n\t\t</thead>";

        /// <summary>
        /// The table row start tag
        /// </summary>
        protected const string TableRowTag = "\n\t\t\t<tr id=\"{0}\" data-template=\"{1}\" class=\"gridRow\">";

        /// <summary>
        /// The table row end tag
        /// </summary>
        protected const string TableRowTagEnd = "\n\t\t\t</tr>";

        /// <summary>
        /// The table data (td) tag
        /// </summary>
        protected const string TableDataTag = "\n\t\t\t\t<td{0}>{1}</td>";

        /// <summary>
        /// The table th tag
        /// </summary>
        protected const string TableTHTag = "\n\t\t\t\t<th{0}>{1}</th>";

        #region TagBuilders

        /// <summary>
        /// Gets the grid container tag builder.
        /// </summary>
        /// <returns></returns>
        protected virtual TagBuilder GetGridContainerTagBuilder()
        {
            var htmlTag = new TagBuilder("div");
            var style = "width: 100%";
            if (Options != null && !string.IsNullOrEmpty(Options.ContainerStyle))
            {
                style = Options.ContainerStyle;
            }
            htmlTag.MergeAttribute("style", style);

            htmlTag.AddCssClass("gridContainer");
            if (Options != null && !string.IsNullOrEmpty(Options.ContainerClass))
            {
                htmlTag.AddCssClass(Options.ContainerClass);
            }
            return htmlTag;
        }

        /// <summary>
        /// Gets the table tag builder.
        /// </summary>
        /// <returns></returns>
        protected virtual TagBuilder GetTableTagBuilder()
        {
            var htmlTag = new TagBuilder("table");
            htmlTag.MergeAttribute("style", "width: 100%");
            htmlTag.AddCssClass("gridTable");
            if (Options != null && !string.IsNullOrEmpty(Options.TableClass))
            {
                htmlTag.AddCssClass(Options.TableClass);
            }
            return htmlTag;
        }

        /// <summary>
        /// Gets the table head tag builder.
        /// </summary>
        /// <returns></returns>
        protected virtual TagBuilder GetTableHeadTagBuilder()
        {
            var htmlTag = new TagBuilder("thead");
            htmlTag.AddCssClass("gridHead");
            if (Options != null && !string.IsNullOrEmpty(Options.HeaderStyle))
            {
                htmlTag.AddCssClass(Options.HeaderStyle);
            }
            return htmlTag;
        }

        /// <summary>
        /// Gets the table body tag builder.
        /// </summary>
        /// <returns></returns>
        protected virtual TagBuilder GetTableBodyTagBuilder()
        {
            var htmlTag = new TagBuilder("tbody");
            htmlTag.AddCssClass("gridBody");
            if (Options != null && !string.IsNullOrEmpty(Options.BodyStyle))
            {
                htmlTag.AddCssClass(Options.BodyStyle);
            }
            return htmlTag;
        }

        /// <summary>
        /// Gets the table row tag builder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="defaultClass">The default class.</param>
        /// <returns></returns>
        protected virtual TagBuilder GetTableRowTagBuilder(string id = null, string defaultClass = null)
        {
            var htmlTag = new TagBuilder("tr");

            if (!string.IsNullOrEmpty(id))
                htmlTag.MergeAttribute("id", id);

            if (!string.IsNullOrEmpty(defaultClass))
                htmlTag.AddCssClass(defaultClass);

            return htmlTag;
        }

        #endregion

        /// <summary>
        /// Generates the header columns markups.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<GeneratedColumnMarkup> GenerateHeaderColumnsMarkups()
        {
            var markupCols = InternalColumns.Where(x => !x.IsHidden)
                .Select(c => new GeneratedColumnMarkup
                {
                    Column = c,
                    HtmlMarkup = string.Format(TableTHTag,
                        AttributesExtensions.ConvertDictionaryToAttributes(c.CellHtmlAttributes),
                        c.RenderHeader())
                }).ToList();

            markupCols.Add(new GeneratedColumnMarkup { HtmlMarkup = string.Empty });// empty column for index
            return markupCols;
        }

        /// <summary>
        /// Gets the header data.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetHeaderData()
        {
            var markups = GenerateHeaderColumnsMarkups().ToList();

            var markupTotalBytes = markups.Sum(x => x.HtmlMarkup.Length);

            var headerData = new StringBuilder(markupTotalBytes + 100);
            markups.ForEach(x => headerData.Append(x.HtmlMarkup));

            var htmlHeadTag = GetTableHeadTagBuilder();
            var htmlHeaderRowTag = GetTableRowTagBuilder(defaultClass: "gridHeader");
            htmlHeaderRowTag.InnerHtml = headerData.ToString();
            htmlHeadTag.InnerHtml = htmlHeaderRowTag.ToString();
            return htmlHeadTag.ToString();
        }

        /// <summary>
        /// Gets the row template.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="rowId">The row identifier.</param>
        /// <returns></returns>
        protected virtual string GetRowTemplate(TModel model, string rowId)
        {
            return null;
        }

        /// <summary>
        /// Sets the HTML field prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        protected virtual void SetHtmlFieldPrefix(string prefix)
        {
            HtmlHelper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = prefix;
        }

        /// <summary>
        /// Sets the model for data.
        /// </summary>
        /// <param name="model">The model.</param>
        protected virtual void SetModelForData(TModel model)
        {
            ViewData.Model = model;
        }

        /// <summary>
        /// Generates the visible column markup.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        protected virtual GeneratedColumnMarkup GenerateVisibleColumnMarkup(GridColumn column)
        {
            return new GeneratedColumnMarkup
            {
                Column = column,
                HtmlMarkup = string.Format(TableDataTag,
                    AttributesExtensions.ConvertDictionaryToAttributes(column.CellHtmlAttributes),
                    column.Render())
            };
        }


        /// <summary>
        /// Generates the hidden column markup.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        protected virtual GeneratedColumnMarkup GenerateHiddenColumnMarkup(GridColumn column)
        {
            return new GeneratedColumnMarkup
            {
                Column = column,
                HtmlMarkup = column.Render()
            };
        }

        /// <summary>
        /// Generates the row columns markups.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="rowId">The row identifier.</param>
        /// <returns></returns>
        protected IEnumerable<GeneratedColumnMarkup> GenerateRowColumnsMarkups(TModel model, string rowId)
        {
            SetModelForData(model);

            SetHtmlFieldPrefix(string.Empty);
            //SetHtmlFieldPrefix must be after index column because index column will generate rowid with index id/name
            var indexColumnMarkup = new GeneratedColumnMarkup
            {
                HtmlMarkup = HtmlHelper.Hidden(string.Format("{0}.Index", PropertyName), rowId).ToHtmlString()
            };

            SetHtmlFieldPrefix(string.Format("{0}[{1}]", PropertyName, rowId));

            var markupCols = InternalColumns
                .Select(x => x.IsHidden ? GenerateHiddenColumnMarkup(x) : GenerateVisibleColumnMarkup(x))
                .ToList();

            markupCols.Add(indexColumnMarkup);

            return markupCols;
        }


        /// <summary>
        /// Gets the row data.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="rowId">The row identifier.</param>
        /// <returns></returns>
        protected virtual string GetRowData(TModel model, string rowId)
        {
            var markups = GenerateRowColumnsMarkups(model, rowId).ToList();

            var markupTotalBytes = markups.Sum(x => x.HtmlMarkup.Length);

            var rowData = new StringBuilder(markupTotalBytes + 100);
            var htmlRowTag = GetTableRowTagBuilder(rowId, "gridRow");

            markups.ToList().ForEach(x => rowData.Append(x.HtmlMarkup));

            var rowTemplate = GetRowTemplate(model, rowId);

            if (!string.IsNullOrEmpty(rowTemplate))
                htmlRowTag.MergeAttribute("data-template", rowTemplate);

            htmlRowTag.InnerHtml = rowData.ToString();
            return htmlRowTag.ToString();
        }

        /// <summary>
        /// Gets the body data.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetBodyData()
        {
            var bodyData = new StringBuilder(1024);

            var rowId = 1000;

            foreach (var model in Model)
            {
                var rowData = GetRowData(model, rowId.ToString());

                bodyData.Append(rowData);
                rowId++;
            }

            var templateModel = Activator.CreateInstance<TModel>();
            const string newRowId = "{0000}";

            var template = HttpContext.Current.Server.HtmlEncode(GetRowData(templateModel, newRowId));

            var tbodyTag = GetTableBodyTagBuilder();

            tbodyTag.MergeAttribute("data-template", template);
            tbodyTag.InnerHtml = bodyData.ToString();

            return tbodyTag.ToString();
        }

        /// <summary>
        /// Adds the button column.
        /// </summary>
        /// <param name="buttonMarkup">The button markup.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="defaultStyle">The default style.</param>
        /// <returns></returns>
        protected GridColumn AddButtonColumn(string buttonMarkup, string headerText, object htmlAttributes, string defaultStyle)
        {
            var column = AddLinkColumn(buttonMarkup, headerText, htmlAttributes);
            column.HtmlAttributes.AddIfNot("class", defaultStyle);
            column.HtmlAttributes.AddIfNot("href", "#");
            return column;
        }

        #endregion

        /// <summary>
        /// To the HTML string.
        /// </summary>
        /// <returns></returns>
        public virtual MvcHtmlString ToHtmlString()
        {
            return MvcHtmlString.Create(ToString());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var headerData = GetHeaderData();

            var bodyData = GetBodyData();

            var tableTag = GetTableTagBuilder();
            tableTag.InnerHtml = headerData + bodyData;
            var container = GetGridContainerTagBuilder();
            container.InnerHtml = tableTag.ToString();

            return container.ToString();
        }

        /// <summary>
        /// Generated Column Markup
        /// </summary>
        protected class GeneratedColumnMarkup
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GeneratedColumnMarkup"/> class.
            /// </summary>
            public GeneratedColumnMarkup()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="GeneratedColumnMarkup"/> class.
            /// </summary>
            /// <param name="column">The column.</param>
            /// <param name="htmlMarkup">The HTML markup.</param>
            public GeneratedColumnMarkup(GridColumn column, string htmlMarkup)
            {
                Column = column;
                HtmlMarkup = htmlMarkup;
            }

            /// <summary>
            /// Gets or sets the column.
            /// </summary>
            /// <value>
            /// The column.
            /// </value>
            public GridColumn Column { get; set; }

            /// <summary>
            /// Gets or sets the HTML markup.
            /// </summary>
            /// <value>
            /// The HTML markup.
            /// </value>
            public string HtmlMarkup { get; set; }
        }
    }

    #region Grid Options

    /// <summary>
    /// 
    /// </summary>
    public class GridOptions
    {
        /// <summary>
        /// Gets or sets the container class.
        /// </summary>
        /// <value>
        /// The container class.
        /// </value>
        public string ContainerClass { get; set; }
        /// <summary>
        /// Gets or sets the table class.
        /// </summary>
        /// <value>
        /// The table class.
        /// </value>
        public string TableClass { get; set; }
        /// <summary>
        /// Gets or sets the container style.
        /// </summary>
        /// <value>
        /// The container style.
        /// </value>
        public string ContainerStyle { get; set; }
        /// <summary>
        /// Gets or sets the header style.
        /// </summary>
        /// <value>
        /// The header style.
        /// </value>
        public string HeaderStyle { get; set; }
        /// <summary>
        /// Gets or sets the body style.
        /// </summary>
        /// <value>
        /// The body style.
        /// </value>
        public string BodyStyle { get; set; }
    }

    #endregion
}