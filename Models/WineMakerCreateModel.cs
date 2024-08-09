using System.ComponentModel.DataAnnotations;

namespace WineCollectionManagerApi.Models
{
    public class WineMakerCreateModel
    {
        [MinLength(1, ErrorMessage = "Name must be at least 1 character long.")]
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<WineBottleCreateModel> WineBottles { get; set; } = new List<WineBottleCreateModel>();
    }
}
