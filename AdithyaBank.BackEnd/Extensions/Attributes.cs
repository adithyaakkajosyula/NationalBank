using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Extensions
{
    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value) => this.StringValue = value;

        #endregion
    }
    public class CheckBoxRequired : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }

            return false;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val-checkboxrequired", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        }
        public class RegExpr
        {
            public const string GeneralName = "^[a-zA-Z]+[a-zA-Z \\s.,-/]*$";

            public const string UserName = "^[a-zA-Z]+[a-zA-Z0-9]*[.]?[a-zA-Z0-9]+$";

            public const string Password = "^[a-zA-Z]+[a-zA-Z0-9@_\\s.]*$";

            public const string Code = "^[a-zA-Z0-9]+$";

            public const string MobileNo = "^[6-9]{1}[0-9]{9}$";

            public const string EMail = "\\A(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)\\Z";

            public const string Numeric = "^[0-9]+$";

            public const string PinCode = "^[1-9]{1}[0-9]{5}$";

            public const string HouseNo = "^[a-zA-Z0-9]+[a-zA-Z0-9-/#,\\s]*$";

            public const string AadharCard = "^[1-9]{1}[0-9]{11}$";

            public const string PanCard = "^[A-Z]+[A-Z]+[A-Z]+[A-Z]+[A-Z]+[0-9]+[0-9]+[0-9]+[0-9]+[A-Z]$";

            public const string BankIFSCCode = "^[A-Z]+[A-Z]+[A-Z]+[A-Z]+[0]+[A-Z0-9]{6,6}$";

            public const string AlphaNumeric = "^[a-zA-Z]+[a-zA-Z0-9 \\s.,-]*$";

            public const string PersonName = "^[a-zA-Z]+[a-zA-Z.\\s]*$";

            public const string Alphabets = "^[A-Z]+[a-zA-Z]*$";

            public const string StreetOrColony = "^[a-zA-Z]+[a-zA-Z0-9 \\s.,-]*$";

            public const string MandalOrTaluka = "^[a-zA-Z]+[a-zA-Z.\\s]*$";

            public const string CityOrVillageOrTown = "^[a-zA-Z]+[a-zA-Z.\\s]*$";

            public const string PhoneNo = "^[+]?[0-9]+[0-9-]*$";

            public const string VoterId = "^[a-zA-Z]+[a-zA-Z]+[a-zA-Z0-9]+[0-9]{7,11}$";

            public const string Description = "^[^'][^']*$";

            public const string AccountNumber = "^[0-9]{8,18}$";

            public static HashSet<char> AllowedCharsInPassword = new HashSet<char> { '!', '@', '#', '$', '%', '*', '_', '.' };
        }
    }

    public class NoSpecialCharsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var str = value as string;
            if (string.IsNullOrEmpty(str)) return true;
            return !str.Any(ch => !char.IsLetterOrDigit(ch));
        }
    }
}
