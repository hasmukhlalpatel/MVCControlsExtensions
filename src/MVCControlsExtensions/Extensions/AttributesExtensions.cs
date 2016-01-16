using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCControls.Extensions
{
    /// <summary>
    /// Attributes Extensions methods.
    /// </summary>
    public static class AttributesExtensions
    {
        /// <summary>
        /// Gets the HTML control attribute list.
        /// </summary>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static RouteValueDictionary GetAttributeList(object htmlAttributes)
        {
            return htmlAttributes != null
                       ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
                       : new RouteValueDictionary();
        }

        /// <summary>
        /// Converts the anonymous object to HTML attributes.
        /// </summary>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static string ConvertAnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            return ConvertDictionaryToAttributes(GetAttributeList(htmlAttributes));
        }

        /// <summary>
        /// Converts the dictionary to attributes.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static string ConvertDictionaryToAttributes(IDictionary<string, object> dictionary)
        {
            if (dictionary != null && dictionary.Any())
            {
                return dictionary.Aggregate(" ", (c, att) => c + String.Format("{0}=\"{1}\"", att.Key, att.Value));
            }
            return String.Empty;
        }

        /// <summary>
        /// Convert the Html attributes dictionary To the HTML attributes string.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static string ToHtmlAttributes(this RouteValueDictionary dictionary)
        {
            return ConvertDictionaryToAttributes(dictionary);
        }

        /// <summary>
        /// Adds attribute if the atribute does not exist.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddIfNot(this IDictionary<string, object> dictionary, string key, object value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key,value);
            }
        }
    }
}