using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCControls.Core
{
    /// <summary>
    /// Required attachments validation attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AttachmentsAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value,
                                                    ValidationContext validationContext)
        {
            var files = value as IEnumerable<HttpPostedFileBase>;

            // files is null return valid because input was not displayed (hidden)
            if (files == null)
                return ValidationResult.Success;;

            // The file is required.
            if (files.Any(x => x == null))
            {
                return new ValidationResult("Please upload a file!");
            }

            // The maximum allowed file size is 10MB.
            if (files.Any(x => x.ContentLength > 10 * 1024 * 1024))
            {
                return new ValidationResult("This file is too big!");
            }
            return ValidationResult.Success;
        }
    }
}