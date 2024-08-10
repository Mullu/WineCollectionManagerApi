using WineCollectionManagerApi.Enums;
using WineCollectionManagerApi.Models;

namespace WineCollectionManagerApi.Services
{
    public interface IWineBottleService
    {
        Task<IEnumerable<WineBottleModel>> GetAll();
        Task<IEnumerable<WineBottleModel>> GetByWinemakerId(int winemakerId);
        Task<WineBottleModel?> GetById(int id);
        Task Add(WineBottleModel wineBottle);
        void Update(WineBottleModel wineBottle);
        Task Delete(int id);
        Task<IEnumerable<WineBottleModel>> Filter(
            int? year,
            int? size,
            int? countInWineCellar,
            WineStyle? style,
            string? taste,
            string? foodPairing);
    }
}
