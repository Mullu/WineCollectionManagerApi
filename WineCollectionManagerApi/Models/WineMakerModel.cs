using System.ComponentModel.DataAnnotations;

namespace WineCollectionManagerApi.Models
{
    public class WineMakerModel
    {
        [Range(0, int.MaxValue, ErrorMessage = "Id must be must be a non-negative integer.")]
        public int Id { get; set; }

        [MinLength(1, ErrorMessage = "Name must be at least 1 character long.")]
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<WineBottleModel> WineBottles { get; set; } = new List<WineBottleModel>();
    }
}
