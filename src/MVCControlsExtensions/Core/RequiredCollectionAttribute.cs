using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCControls.Core
{
    /// <summary>
    /// Required Collection validation Attribute to validate collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class RequiredCollectionAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _validateMethod;

        public RequiredCollectionAttribute(string validateMethod =null)
        {
            _validateMethod = validateMethod;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var list = value as IList;
                if (list != null && list.Count > 0)
                    return ValidationResult.Success;
            }
            return new ValidationResult(String.Format(ErrorMessageString, validationContext.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
                {
                    ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                    ValidationType = "requiredcollection"
                };
            rule.ValidationParameters["validatemethod"] = _validateMethod ?? ("validate" + metadata.PropertyName);
            yield return rule;
        }
    }
}