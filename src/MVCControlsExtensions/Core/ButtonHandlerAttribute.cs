using System;
using System.Reflection;
using System.Web.Mvc;

namespace MVCControls.Core
{
    /// <summary>
    /// Multiple submit button handler. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonHandlerAttribute : ActionNameSelectorAttribute
    {
        /// <summary>
        /// Gets or sets the match form key.
        /// </summary>
        /// <value>
        /// The match form key.
        /// </value>
        public string MatchFormKey { get; set; }
        /// <summary>
        /// Gets or sets the match form value.
        /// </summary>
        /// <value>
        /// The match form value.
        /// </value>
        public string MatchFormValue { get; set; }

        /// <summary>
        /// Determines whether the action name is valid in the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="methodInfo">Information about the action method.</param>
        /// <returns>
        /// true if the action name is valid in the specified controller context; otherwise, false.
        /// </returns>
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(MatchFormKey) && string.IsNullOrEmpty(MatchFormValue))
            {
                return false;
            }
            if (string.IsNullOrEmpty(MatchFormKey))
            {
                return actionName == MatchFormValue;
            }
            if (string.IsNullOrEmpty(MatchFormValue))
            {
                return controllerContext.HttpContext.Request[MatchFormKey] != null;
            }
            return controllerContext.HttpContext.Request[MatchFormKey] != null &&
                   controllerContext.HttpContext.Request[MatchFormKey] == MatchFormValue;
        }
    }
}