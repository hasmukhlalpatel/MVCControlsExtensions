using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MVCControls.Core;

namespace MVCControls.Extensions
{
    /// <summary>
    /// DataExtensions
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// Updates the lookup text based on specified lookup attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        public static void UpdateLookupText<T>(T model)
            where T : class
        {
            var properties = model.GetType().GetProperties();

            foreach (var pinfo in properties)
            {
                if (pinfo.PropertyType.IsClass && pinfo.PropertyType != typeof(string))
                {
                    var value = pinfo.GetValue(model);

                    if (value is IEnumerable)
                    {
                        var list = value as IEnumerable;
                        foreach (var obj in list)
                        {
                            UpdateLookupText(obj);
                        }
                    }
                    else if (value != null)
                    {
                        UpdateLookupText(value);
                    }
                }
                else
                {
                    var lookupTextAttrb = pinfo.GetCustomAttributes(typeof(LookupTextAttribute))
                                               .ToList()
                                               .FirstOrDefault() as LookupTextAttribute;

                    if (lookupTextAttrb != null)
                    {
                        var lookuppInfo = properties
                            .FirstOrDefault(x => x.Name == lookupTextAttrb.LookupIdPropertyName);

                        if (lookuppInfo != null)
                        {
                            var lookupValue = lookuppInfo.GetValue(model);
                            if (lookupValue != null)
                            {
                                var lookItems = ControlExtensions.GetLookupItems(lookuppInfo);

                                var lookupText = lookItems
                                    .FirstOrDefault(x => x.Id == lookupValue.ToString() || x.Key == lookupValue.ToString());

                                if (lookupText != null) pinfo.SetValue(model, lookupText.Description);   
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// conver string to the Pascal case to string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string PascalCaseToString(this string str, string replacement = " ")
        {
            var regex = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");
            return regex.Replace(str, replacement);
        }
    }
}