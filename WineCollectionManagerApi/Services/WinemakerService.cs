using WineCollectionManagerApi.Models;

namespace WineCollectionManagerApi.Services
{
    public class WinemakerService : IWinemakerService
    {
        private readonly List<WineMakerModel> _winemakers = new();

        public Task<IEnumerable<WineMakerModel>> GetAll() =>
            Task.FromResult<IEnumerable<WineMakerModel>>(_winemakers);

        public Task<WineMakerModel?> GetById(int id)
        {
            return Task.FromResult(_winemakers.FirstOrDefault(w => w.Id == id));
        }

        public Task Add(WineMakerModel winemaker)
        {
            winemaker.Id = _winemakers.Count > 0 ? _winemakers.Max(w => w.Id) + 1 : 1;
            _winemakers.Add(winemaker);

            return Task.CompletedTask;
        }

        public Task Update(WineMakerModel winemaker)
        {
            var index = _winemakers.FindIndex(w => w.Id == winemaker.Id);
            if (index >= 0)
            {
                _winemakers[index] = winemaker;
            }

            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var winemaker = _winemakers.FirstOrDefault(w => w.Id == id);
            if (winemaker != null)
            {
                _winemakers.Remove(winemaker);
            }

            return Task.CompletedTask;
        }
    }
}
