using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MVCControls.Extensions
{
    //[assembly: InternalsVisibleTo("MVCControlsExtensions.UnitTests")]
    /// <summary>
    /// Client-side ViewModel Builder
    /// </summary>
    public class ViewModelBuilder
    {
        private readonly IViewModelGenerator _viewModelGenerator;

        internal static readonly ConcurrentDictionary<Type, ViewModelClass> ViewModelDictionary =
            new ConcurrentDictionary<Type, ViewModelClass>();

        // Change this list to include any non-primitive types you think should be eligible for display/edit
        private static readonly Type[] BindableNonPrimitiveTypes =
        {
            typeof (string),
            typeof (decimal),
            typeof (Guid),
            typeof (DateTime),
            typeof (DateTimeOffset),
            typeof (TimeSpan)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBuilder"/> class.
        /// </summary>
        /// <param name="viewModelGenerator">The view model generator.</param>
        public ViewModelBuilder(IViewModelGenerator viewModelGenerator)
        {
            _viewModelGenerator = viewModelGenerator;
        }

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="nameFactory">The name factory.</param>
        public static void BuildViewModel(Type type, Func<Type, string> nameFactory = null)
        {
            if (type == null || ViewModelDictionary.ContainsKey(type))
                return;
            var typeProperties = GetOrderedProperties(type);

            var vmClass = new ViewModelClass
            {
                Name = nameFactory == null ? type.Name : nameFactory(type),
                Properties = new List<ViewModelProperty>()
            };

            foreach (var property in typeProperties)
            {
                var vmProperty = new ViewModelProperty
                {
                    Name = property.Name,
                    UnderlyingType = property.PropertyType,
                    IsClass = property.PropertyType.IsClass,
                    IsArray = property.PropertyType.IsArray,
                    IsGenericType = property.PropertyType.IsGenericType,
                };

                vmProperty.UnderlyingType = vmProperty.IsGenericType
                    ? property.PropertyType.GetGenericTypeArgumentsTypes()[0]
                    : property.PropertyType;

                if (vmProperty.UnderlyingType.IsPrimitive ||
                    BindableNonPrimitiveTypes.Contains(vmProperty.UnderlyingType))
                {
                    vmProperty.IsClass = false;
                    //do nothing to avoid another mapping.
                }
                    //List or collection 
                else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    vmProperty.IsArray = true;
                    if (property.PropertyType.HasElementType)
                    {
                        vmProperty.UnderlyingType = property.PropertyType.GetElementType();
                        if (!property.PropertyType.IsPrimitive &&
                            !BindableNonPrimitiveTypes.Contains(vmProperty.UnderlyingType))
                        {
                            BuildViewModel(vmProperty.UnderlyingType, nameFactory);
                        }
                        else
                        {
                            vmProperty.IsClass = false;
                        }
                    }
                    else if (vmProperty.IsGenericType &&
                             !property.PropertyType.IsPrimitive &&
                             !BindableNonPrimitiveTypes.Contains(vmProperty.UnderlyingType))
                    {
                        BuildViewModel(vmProperty.UnderlyingType, nameFactory);
                    }
                    else
                    {
                        vmProperty.IsClass = false;
                    }
                }
                    // classes or interface
                else if (property.PropertyType.IsClass)
                {
                    BuildViewModel(vmProperty.UnderlyingType, nameFactory);
                }
                else if (!vmProperty.UnderlyingType.IsPrimitive &&
                         !BindableNonPrimitiveTypes.Contains(vmProperty.UnderlyingType))
                {
                    vmProperty.IsUnknown = true;
                }

                vmClass.Properties.Add(vmProperty);
            }

            ViewModelDictionary.TryAdd(type, vmClass);
        }

        /// <summary>
        /// Generates the view model.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GenerateViewModel(Type type)
        {
            var viewModelsb = new StringBuilder(1024);
            var generatedVms = new Dictionary<Type, bool>();
            GenerateViewModel(type, viewModelsb, generatedVms, 0);
            var generatedVm = ViewModelDictionary[type];

            viewModelsb.Insert(0, _viewModelGenerator.BeginScripts(generatedVm.Name));
            viewModelsb.Append(_viewModelGenerator.EndScripts(generatedVm.Name));

            return viewModelsb.ToString();
        }

        private void GenerateViewModel(Type type, StringBuilder stringBuilder,Dictionary<Type, bool> generatedVms, int noOfTabs)
        {
            //avoid duplicate script
            if (generatedVms.ContainsKey(type))
            {
                return;
            }
            generatedVms.Add(type, true);
            
            //build metadata if not generate before.
            if (!ViewModelDictionary.ContainsKey(type))
            {
                BuildViewModel(type, t => t.Name);
            }
            var currentTabs = noOfTabs + 1;
            var generatedVm = ViewModelDictionary[type];

            foreach (var property in generatedVm.Properties.Where(x => x.IsClass || x.IsArray))
            {
                if (ViewModelDictionary.ContainsKey(property.UnderlyingType))
                {
                    var generateVmString = new StringBuilder();
                    GenerateViewModel(property.UnderlyingType, generateVmString, generatedVms, noOfTabs);
                    stringBuilder.Append(generateVmString);
                }
            }

            stringBuilder.Append(_viewModelGenerator.GetClassStart(generatedVm.Name, currentTabs));

            GenerateProperties(generatedVm, stringBuilder, currentTabs);

            GenerateUpdateMethod(generatedVm, stringBuilder, currentTabs + 1);

            GenerateCrudMethods(generatedVm, stringBuilder, currentTabs+1);

            stringBuilder.Append(_viewModelGenerator.GetClassEnd(generatedVm.Name, currentTabs));
        }

        private void GenerateProperties(ViewModelClass generatedVm, StringBuilder stringBuilder, int currentTabs)
        {
            var propertyTabls = currentTabs + 1;
            foreach (var property in generatedVm.Properties)
            {
                if (property.IsArray)
                {
                    stringBuilder.Append(_viewModelGenerator.GetArrayProperty(property.Name, propertyTabls));
                }
                else if (property.IsClass)
                {
                    var propertVm = ViewModelDictionary[property.UnderlyingType];
                    stringBuilder.Append(_viewModelGenerator.GetClassProperty(property.Name, propertVm.Name, propertyTabls));
                } 
                else
                {
                    stringBuilder.Append(_viewModelGenerator.GetProperty(property.Name, propertyTabls));
                }
            }
        }

        private void GenerateUpdateMethod(ViewModelClass generatedVm, StringBuilder stringBuilder, int noOfTabs)
        {
            stringBuilder.Append(_viewModelGenerator.GetUpdatePropertiesStart(noOfTabs));

            var updatePropertyTabls = noOfTabs + 1;

            foreach (var property in generatedVm.Properties)
            {
                if (property.IsArray)
                {
                    stringBuilder.Append(_viewModelGenerator.GetArrayPropertyUpdate(property.Name, updatePropertyTabls));
                } 
                else if (property.IsClass)
                {
                    stringBuilder.Append(_viewModelGenerator.GetClassPropertyUpdate(property.Name, updatePropertyTabls));
                }
                else
                {
                    var defaultValue = _viewModelGenerator.GetDefaultValue(property.UnderlyingType);
                    stringBuilder.Append(_viewModelGenerator.GetPropertyUpdate(property.Name, defaultValue,
                        updatePropertyTabls));
                }
            }

            stringBuilder.Append(_viewModelGenerator.GetUpdatePropertiesEnd(noOfTabs));
        }

        private void GenerateCrudMethods(ViewModelClass generatedVm, StringBuilder stringBuilder, int noOfTabs)
        {
            foreach (var property in generatedVm.Properties.Where(x => x.IsArray))
            {
                var generatedSubVm = ViewModelDictionary[property.UnderlyingType];

                stringBuilder.Append(_viewModelGenerator.GetAddFunction(property.Name, generatedSubVm.Name, noOfTabs));
                stringBuilder.Append(_viewModelGenerator.GetArrayPropertyUpdateMethod(property.Name, generatedSubVm.Name, noOfTabs));
                stringBuilder.Append(_viewModelGenerator.GetRemoveFunction(property.Name, generatedSubVm.Name, noOfTabs));
            }
        }

        private static IEnumerable<PropertyInfo> GetOrderedProperties(Type type)
        {
            var properties = type.GetProperties();
            var orderedList = properties
                .Select(pinfo => new OrderedPropertyInfo
                {
                    PropertyInfo = pinfo,
                    Order =
                        !pinfo.PropertyType.IsValueType && pinfo.PropertyType != typeof(string)
                            ? (typeof(IEnumerable).IsAssignableFrom(pinfo.PropertyType) ? 1 : 2)
                            : 3
                })
                .ToList();

            return orderedList
                //.OrderBy(x => x.Order)
                .Select(x => x.PropertyInfo).ToList();
        }


        internal class OrderedPropertyInfo
        {
            public int Order { get; set; }
            public PropertyInfo PropertyInfo { get; set; }
        }
    }

}