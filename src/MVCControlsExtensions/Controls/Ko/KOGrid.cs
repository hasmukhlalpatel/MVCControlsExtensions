using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MVCControls.Extensions;
using MVCControls.Extensions.Ko;

namespace MVCControls.Controls.Ko
{
    /// <summary>
    /// Knockout Grid
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class KOGrid<TModel> : Grid<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KOGrid{TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyName">Name of the property.</param>
        public KOGrid(IEnumerable<TModel> model, HtmlHelper<TModel> htmlHelper, string propertyName)
            : base(model, htmlHelper, propertyName)
        {
        }

        #region add KO Columns

        /// <summary>
        /// Adds the ko column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridColumn<TModel, TValue> AddKOColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                 string headerText = null,
                                                                 object htmlAttributes = null,
                                                                 string binding = "text",
                                                                 KOBindingOptions options = null,
                                                                 object cellHtmlAttributes = null,
                                                                 KOBindingOptions cellBindingOptions = null)
        {
            var column = AddColumnFor(expr, headerText, htmlAttributes, cellHtmlAttributes);

            SetBinding(column, expr, binding, options);
            SetCellBinding(cellBindingOptions, column);
            return column;
        }


        /// <summary>
        /// Adds the ko date column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridColumn<TModel, TValue> AddKODateColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                 string headerText = null,
                                                                 object htmlAttributes = null,
                                                                 string binding = "dateString",
                                                                 KOBindingOptions options = null,
                                                                 object cellHtmlAttributes = null,
                                                                 KOBindingOptions cellBindingOptions = null)
        {
            return AddKOColumnFor(expr, headerText, htmlAttributes,
                                  binding ?? "dateString", options, cellHtmlAttributes, cellBindingOptions);
        }

        /// <summary>
        /// Adds the ko text column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridTextColumn<TModel, TValue> AddKOTextColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                         string headerText = null,
                                                                         object htmlAttributes = null,
                                                                         string binding = "value",
                                                                         KOBindingOptions options = null,
                                                                         object cellHtmlAttributes = null,
                                                                         KOBindingOptions cellBindingOptions = null)
        {
            var column = AddTextColumnFor(expr, headerText, htmlAttributes, cellHtmlAttributes);

            SetBinding(column, expr, binding, options);

            SetCellBinding(cellBindingOptions, column);
            return column;
        }

        /// <summary>
        /// Adds the ko popup text column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="displayAttributes">The display attributes.</param>
        /// <param name="displayBinding">The display binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridPopupTextColumn<TModel, TValue> AddKOPopupTextColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                         string headerText = null,
                                                                         object htmlAttributes = null,
                                                                         string binding = "value",
                                                                         object displayAttributes = null,
                                                                         string displayBinding = "text",
                                                                         KOBindingOptions options = null,
                                                                         object cellHtmlAttributes = null,
                                                                         KOBindingOptions cellBindingOptions = null)
        {
            var column = AddPopupTextColumnFor(expr, headerText, htmlAttributes, displayAttributes, cellHtmlAttributes);

            SetBinding(column, expr, binding, options);

            SetPopupTextBinding(column, expr, displayBinding, null);

            SetCellBinding(cellBindingOptions, column);
            return column;
        }

        /// <summary>
        /// Adds the ko pop-up text area column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="displayAttributes">The display attributes.</param>
        /// <param name="displayBinding">The display binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridPopupTextAreaColumn<TModel, TValue> AddKOPopupTextAreaColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                         string headerText = null,
                                                                         object htmlAttributes = null,
                                                                         string binding = "value",
                                                                         object displayAttributes = null,
                                                                         string displayBinding = "text",
                                                                         KOBindingOptions options = null,
                                                                         object cellHtmlAttributes = null,
                                                                         KOBindingOptions cellBindingOptions = null)
        {
            var column = AddPopupTextAreaColumnFor(expr, headerText, htmlAttributes, displayAttributes, cellHtmlAttributes);

            SetBinding(column, expr, binding, options);

            SetPopupTextBinding(column, expr, displayBinding, null);

            SetCellBinding(cellBindingOptions, column);
            return column;
        }

        private void SetPopupTextBinding<TValue>(GridPopupTextColumn<TModel, TValue> column,
                Expression<Func<TModel, TValue>> expr,
                string binding, KOBindingOptions options,
                bool uniqueName = false)
        {
            var metadata = GetModelMetadata(expr);

            KOControlExtensions.AddDataBinding(metadata.PropertyName, binding,
                                               column.DisplayAttributes, uniqueName, options);
        }

        /// <summary>
        /// Adds the ko date text column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridTextColumn<TModel, TValue> AddKODateTextColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                             string headerText = null,
                                                                             object htmlAttributes = null,
                                                                             string binding = "dateString",
                                                                             KOBindingOptions options = null,
                                                                             object cellHtmlAttributes = null,
                                                                             KOBindingOptions cellBindingOptions = null)
        {
            var column = AddTextColumnFor(expr, headerText, htmlAttributes, cellHtmlAttributes);

            options = options ?? new KOBindingOptions();
            options.ExtraParameters = options.ExtraParameters ?? new { showDatePicker = "true" };

            SetBinding(column, expr, binding, options);

            SetCellBinding(cellBindingOptions, column);
            return column;
        }


        /// <summary>
        /// Adds the ko CheckBox column for.
        /// </summary>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridCheckBoxColumn<TModel> AddKOCheckBoxColumnFor(Expression<Func<TModel, bool>> expr,
                                                                 string headerText = null,
                                                                 object htmlAttributes = null,
                                                                 string binding = "checked",
                                                                 KOBindingOptions options = null,
                                                                 object cellHtmlAttributes = null,
                                                                 KOBindingOptions cellBindingOptions = null)
        {
            var column = AddCheckBoxColumnFor(expr, headerText, htmlAttributes, cellHtmlAttributes);

            SetBinding(column, expr, binding, options);

            SetCellBinding(cellBindingOptions, column);
            return column;
        }

        /// <summary>
        /// Adds the ko RadioButton column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridRadioButtonColumn<TModel, TValue> AddKORadioButtonColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                                 string headerText = null,
                                                                                 object htmlAttributes = null,
                                                                                 string binding = "checked",
                                                                                 KOBindingOptions options = null,
                                                                                 object cellHtmlAttributes = null,
                                                                                 KOBindingOptions cellBindingOptions = null)
        {
            var column = AddRadioButtonColumnFor(expr, headerText, htmlAttributes, cellHtmlAttributes);

            SetBinding(column, expr, binding, options, true);

            SetCellBinding(cellBindingOptions, column);
            return column;
        }

        /// <summary>
        /// Adds the ko drop down column for.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expr">The Expression.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridDropDownColumn<TModel, TValue> AddKODropDownColumnFor<TValue>(Expression<Func<TModel, TValue>> expr,
                                                                                 string headerText = null,
                                                                                 object htmlAttributes = null,
                                                                                 string binding = "value",
                                                                                 KOBindingOptions options = null,
                                                                                 object cellHtmlAttributes = null,
                                                                                 KOBindingOptions cellBindingOptions = null)
        {
            var column = AddDropDownColumnFor(expr, headerText, htmlAttributes, cellHtmlAttributes);

            SetBinding(column, expr, binding, options);

            SetCellBinding(cellBindingOptions, column);
            return column;
        }

        #region KO button columns
        //<a class="gridDeleteButton smallGridButton" href="#" data-bind='click: $parent.removeSupplierContact '>Delete</a>

        private const string KOAddButton = "<button class='addButton' type='button' {1}>{0}</button>";

        /// <summary>
        /// Adds the ko edit button column.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="options">The options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridColumn AddKOEditButtonColumn(string displayText, string headerText = null,
                                                object htmlAttributes = null,
                                                KOBindingOptions options = null,
                                                object cellHtmlAttributes = null,
                                                KOBindingOptions cellBindingOptions = null)
        {
            return AddKOButtonColumn(displayText, headerText, htmlAttributes, options, cellBindingOptions,
                                     "gridEditButton smallGridButton");
        }

        /// <summary>
        /// Adds the ko delete button column.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="options">The options.</param>
        /// <param name="headerOptions">The header options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridColumn AddKODeleteButtonColumn(string displayText, string headerText = null,
                                                  object htmlAttributes = null,
                                                  KOBindingOptions options = null,
                                                  KOBindingOptions headerOptions = null,
                                                  object cellHtmlAttributes = null,
                                                  KOBindingOptions cellBindingOptions = null)
        {
            //generate add button
            if (headerOptions != null)
            {
                headerText = string.Format(KOAddButton, headerText ?? "", headerOptions.ToBindingString());
            }

            return AddKOButtonColumn(displayText, headerText, htmlAttributes, options, cellBindingOptions,
                                     "gridDeleteButton smallGridButton");
        }

        /// <summary>
        /// Adds the ko link button column.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="options">The options.</param>
        /// <param name="headerOptions">The header options.</param>
        /// <param name="cellHtmlAttributes">The cell HTML attributes.</param>
        /// <param name="cellBindingOptions">The cell binding options.</param>
        /// <returns></returns>
        public GridColumn AddKOLinkButtonColumn(string displayText, string headerText = null,
                                                object htmlAttributes = null,
                                                KOBindingOptions options = null,
                                                KOBindingOptions headerOptions = null,
                                                object cellHtmlAttributes = null,
                                                KOBindingOptions cellBindingOptions = null)
        {
            //generate add button
            if (headerOptions != null)
            {
                headerText = string.Format(KOAddButton, headerText ?? "", headerOptions.ToBindingString());
            }

            return AddKOButtonColumn(displayText, headerText, htmlAttributes, options, cellBindingOptions,
                                     "smallGridButton");
        }

        private GridColumn AddKOButtonColumn(string buttonMarkup, string headerText, object htmlAttributes,
                                             KOBindingOptions options,
                                             KOBindingOptions cellBindingOptions, string defaultStyle)
        {
            var column = AddButtonColumn(buttonMarkup, headerText, htmlAttributes, defaultStyle);
            if (options != null)
            {
                column.HtmlAttributes.AddIfNot("data-bind", options.ToParams());
            }

            SetCellBinding(cellBindingOptions, column);
            return column;
        }

        #endregion

        private void SetBinding<TValue>(GridColumn column,
                                Expression<Func<TModel, TValue>> expr,
                                string binding, KOBindingOptions options,
                                bool uniqueName = false)
        {
            var metadata = GetModelMetadata(expr);

            KOControlExtensions.AddDataBinding(metadata.PropertyName, binding,
                                               column.HtmlAttributes, uniqueName, options);
        }

        private static void SetCellBinding(KOBindingOptions cellBindingOptions, GridColumn column)
        {
            if (cellBindingOptions != null)
                column.CellHtmlAttributes.Add("data-bind", cellBindingOptions.ToParams());
        }

        #endregion

        #region overrides

        /// <summary>
        /// Gets the body data.
        /// </summary>
        /// <returns></returns>
        protected override string GetBodyData()
        {
            var bodyData = new StringBuilder(1024);
            //create only body template for knockout
            var model = Activator.CreateInstance<TModel>();
            const string rowId = "0000";
            var rowData = GetRowData(model, rowId);
            bodyData.Append(rowData);

            var tbodyTag = GetTableBodyTagBuilder();
            tbodyTag.MergeAttribute("data-bind", string.Format("foreach: {0}", PropertyName));
            tbodyTag.InnerHtml = bodyData.ToString();
            return tbodyTag.ToString();
        }

        #endregion

    }

}