using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MVCControls.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class KOViewModelBuilder
    {
        /// <summary>
        /// The knockout view model string
        /// </summary>
        public const string KnockoutViewModelString = "KnockOutViewModel";

        /// <summary>
        /// Build Knockout view model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static void KOBuildViewModel<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            var modelName = htmlHelper.GetKOModelName();

            BuildViewModel(htmlHelper.ViewData.Model, modelName);
        }

        /// <summary>
        /// Builds the Knockout view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="name">The name.</param>
        public static void BuildViewModel(object model, string name = "model")
        {
            if (model == null || KnockoutViewModelDictionary.ContainsKey(name))
                return;

            var modelScript = new StringBuilder(string.Format("\n\tvar {0} = function () {{\n\t\tvar self = this;", name));
            var modelUpdateScript = new StringBuilder("\n\t\tself.Update = function (data){");
            modelUpdateScript.Append("\n\tif (data==null) return;");

            var modelAddUpdScript = new StringBuilder();

            foreach (var pinfo in model.GetType().GetProperties())
            {
                var koPropertyName = "ko.observable()";
                var updateObservableFormat = string.Empty;
                var newObservableFormat = string.Empty;

                if (pinfo.PropertyType.IsClass && pinfo.PropertyType != typeof(string))
                {
                    if (pinfo.PropertyType.IsGenericType)
                    {
                        //var genericParamType = pinfo.PropertyType.GenericTypeArguments[0];
                        var genericParamType = pinfo.PropertyType.GetGenericTypeArgumentsTypes()[0];
                        var genericParamobj = Activator.CreateInstance(genericParamType);
                        var viewModelName = genericParamType.Name.Replace("ViewModel", "");
                        BuildViewModel(genericParamobj, viewModelName);

                        var typeObj = Activator.CreateInstance(pinfo.PropertyType);

                        if (typeObj is IEnumerable)
                        {
                            koPropertyName = string.Format("ko.observableArray([new {0}()])", viewModelName);
                            modelAddUpdScript.AppendFormat("\n\t\tself.add{1} = function() {{\n\t\t\tself.{0}.push(new {1}());\n\t\t\t }};",
                                                           pinfo.Name, viewModelName);
                            modelAddUpdScript.AppendFormat("\n\t\tself.remove{1} = function(line) {{\n\t\t\t self.{0}.remove(line);\n\t\t\t }};",
                                                           pinfo.Name, viewModelName);

                            var newObserverObj =
                                string.Format(
                                    "\n\t\t\t\t var obj =data.{0}[i]; var newObserver = new {1}();\n\t\t\t\tnewObserver.Update(obj);\n\t\t\t\tself.{0}.push(newObserver);",
                                    pinfo.Name, viewModelName);

                            var forUpdateLoop = string.Format("\n\t\t\tfor (var i=0;data.{0} != null && i<data.{0}.length;i++) {{\n\t\t\t {1} \n\t\t\t}}",
                                                              pinfo.Name, newObserverObj);
                            updateObservableFormat = string.Format("\n\t\t\tself.{0}.removeAll();", pinfo.Name) + forUpdateLoop;
                        }
                    }
                    else
                    {
                        var genericParamobj = Activator.CreateInstance(pinfo.PropertyType);
                        var viewModelName = pinfo.PropertyType.Name.Replace("ViewModel", "");
                        koPropertyName = string.Format("new {0}()", viewModelName);
                        updateObservableFormat = string.Format("\n\t\t\tself.{0}.Update(data.{0});", pinfo.Name);
                        BuildViewModel(genericParamobj, viewModelName);
                    }
                }
                else if (pinfo.PropertyType.IsValueType && (pinfo.PropertyType == typeof(DateTime) || pinfo.PropertyType == typeof(DateTime?)))
                {
                    koPropertyName = "ko.observable(new Date())";
                    updateObservableFormat = string.Format("\n\t\t\tself.{0}(jsonDateToDate(data.{0}));", pinfo.Name);
                }

                modelScript.Append(string.IsNullOrEmpty(newObservableFormat)
                                       ? string.Format("\n\t\tself.{0} = {1};", pinfo.Name, koPropertyName)
                                       : newObservableFormat);

                modelUpdateScript.Append(string.IsNullOrEmpty(updateObservableFormat)
                                             ? string.Format("\n\t\t\tself.{0}(data.{0});", pinfo.Name)
                                             : updateObservableFormat);
            }

            modelUpdateScript.Append("\n\t\t}");

            modelScript.Append(modelUpdateScript);
            modelScript.Append(modelAddUpdScript);

            modelScript.Append("\n\t}");

            KnockoutViewModelDictionary.Add(name, modelScript);

        }

        /// <summary>
        /// Gets the ListView model json.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="selectListItems">The select list items.</param>
        /// <returns></returns>
        public static StringBuilder GetListViewModelJson(string propertyName, IEnumerable<SelectListItem> selectListItems)
        {
            var koViewModel = new StringBuilder(string.Format("\n\t\t{0}.{1}List = [", KnockoutViewModelString, propertyName));

            foreach (var listItem in selectListItems)
            {
                koViewModel.Append('{');
                koViewModel.AppendFormat("Value:'{0}', Text:'{1}',", listItem.Value, listItem.Text);
                koViewModel.Append("},");
            }
            koViewModel.Append("];");
            return koViewModel;
        }

        /// <summary>
        /// Generate Knockout view model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="applyBindings">if set to <c>true</c> [apply bindings].</param>
        /// <returns></returns>
        public static MvcHtmlString KOGenerateViewModel<TModel>(this HtmlHelper<TModel> htmlHelper,
                                                                bool applyBindings = false)
        {
            if (!htmlHelper.ViewContext.HttpContext.Items.Contains(KnockoutViewModelString))
                return MvcHtmlString.Empty;

            var modelName = htmlHelper.GetKOModelName();

            var knockoutViewModelDictionary = GetKnockoutViewModelDictionary(htmlHelper);

            if (knockoutViewModelDictionary == null)
                return MvcHtmlString.Empty;

            var koViewModelScript =
                new StringBuilder(string.Format("\n\tif ({0} === undefined) {{\n\t var {0} = {{}};\n\t}}",
                                                KnockoutViewModelString));

            foreach (var kp in knockoutViewModelDictionary)
            {
                koViewModelScript.Append(kp.Value);
            }

            koViewModelScript.AppendFormat("\n\t{0}.{1} = new {1}();\n", KnockoutViewModelString, modelName);
            if (applyBindings)
            {
                koViewModelScript.Append(
                    string.Format("$(document).ready(function () {{\n\tko.applyBindings({0});\n\t}});",
                                  KnockoutViewModelString));
            }
            return MvcHtmlString.Create(koViewModelScript.ToString());
        }


        /// <summary>
        /// Gets the knockout view model dictionary.
        /// </summary>
        /// <value>
        /// The knockout view model dictionary.
        /// </value>
        public static Dictionary<string, StringBuilder> KnockoutViewModelDictionary
        {
            get
            {
                var httpContext = HttpContext.Current;
                if (!httpContext.Items.Contains(KnockoutViewModelString))
                {
                    var objectDictionary = new Dictionary<string, StringBuilder>();
                    httpContext.Items.Add(KnockoutViewModelString, objectDictionary);
                    return objectDictionary;
                }
                return httpContext.Items[KnockoutViewModelString] as Dictionary<string, StringBuilder>;

            }
        }

        #region private methods and properties

        private static Dictionary<string, StringBuilder> GetKnockoutViewModelDictionary<TModel>(HtmlHelper<TModel> htmlHelper)
        {
            if (!htmlHelper.ViewContext.HttpContext.Items.Contains(KnockoutViewModelString))
            {
                var objectDictionary = new Dictionary<string, StringBuilder>();
                htmlHelper.ViewContext.HttpContext.Items.Add(KnockoutViewModelString, objectDictionary);
                return objectDictionary;
            }
            return htmlHelper.ViewContext.HttpContext.Items[KnockoutViewModelString] as Dictionary<string, StringBuilder>;
        }

        private const string RenderingViewModelString = "RenderingViewModel";

        /// <summary>
        /// Gets or sets the rendering view model.
        /// </summary>
        /// <value>
        /// The rendering view model.
        /// </value>
        public static string RenderingViewModel
        {
            get
            {
                var httpContext = HttpContext.Current;
                if (httpContext.Items.Contains(RenderingViewModelString))
                {
                    return httpContext.Items[RenderingViewModelString] as string;
                }
                return string.Empty;
            }
            set
            {
                var httpContext = HttpContext.Current;
                if (!httpContext.Items.Contains(RenderingViewModelString))
                {
                    httpContext.Items.Add(RenderingViewModelString, value);
                }
                else
                {
                    httpContext.Items[RenderingViewModelString] = value;
                }
            }
        }
        
        /// <summary>
        /// Gets the name of the ko model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <returns></returns>
        public static string GetKOModelName<TModel>(this HtmlHelper<TModel> helper)
        {
            if (string.IsNullOrEmpty(RenderingViewModel))
            {
                RenderingViewModel = helper.ViewData.Model.GetType().Name.Replace("ViewModel", "");
            }

            return RenderingViewModel;
        }

        #endregion

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