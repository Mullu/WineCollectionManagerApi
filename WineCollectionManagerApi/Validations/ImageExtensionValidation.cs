using System.ComponentModel.DataAnnotations;

namespace WineCollectionManagerApi.Validations
{
    public class ImageExtensionValidation : ValidationAttribute
    {
        public string Extensions { get; set; } = "png,jpg";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not string fileName)
            {
                return new ValidationResult("The value must be a string representing a file name.");
            }

            var allowedExtensions = Extensions
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(ext => ext.Trim().ToLowerInvariant())
                .ToArray();

            var fileExtension = Path.GetExtension(fileName)?.TrimStart('.').ToLowerInvariant();

            if (fileExtension == null || !allowedExtensions.Contains(fileExtension))
            {
                return new ValidationResult(ErrorMessage ?? "Invalid file extension.");
            }

            return ValidationResult.Success;
        }
    }
}
