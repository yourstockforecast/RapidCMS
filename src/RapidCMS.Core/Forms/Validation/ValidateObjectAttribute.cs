﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RapidCMS.Core.Forms.Validation
{
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }

            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var compositeResults = new CompositeValidationResult($"Validation for {validationContext.DisplayName} failed!", validationContext.MemberName ?? "Unknown member");
                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success!;
        }
    }
}
