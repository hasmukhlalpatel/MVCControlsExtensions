using System;

namespace MVCControls.Extensions
{
    /// <summary>
    /// Knockout ViewModel Generator
    /// </summary>
    public class KoViewModelGenerator : IViewModelGenerator
    {
        private const string AsciiTabs = "\t";
        private const string Self = "self";
        private const string ClassStart = "{0}var {1} = function(){{{0} var {2} = this;";
        private const string ClassEnd = "{0}}};";

        private const string KOPropertyName = "{0}{1}.{2} = ko.observable();";
        private const string KOClassPropertyName = "{0}{1}.{2} = new {3}();";
        private const string KOArrayPropertyName = "{0}{1}.{2} = ko.observableArray();";

        private const string KOUpdatePropertiesStart = "{0}{1}.Update = function (data){{";
        private const string KOUpdatePropertiesEnd = "{0}}};";
        private const string KOPropertyUpdate = "{0}{1}.{2}((data == null? {3} : (data.{2} == null ? {3}: data.{2})));";
        private const string KOClassPropertyUpdate = "{0}{1}.{2}.Update(data == null? null : (data.{2} == null ? null: data.{2}));";
        private const string KOArrayPropertyUpdate = "{0}{1}.Update{2}(data == null? null : (data.{2} == null ? null: data.{2}));";

        private const string KOUpdateLoopScript = @"
        {0}{1}.Update{2} = function (data){{
            {0}    {1}.{2}.removeAll();
            {0}    if(data != null){{
            {0}        for (var i=0;i<data.length;i++){{
            {0}             var obsObj = new {3}();
            {0}             obsObj.Update(data[i]);
            {0}             {1}.{2}.push(obsObj);
            {0}         }}
            {0}    }}
        {0}}};";

        private const string KOAddScript = @"
        {0}{1}.add{3} = function(){{
            {0}{1}.{2}.push(new {3}());
        {0}}};";
        private const string KORemoveScript = @"
        {0}{1}.remove{3} = function(line){{
            {0}{1}.{2}.remove(line);
        {0}}};";

        private const string StartScript = "{0}/*Automatic generated script of {1}*/";
        private const string EndScript = "\n\tif ({0} === undefined) {{\n\t var {0} = {{}};\n\t}}\n\t{0}.{1} = new {1}();";

        private const string KnockoutViewModelString = "KnockOutViewModel";

        /// <summary>
        /// Gets the tabs.
        /// </summary>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        private static string GetTabs(int noOfTabs = 1)
        {
            return string.Format("\n{0}", AsciiTabs.PadLeft(noOfTabs, '\t'));
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetDefaultValue(Type type)
        {
            if (type == typeof(string))
                return "''";
            if (type == typeof(DateTime))
                return "new Date()";
            if (type == typeof(bool))
                return "false";
            return type.IsValueType
                ? Activator.CreateInstance(type).ToString()
                : "''";
        }

        /// <summary>
        /// Begin Scripts
        /// </summary>
        /// <param name="name"></param>
        /// <param name="noOfTabs"></param>
        /// <returns></returns>
        public string BeginScripts(string name, int noOfTabs = 1)
        {
            return string.Format(StartScript, AsciiTabs.PadLeft(noOfTabs, '\t'), name); 
        }

        /// <summary>
        /// End Scripts
        /// </summary>
        /// <param name="name"></param>
        /// <param name="noOfTabs"></param>
        /// <returns></returns>
        public string EndScripts(string name, int noOfTabs = 1)
        {
            return string.Format(EndScript, KnockoutViewModelString, name);
        }



        /// <summary>
        /// Gets the class start.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetClassStart(string name, int noOfTabs = 1)
        {
            return string.Format(ClassStart, GetTabs(noOfTabs), name, Self);
        }

        /// <summary>
        /// Gets the class end.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetClassEnd(string name, int noOfTabs = 1)
        {
            return string.Format(ClassEnd, GetTabs(noOfTabs));
        }

        /// <summary>
        /// Gets the class constructor.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetClassConstructor(string name, int noOfTabs = 1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetProperty(string name, int noOfTabs = 1)
        {
            return string.Format(KOPropertyName, GetTabs(noOfTabs), Self, name);
        }

        /// <summary>
        /// Gets the class property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetClassProperty(string name, string className, int noOfTabs = 1)
        {
            return string.Format(KOClassPropertyName, GetTabs(noOfTabs), Self, name, className);
        }

        /// <summary>
        /// Gets the array property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetArrayProperty(string name, int noOfTabs = 1)
        {
            return string.Format(KOArrayPropertyName, GetTabs(noOfTabs), Self, name);
        }

        /// <summary>
        /// Gets the update properties start.
        /// </summary>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetUpdatePropertiesStart(int noOfTabs = 1)
        {
            return string.Format(KOUpdatePropertiesStart, GetTabs(noOfTabs), Self);
        }

        /// <summary>
        /// Gets the update properties end.
        /// </summary>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetUpdatePropertiesEnd(int noOfTabs = 1)
        {
            return string.Format(KOUpdatePropertiesEnd, GetTabs(noOfTabs));
        }

        /// <summary>
        /// Gets the property update.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetPropertyUpdate(string name, string defaultValue, int noOfTabs = 1)
        {
            return string.Format(KOPropertyUpdate, GetTabs(noOfTabs), Self, name, defaultValue);
        }

        /// <summary>
        /// Gets the class property update.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetClassPropertyUpdate(string name, int noOfTabs = 1)
        {
            return string.Format(KOClassPropertyUpdate, GetTabs(noOfTabs), Self, name);
        }

        /// <summary>
        /// Gets the array property update.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetArrayPropertyUpdate(string name, int noOfTabs = 1)
        {
            return string.Format(KOArrayPropertyUpdate, GetTabs(noOfTabs), Self, name);
        }

        /// <summary>
        /// Gets the array property update method.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetArrayPropertyUpdateMethod(string name, string className, int noOfTabs = 1)
        {
            return string.Format(KOUpdateLoopScript, AsciiTabs.PadLeft(noOfTabs, '\t'), Self, name, className);
        }

        /// <summary>
        /// Gets the add function.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetAddFunction(string name, string className, int noOfTabs = 1)
        {
            return string.Format(KOAddScript, AsciiTabs.PadLeft(noOfTabs, '\t'), Self, name, className);
        }

        /// <summary>
        /// Gets the remove function.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        public string GetRemoveFunction(string name, string className, int noOfTabs = 1)
        {
            return string.Format(KORemoveScript, AsciiTabs.PadLeft(noOfTabs, '\t'), Self, name, className);
        }

    }
}