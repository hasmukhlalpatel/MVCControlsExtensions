using System;

namespace MVCControls.Extensions
{
#if DOTNET4_0
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public static class CustomAttributeExtensions
    {
        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType)
        {
            return element.GetCustomAttributes(attributeType, true)
                .Select(x=> x as Attribute);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element) 
            where T : Attribute
        {
            return element.GetCustomAttributes(typeof (T), true)
                .Select(x => x as T);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static object GetValue(this PropertyInfo element, object model)
        {
            return element.GetValue(model, null);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(this PropertyInfo element, object obj, object value)
        {
            element.SetValue(obj, value, null);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class CustomExtensions
    {
        /// <summary>
        /// Get Generic Type Arguments Types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] GetGenericTypeArgumentsTypes(this Type type)
        {
            return type.GetGenericArguments();
        } 
       
    }

#endif

#if DOTNET4_5
    /// <summary>
    /// 
    /// </summary>
    public static class CustomExtensions
    {
        /// <summary>
        /// Get Generic Type Arguments Types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] GetGenericTypeArgumentsTypes(this Type type)
        {
            return type.GenericTypeArguments;
        }
    }

#endif
}
