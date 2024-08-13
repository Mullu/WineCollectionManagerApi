using System.Linq.Expressions;
using WineCollectionManagerApi.Models;
using WineCollectionManagerApi.Enums;

namespace WineCollectionManagerApi.Services
{
    public class WineBottleService : IWineBottleService
    {
        private readonly List<WineBottleModel> _wineBottles = new();
        private readonly IWinemakerService _winemakerService;

        public WineBottleService(IWinemakerService winemakerService)
        {
            _winemakerService = winemakerService;
        }

        public Task<IEnumerable<WineBottleModel>> GetAll() =>
            Task.FromResult<IEnumerable<WineBottleModel>>(_wineBottles);

        public Task<IEnumerable<WineBottleModel>> GetByWinemakerId(int winemakerId)
        {
            return Task.FromResult<IEnumerable<WineBottleModel>>(
                _wineBottles.Where(wb => wb.WinemakerId == winemakerId));
        }

        public Task<WineBottleModel?> GetById(int id)
        {
            return Task.FromResult(_wineBottles.FirstOrDefault(wb => wb.Id == id));
        }

        public async Task Add(WineBottleModel wineBottle)
        {
            wineBottle.Id = _wineBottles.Count > 0 ? _wineBottles.Max(wb => wb.Id) + 1 : 1;

            var winemaker = await _winemakerService.GetById(wineBottle.WinemakerId);

            if (winemaker != null)
            {
                winemaker.WineBottles.Add(wineBottle);
                _wineBottles.Add(wineBottle);
            }
            else
            {
                throw new ArgumentException($"Winemaker with ID {wineBottle.WinemakerId} does not exist.");
            }
        }

        public void Update(WineBottleModel wineBottle)
        {
            var index = _wineBottles.FindIndex(wb => wb.Id == wineBottle.Id);

            if (index >= 0)
            {
                _wineBottles[index] = wineBottle;
            }
        }

        public async Task Delete(int id)
        {
            var wineBottle = _wineBottles.FirstOrDefault(wb => wb.Id == id);

            if (wineBottle != null)
            {
                var winemaker = await _winemakerService.GetById(wineBottle.WinemakerId);
                winemaker?.WineBottles.Remove(wineBottle);
                _wineBottles.Remove(wineBottle);
            }
        }

        public async Task<IEnumerable<WineBottleModel>> Filter(
            int? year,
            int? sizeInMilliLiter,
            int? countInWineCellar,
            WineStyle? style,
            string? taste,
            string? foodPairing)
        {
            Expression<Func<WineBottleModel, bool>> filterExpression = wb => true;

            if (year.HasValue)
            {
                filterExpression = CombineExpressions(filterExpression, wb => wb.Year == year.Value);
            }

            if (sizeInMilliLiter.HasValue)
            {
                filterExpression = CombineExpressions(filterExpression, wb => wb.SizeInMilliLiter == sizeInMilliLiter.Value);
            }

            if (countInWineCellar.HasValue)
            {
                filterExpression = CombineExpressions(filterExpression, wb => wb.CountInWineCellar == countInWineCellar.Value);
            }

            if (style.HasValue)
            {
                filterExpression = CombineExpressions(filterExpression, wb => wb.Style == style.Value);
            }

            if (!string.IsNullOrWhiteSpace(taste))
            {
                filterExpression = CombineExpressions(filterExpression, wb => wb.Taste.Contains(taste, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(foodPairing))
            {
                filterExpression = CombineExpressions(filterExpression, wb => wb.FoodPairing.Contains(foodPairing, StringComparison.OrdinalIgnoreCase));
            }

            var query = _wineBottles.AsQueryable().Where(filterExpression);

            return await Task.FromResult(query.ToList());
        }

        private static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var invokedExpression = Expression.Invoke(second, first.Parameters);
            var combinedExpression = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(first.Body, invokedExpression),
                first.Parameters);

            return combinedExpression;
        }
    }
}
