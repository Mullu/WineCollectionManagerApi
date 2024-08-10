using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WineCollectionManagerApi.Enums;
using WineCollectionManagerApi.Validations;

namespace WineCollectionManagerApi.Models
{
    public class WineBottleCreateModel
    {
        [MinLength(1, ErrorMessage = "Name must be at least 1 character long.")]
        public string Name { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int Year { get; set; }

        [JsonPropertyName("Size")]
        public int SizeInMilliLiter { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Count in wine cellar must be a non-negative integer.")]
        public int CountInWineCellar { get; set; }

        [EnumDataType(typeof(WineStyle), ErrorMessage = "Invalid wine style.")]
        public WineStyle Style { get; set; }

        public string Taste { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string FoodPairing { get; set; } = string.Empty;

        [UriValidation(ErrorMessage = "Invalid or unsupported URI.")]
        public Uri Link { get; set; } = new Uri("http://placeholder.com");

        [ImageExtensionValidation(ErrorMessage = "Image must be a .jpg or .png file.")]
        public string Image { get; set; } = String.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "WinemakerId must be must be a non-negative integer.")]
        public int WinemakerId { get; set; }
    }
}
