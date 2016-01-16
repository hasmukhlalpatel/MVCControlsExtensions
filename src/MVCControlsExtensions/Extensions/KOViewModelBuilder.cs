using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCControls.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class KOViewModelBuilder
    {
        internal static readonly ConcurrentDictionary<Guid, Type> ViewModelKeys = new ConcurrentDictionary<Guid, Type>();
        internal static readonly ConcurrentDictionary<Guid, string> ViewModels = new ConcurrentDictionary<Guid, string>();
        /// <summary>
        /// The knockout view model string
        /// </summary>
        public const string KnockoutViewModelString = "KnockOutViewModel";

        /// <summary>
        /// ViewModel Name Factory
        /// </summary>
        public static Func<Type, string> ViewModelNameFactory = t => t.Name.Replace("ViewModel", "");

        
        /// <summary>
        /// Render Knockout view model inline with view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static MvcHtmlString KOGenerateViewModel<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            var modelType = htmlHelper.ViewData.Model.GetType();
            BuildViewModelMetadata(modelType);

            return MvcHtmlString.Create(RenderKOViewModelScripts(modelType.GUID));
        }

        /// <summary>
        /// Render Knockout view model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static MvcHtmlString RenderKOViewModel<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            var modelType = htmlHelper.ViewData.Model.GetType();
            return RenderKOViewModel(htmlHelper, modelType);
        }

        //private const string ScriptTagMarkup = "<{0} src=\"{1}-{2}\"></{0}>";

        private const string ScriptTagMarkup = "<{0} src=\"{1}\"></{0}>";

        /// <summary>
        /// Render Knockout view model.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="modelType">the type of the model</param>
        public static MvcHtmlString RenderKOViewModel<TModel>(this HtmlHelper<TModel> htmlHelper, Type modelType)
        {
            BuildViewModelMetadata(modelType);
            var url = UrlHelper.GenerateUrl(null, null, null, new RouteValueDictionary(new {typeId = modelType.GUID}), htmlHelper.RouteCollection,
                htmlHelper.ViewContext.RequestContext, false);
            return MvcHtmlString.Create(string.Format(ScriptTagMarkup, "script", url));
            //return MvcHtmlString.Create(string.Format(ScriptTagMarkup, "script", "KOViewmodel", modelType.GUID));

        }

        private static void BuildViewModelMetadata(Type modelType)
        {
            if (!ViewModelKeys.ContainsKey(modelType.GUID))
            {
                ViewModelKeys.TryAdd(modelType.GUID, modelType);
            }

            ViewModelBuilder.BuildViewModel(modelType, ViewModelNameFactory);
        }

        /// <summary>
        /// Render Knockout view model's scripts.
        /// </summary>
        /// <param name="typeId">the type id of the model</param>
        public static string RenderKOViewModelScripts(Guid typeId)
        {
            if (!ViewModelKeys.ContainsKey(typeId))
            {
                throw new ArgumentException("Type id invalid");
            }
            if (ViewModels.ContainsKey(typeId)) return ViewModels[typeId];

            var koVmBuilder = new ViewModelBuilder(new KoViewModelGenerator());
            var scripts = koVmBuilder.GenerateViewModel(ViewModelKeys[typeId]);
            ViewModels.TryAdd(typeId, scripts);
            return scripts;
        }

        /// <summary>
        /// Generates the json data from model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MvcHtmlString GenerateJsonModel<TModel>(this HtmlHelper<TModel> htmlHelper, string model = "model")
        {
            const string jsonModelFormat = "var {0} = {1};";

            var jsonData = htmlHelper.Raw(Json.Encode(htmlHelper.ViewData.Model));

            return MvcHtmlString.Create(string.Format(jsonModelFormat, model, jsonData));

        }
    }
}