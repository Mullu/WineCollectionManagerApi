using WineCollectionManagerApi.Models;

namespace WineCollectionManagerApi.Services
{
    public interface IWinemakerService
    {
        Task<IEnumerable<WineMakerModel>> GetAll();
        Task<WineMakerModel?> GetById(int id);
        Task Add(WineMakerModel winemaker);
        Task Update(WineMakerModel winemaker);
        Task Delete(int id);
    }
}
