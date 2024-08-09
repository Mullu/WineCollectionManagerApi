using System.ComponentModel.DataAnnotations;

namespace WineCollectionManagerApi.Validations
{
    public class UriValidation : ValidationAttribute
    {
        public string[] AllowedSchemes { get; set; } = { "http", "https" };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is Uri uri)
            {
                if (!IsValidUri(uri))
                {
                    return new ValidationResult(ErrorMessage ?? "The provided URI is invalid.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("The value must be a valid URI.");
        }

        private bool IsValidUri(Uri uri)
        {
            return uri.IsWellFormedOriginalString() &&
                   AllowedSchemes.Contains(uri.Scheme);
        }
    }
}
