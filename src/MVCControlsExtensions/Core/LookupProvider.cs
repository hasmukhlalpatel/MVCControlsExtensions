using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using MVCControls.Extensions;
using MVCControls.Models;

namespace MVCControls.Core
{
    /// <summary>
    /// Lookup provider for dropdown and radio controls.
    /// </summary>
    public class LookupProvider
    {
        /// <summary>
        /// The static cached lookup
        /// </summary>
        protected static readonly ConcurrentDictionary<Type, IList<Lookup>> StaticCachedLookup = new ConcurrentDictionary<Type, IList<Lookup>>();

        /// <summary>
        /// Gets the enum lookup.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static IEnumerable<Lookup> GetEnumLookup<T>(T enumType)
            where T : Type
        {
            return StaticCachedLookup.GetOrAdd(enumType, (x) => GetEnumLookup(x).ToList());
        }

        /// <summary>
        /// Gets the enum lookup.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static IEnumerable<Lookup> GetEnumLookup(Type enumType)
        {
            var returnList = new List<Lookup>();

            foreach (var fieldInfo in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                var lookup = new Lookup
                    {
                        Key = Enum.Parse(enumType, fieldInfo.Name).ToString(),
                        Id = ((int)Enum.Parse(enumType, fieldInfo.Name)).ToString(),
                    };
                lookup.Description = GetDescription(fieldInfo);
                returnList.Add(lookup);
            }

            return returnList;
        }

        /// <summary>
        /// Gets the enum lookup values.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static IEnumerable<Lookup> GetEnumLookupValues(Type enumType)
        {
            var returnList = new List<Lookup>();

            foreach (var fieldInfo in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                var lookup = new Lookup
                    {
                        Key = Enum.Parse(enumType, fieldInfo.Name).ToString(),
                        Id = Enum.Parse(enumType, fieldInfo.Name).ToString(),
                    };
                lookup.Description = GetDescription(fieldInfo);
                returnList.Add(lookup);
            }

            return returnList;
        }

        private static string GetDescription(FieldInfo fieldInfo)
        {
            var dscrAttributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>().ToList();
            return dscrAttributes.Any() ? dscrAttributes.First().Description : fieldInfo.Name;

//#if DOTNET4_5
//            var dscrAttributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>().ToList();
//            return dscrAttributes.Any() ? dscrAttributes.First().Description : fieldInfo.Name;
//#endif
//#if DOTNET4_0
//            var dscrAttributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>().ToList();
//            return dscrAttributes.Any() ? dscrAttributes.First().Description : fieldInfo.Name;
//#endif
        }

        /// <summary>
        /// Empties the lookup.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lookup> EmptyLookup()
        {
            return new List<Lookup>
                {
                    new Lookup {Id = "0", Description = "select..."}
                };
        }

    }
}