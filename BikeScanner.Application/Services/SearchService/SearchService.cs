using BikeScanner.Application.Models;
using BikeScanner.Domain.Repositories;
using System.Threading.Tasks;
using System.Linq;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Exceptions;

namespace BikeScanner.Application.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly IContentsRepository _contentsRepository;
        private ContentEntity[] _newContents;

        public SearchService(IContentsRepository contentsRepository)
        {
            _contentsRepository = contentsRepository;
        }

        //ToDo add restrictions
        public Task<bool> CanSearch(long userId)
        {
            return Task.FromResult(true);
        }

        public async Task<SearchResultModel[]> Search(long userId, string query)
        {
            if (!await CanSearch(userId))
                throw AppError.SearchLimit;

            var result = await _contentsRepository.Search(query);

            return result
                .Select(r => new SearchResultModel(query, r.AdUrl))
                .ToArray();
        }

        public async Task<SearchResultModel[]> SearchEpoch(long userId, string query, long indexEpoch)
        {
            if (_newContents == null)
            {
                _newContents = await _contentsRepository.GetContents(indexEpoch);
            }

            var result = _newContents
                .Where(c => c.Text.ToUpper().Contains(query.ToUpper()));

            return result
                .Select(r => new SearchResultModel(query, r.AdUrl))
                .ToArray();
        }
    }
}
