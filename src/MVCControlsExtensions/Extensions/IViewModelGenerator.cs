using System;

namespace MVCControls.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IViewModelGenerator
    {
        /// <summary>
        /// Begin Scripts
        /// </summary>
        /// <param name="name"></param>
        /// <param name="noOfTabs"></param>
        /// <returns></returns>
        string BeginScripts(string name, int noOfTabs = 1);

        /// <summary>
        /// End Scripts
        /// </summary>
        /// <param name="name"></param>
        /// <param name="noOfTabs"></param>
        /// <returns></returns>
        string EndScripts(string name, int noOfTabs = 1);

        /// <summary>
        /// Gets the class start.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetClassStart(string name, int noOfTabs = 1);
        /// <summary>
        /// Gets the class end.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetClassEnd(string name, int noOfTabs = 1);

        /// <summary>
        /// Gets the class constructor.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetClassConstructor(string name, int noOfTabs = 1);

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        string GetDefaultValue(Type type);

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetProperty(string name, int noOfTabs = 1);
        /// <summary>
        /// Gets the class property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetClassProperty(string name, string className, int noOfTabs = 1);
        /// <summary>
        /// Gets the array property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetArrayProperty(string name, int noOfTabs = 1);
        /// <summary>
        /// Gets the update properties start.
        /// </summary>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetUpdatePropertiesStart(int noOfTabs = 1);
        /// <summary>
        /// Gets the update properties end.
        /// </summary>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetUpdatePropertiesEnd(int noOfTabs = 1);

        /// <summary>
        /// Gets the property update.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetPropertyUpdate(string name, string defaultValue, int noOfTabs = 1);
        /// <summary>
        /// Gets the class property update.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetClassPropertyUpdate(string name, int noOfTabs = 1);
        /// <summary>
        /// Gets the array property update.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetArrayPropertyUpdate(string name, int noOfTabs = 1);
        /// <summary>
        /// Gets the array property update method.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetArrayPropertyUpdateMethod(string name, string className, int noOfTabs = 1);

        /// <summary>
        /// Gets the add function.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetAddFunction(string name, string className, int noOfTabs = 1);
        /// <summary>
        /// Gets the remove function.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="noOfTabs">The no of tabs.</param>
        /// <returns></returns>
        string GetRemoveFunction(string name, string className, int noOfTabs = 1);
    }
}