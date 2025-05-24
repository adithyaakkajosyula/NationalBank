using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AdithyaBank.BackEnd.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredIfAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _valueoperator;

        private string PropertyName { get; set; }
        private string _errorMessage { get; set; }
        private object DesiredValue { get; set; }

        public RequiredIfAttribute(string propertyName, object desiredvalue, string valueoperator = "=")
            : base("{0} is invalid")
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            _valueoperator = valueoperator;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();
            object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            //&& value is string ? string.IsNullOrEmpty(Convert.ToString(value)) : Convert.ToInt64(value) <= 0
            if (_valueoperator.Equals("="))
            {
                if (proprtyvalue.ToString() == DesiredValue.ToString())
                {
                    if (value is string)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(value)))
                        {
                            string errorMessage = FormatErrorMessage(context.DisplayName);
                            return new ValidationResult(errorMessage);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt64(value) <= 0)
                        {
                            string errorMessage = FormatErrorMessage(context.DisplayName);
                            return new ValidationResult(errorMessage);
                        }
                    }

                }
            }
            else if (_valueoperator.Equals("!="))
            {
                if (proprtyvalue.ToString() != DesiredValue.ToString())
                {
                    if (value is string)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(value)))
                        {
                            string errorMessage = FormatErrorMessage(context.DisplayName);
                            return new ValidationResult(errorMessage);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt64(value) <= 0)
                        {
                            string errorMessage = FormatErrorMessage(context.DisplayName);
                            return new ValidationResult(errorMessage);
                        }
                    }

                }
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            string errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-requiredif", errorMessage);
        }

        private bool MergeAttribute(
       IDictionary<string, string> attributes,
       string key,
       string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}
