using WineCollectionManagerApi.Models;
using WineCollectionManagerApi.Services;
using WineCollectionManagerApi.Enums;

namespace WineCollectionManagerApi.Initialization
{
    public static class SampleDataInitializer
    {
        public static async Task Initialize(
            IWinemakerService winemakerService,
            IWineBottleService wineBottleService)
        {
            var winemaker1 = new WineMakerModel
            {
                Id = 1,
                Name = "Winemaker A",
                Address = "123 Wine St, Napa, CA"
            };

            var winemaker2 = new WineMakerModel
            {
                Id = 2,
                Name = "Winemaker B",
                Address = "456 Vineyard Rd, Sonoma, CA"
            };

            await winemakerService.Add(winemaker1);
            await winemakerService.Add(winemaker2);

            var wineBottle1 = new WineBottleModel
            {
                Id = 1,
                Name = "Cabernet Sauvignon",
                Year = 2018,
                SizeInMilliLiter = 500,
                CountInWineCellar = 10,
                Style = WineStyle.Dry,
                Taste = "Plum, Tobacco",
                Description = "A full-bodied red wine.",
                FoodPairing = "Steak, Grilled Lamb",
                Link = new Uri("http://example.com/cab-sauvignon"),
                Image = "http://example.com/images/cab-sauvignon.jpg",
                WinemakerId = 1
            };

            var wineBottle2 = new WineBottleModel
            {
                Id = 2,
                Name = "Chardonnay",
                Year = 2020,
                SizeInMilliLiter = 750,
                CountInWineCellar = 20,
                Style = WineStyle.Dry,
                Taste = "Apple, Vanilla",
                Description = "A crisp white wine.",
                FoodPairing = "Chicken, Fish",
                Link = new Uri("http://example.com/chardonnay"),
                Image = "http://example.com/images/chardonnay.jpg",
                WinemakerId = 2
            };

            var wineBottle3 = new WineBottleModel
            {
                Id = 3,
                Name = "Merlot",
                Year = 2019,
                SizeInMilliLiter = 500,
                CountInWineCellar = 15,
                Style = WineStyle.SemiDry,
                Taste = "Cherry, Chocolate",
                Description = "A smooth red wine with chocolate notes.",
                FoodPairing = "Pasta, Beef",
                Link = new Uri("http://example.com/merlot"),
                Image = "http://example.com/images/merlot.jpg",
                WinemakerId = 1
            };

            var wineBottle4 = new WineBottleModel
            {
                Id = 4,
                Name = "Sauvignon Blanc",
                Year = 2021,
                SizeInMilliLiter = 750,
                CountInWineCellar = 25,
                Style = WineStyle.Dry,
                Taste = "Citrus, Green Apple",
                Description = "A refreshing white wine with a citrusy flavor.",
                FoodPairing = "Seafood, Salads",
                Link = new Uri("http://example.com/sauvignon-blanc"),
                Image = "http://example.com/images/sauvignon-blanc.jpg",
                WinemakerId = 2
            };

            await wineBottleService.Add(wineBottle1);
            await wineBottleService.Add(wineBottle2);
            await wineBottleService.Add(wineBottle3);
            await wineBottleService.Add(wineBottle4);
        }
    }
}
