using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MVCControls.Extensions;

namespace MVCControls.Controls
{
    #region general columns

    /// <summary>
    /// Anchor (link) column for the grid. 
    /// </summary>
    public class GridLinkColumn : GridColumn
    {
        /// <summary>
        /// Renders the column data.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            if (HtmlAttributes != null && HtmlAttributes.Any())
            {
                var attributes = AttributesExtensions.ConvertDictionaryToAttributes(HtmlAttributes);
                return string.Format("<a {1}>{0}</a>", DataText, attributes);
            }
            return DataText;
        }
    }

 
    /// <summary>
    /// Generic grid column.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class GridColumn<TModel> : GridColumnBase<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridColumn{TModel}"/> class.
        /// </summary>
        /// <param name="dataFunction">The data function.</param>
        public GridColumn(Func<TModel, MvcHtmlString> dataFunction)
        {
            DataFunction = dataFunction;
        }

        /// <summary>
        /// Gets or sets the data function.
        /// </summary>
        /// <value>
        /// The data function.
        /// </value>
        public Func<TModel, MvcHtmlString> DataFunction { get; protected set; }


        /// <summary>
        /// Renders the column data.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            var value = DataFunction(Model);
            return value != null ? value.ToHtmlString() : string.Empty;
        }
    }


    /// <summary>
    /// GEneric grid column,.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    public class GridColumn<TModel, TValue> : GridColumnBase<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridColumn{TModel, TValue}"/> class.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="valueFormatter">The value formatter.</param>
        /// <param name="templateName">Name of the template.</param>
        public GridColumn(Func<TModel, TValue, string> formatter = null,
            Func<TValue, string> valueFormatter = null, string templateName = null)
        {
            Formatter = formatter;
            ValueFormatter = valueFormatter;
            TemplateName = templateName;
        }

        /// <summary>
        /// Gets the model metadata.
        /// </summary>
        /// <returns></returns>
        protected ModelMetadata GetModelMetadata()
        {
            return ModelMetadata.FromLambdaExpression(ColumnExpression, ViewData);
        }

        readonly object _defaultValue = default(TValue);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns></returns>
        protected TValue GetValue(ModelMetadata metadata = null)
        {
            var modelMetadata = metadata ?? GetModelMetadata();
            return (TValue)(modelMetadata.Model ?? _defaultValue);
        }

        /// <summary>
        /// Gets or sets the formatter.
        /// </summary>
        /// <value>
        /// The formatter.
        /// </value>
        public Func<TModel, TValue, string> Formatter { get; protected set; }
        /// <summary>
        /// Gets or sets the value formatter.
        /// </summary>
        /// <value>
        /// The value formatter.
        /// </value>
        public Func<TValue, string> ValueFormatter { get; protected set; }
        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        public string TemplateName { get; protected set; }

        /// <summary>
        /// Gets or sets the column expression.
        /// </summary>
        /// <value>
        /// The column expression.
        /// </value>
        public Expression<Func<TModel, TValue>> ColumnExpression { get; set; }

        private Func<TModel, bool> _renderWhenFunc;

        /// <summary>
        /// Renders the when.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public void RenderWhen(Func<TModel, bool> predicate)
        {
            _renderWhenFunc = predicate;
        }

        /// <summary>
        /// Renders the column data.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            if (_renderWhenFunc != null && !_renderWhenFunc(Model))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(TemplateName))
            {
                var value = HtmlHelper.DisplayTextValueFor(ColumnExpression);
                var attributes = AttributesExtensions.ConvertDictionaryToAttributes(HtmlAttributes);
                return string.Format("<span {1}>{0}</span>", value, attributes);
            }
            return HtmlHelper.DisplayFor(ColumnExpression, TemplateName).ToHtmlString();
        }

        /// <summary>
        /// Renders the header text.
        /// </summary>
        /// <returns></returns>
        public override string RenderHeader()
        {
            if (!string.IsNullOrEmpty(HeaderText))
                return HeaderText;

            var modelMetadata = GetModelMetadata();
            var text = modelMetadata.DisplayName ?? modelMetadata.PropertyName;
            return text != null ? (text).PascalCaseToString() : string.Empty;
        }
    }

    /// <summary>
    /// Generic text column for property.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class GridHiddenColumn<TModel, TValue> : GridColumn<TModel, TValue>
    {
        /// <summary>
        /// IsHidden
        /// </summary>
        public override bool IsHidden { get { return true; } }

        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            return HtmlHelper.HiddenFor(ColumnExpression, GetHtmlAttributes()).ToHtmlString();
        }
    }


    /// <summary>
    /// Generic text column for property.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class GridTextColumn<TModel, TValue> : GridColumn<TModel, TValue>
    {
        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            return HtmlHelper.TextBoxFor(ColumnExpression, GetHtmlAttributes()).ToHtmlString();
        }
    }

    /// <summary>
    /// Grid Popup Text Column
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class GridPopupTextColumn<TModel, TValue> : GridColumn<TModel, TValue>
    {
        /// <summary>
        /// The popup text markup
        /// </summary>
        protected const string PopupTextMarkup = "<div tabIndex=\"0\" class=\"gridPopupTextHover\">{0}\n\t<div class=\"gridPopupTextTooltip\">{1}</div>\n</div>";

        /// <summary>
        /// Gets or sets the display attributes.
        /// </summary>
        /// <value>
        /// The display attributes.
        /// </value>
        public IDictionary<string, object> DisplayAttributes { get; set; }

        /// <summary>
        /// Renders the column data.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            var displayMarkup = GetDisplayMarkup();

            return string.Format(PopupTextMarkup,
                displayMarkup, HtmlHelper.TextBoxFor(ColumnExpression, HtmlAttributes));
        }

        /// <summary>
        /// Gets the display markup.
        /// </summary>
        /// <returns></returns>
        protected string GetDisplayMarkup()
        {
            var value = HtmlHelper.DisplayTextValueFor(ColumnExpression);
            var attributes = AttributesExtensions.ConvertDictionaryToAttributes(DisplayAttributes);
            var displayMarkup = string.Format("<span {1}>{0}</span>", value, attributes);
            return displayMarkup;
        }
    }

    /// <summary>
    /// Grid Popup TextArea Column
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class GridPopupTextAreaColumn<TModel, TValue> : GridPopupTextColumn<TModel, TValue>
    {
        /// <summary>
        /// Renders the column data.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            var displayMarkup = GetDisplayMarkup();

            return string.Format(PopupTextMarkup,
                displayMarkup, HtmlHelper.TextAreaFor(ColumnExpression, HtmlAttributes));
        }
    }


    /// <summary>
    /// Generic check-box column for property.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class GridCheckBoxColumn<TModel> : GridColumn<TModel, bool>
    {
        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            return HtmlHelper.CheckBoxFor(ColumnExpression, GetHtmlAttributes()).ToHtmlString();
        }
    }

    /// <summary>
    /// Generic drop down column for property.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class GridDropDownColumn<TModel, TValue> : GridColumn<TModel, TValue>
    {
        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            return HtmlHelper.DropDownListFor(ColumnExpression, GetHtmlAttributes()).ToHtmlString();
        }
    }

    /// <summary>
    /// Generic RadioButton column for property.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class GridRadioButtonColumn<TModel, TValue> : GridColumn<TModel, TValue>
    {
        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            return HtmlHelper.LabelRadioButtonFor(ColumnExpression, GetHtmlAttributes()).ToHtmlString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class GridPartialColumn<TModel> : GridColumnBase<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridPartialColumn{TModel}"/> class.
        /// </summary>
        /// <param name="partialViewName">Partial name of the view.</param>
        public GridPartialColumn(string partialViewName)
        {
            PartialViewName = partialViewName;
        }

        /// <summary>
        /// Gets the partial name of the view.
        /// </summary>
        /// <value>
        /// The partial name of the view.
        /// </value>
        public string PartialViewName { get; private set; }

        /// <summary>
        /// Renders the column data.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            return HtmlHelper.PartialFor(PartialViewName, "").ToHtmlString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class GridPartialColumn<TModel, TValue> : GridColumn<TModel, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridPartialColumn{TModel, TValue}"/> class.
        /// </summary>
        /// <param name="partialViewName">Partial name of the view.</param>
        public GridPartialColumn(string partialViewName)
        {
            PartialViewName = partialViewName;
        }

        /// <summary>
        /// Gets the partial name of the view.
        /// </summary>
        /// <value>
        /// The partial name of the view.
        /// </value>
        public string PartialViewName { get; private set; }

        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            return HtmlHelper.PartialFor(ColumnExpression, PartialViewName).ToHtmlString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class GridActionLinkColumn<TModel> : GridColumnBase<TModel>
    {
        /// <summary>
        /// Gets or sets the link text.
        /// </summary>
        /// <value>
        /// The link text.
        /// </value>
        public string LinkText { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>
        /// The route values.
        /// </value>
        public object RouteValues { get; set; }

        /// <summary>
        /// Gets or sets the route values factory.
        /// </summary>
        /// <value>
        /// The route values factory.
        /// </value>
        public Func<TModel, object> RouteValuesFactory { get; set; }

        /// <summary>
        /// Renders the column data.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            var routValues = RouteValuesFactory != null ? RouteValuesFactory(Model) : RouteValues;

            return HtmlHelper.ActionLink(LinkText, ActionName, ControllerName,
                new RouteValueDictionary(routValues),
                GetHtmlAttributes())
                .ToHtmlString();
        }
    }

    #endregion
}