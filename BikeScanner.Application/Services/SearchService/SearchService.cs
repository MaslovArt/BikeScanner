using BikeScanner.Domain.Repositories;
using System.Threading.Tasks;
using System.Linq;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Exceptions;
using System;

namespace BikeScanner.Application.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly ISearchHistoryRepository   _searchHistoryRepository;
        private readonly IContentsRepository        _contentsRepository;
        private ContentEntity[]                     _newContents;

        public SearchService(
            IContentsRepository contentsRepository,
            ISearchHistoryRepository searchHistoryRepository
            )
        {
            _contentsRepository = contentsRepository;
            _searchHistoryRepository = searchHistoryRepository;
        }

        //ToDo add restrictions
        public Task<bool> CanSearch(long userId)
        {
            return Task.FromResult(true);
        }

        public async Task<PagedEntities<ContentEntity>> Search(long userId, string query, int skip, int take)
        {
            if (!await CanSearch(userId))
                throw AppError.SearchLimit;

            await WriteHistory(userId, query);

            return await _contentsRepository.Search(query, skip, take);
        }

        public async Task<ContentEntity[]> SearchEpoch(long userId, string query, long indexEpoch)
        {
            if (_newContents == null)
            {
                _newContents = await _contentsRepository.GetContents(indexEpoch);
            }

            return _newContents
                .Where(c => c.Text.ToUpper().Contains(query.ToUpper()))
                .ToArray();
        }

        private Task WriteHistory(long userId, string query)
        {
            var history = new SearchHistoryEntity()
            {
                UserId = userId,
                SearchQuery = query,
                Date = DateTime.UtcNow
            };
            return _searchHistoryRepository.Add(history);
        }
    }
}
