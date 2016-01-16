using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCControls.Core
{
    /// <summary>
    /// Flexible Validation Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class FlexibleValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _propertName;
        private readonly string _parameters;
        private readonly string _clientSideValidationMethodName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexibleValidationAttribute"/> class.
        /// </summary>
        /// <param name="propertName">Name of the property.</param>
        /// <param name="clientSideValidationMethodName">Name of the client side validation method.</param>
        /// <param name="parameters">The parameters.</param>
        public FlexibleValidationAttribute(string propertName, string clientSideValidationMethodName = null, string parameters = null)
        {
            _propertName = propertName;
            _parameters = parameters;
            _clientSideValidationMethodName = clientSideValidationMethodName;
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = validationContext.ObjectInstance as IFlexibleValidation;
            if (viewModel == null || viewModel.FlexibleValidation(_propertName, _parameters))
                return ValidationResult.Success;

            return new ValidationResult(String.Format(ErrorMessageString, validationContext.DisplayName));
        }

        /// <summary>
        /// When implemented in a class, returns client validation rules for that class.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The controller context.</param>
        /// <returns>
        /// The client validation rules for this validator.
        /// </returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "flexiblevalidation"
            };

            rule.ValidationParameters["fieldname"] = _propertName ?? metadata.PropertyName;

            rule.ValidationParameters["validationmethodname"] = _clientSideValidationMethodName ?? "validateFlexibleValidation";

            if (!string.IsNullOrEmpty(_parameters))
            {
                rule.ValidationParameters["parameters"] = _parameters;
            }
            yield return rule;
        }
    }

    /// <summary>
    /// Flexible Validation interface
    /// </summary>
    public interface IFlexibleValidation
    {

        /// <summary>
        /// Flexible the validation.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        bool FlexibleValidation(string propertyName, string parameters = null);
    }
}
