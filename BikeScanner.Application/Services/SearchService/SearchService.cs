using BikeScanner.Domain.Repositories;
using System.Threading.Tasks;
using BikeScanner.Domain.Models;
using BikeScanner.Application.Models.Search;
using AutoMapper;
namespace BikeScanner.Application.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly IMapper                    _mapper;
        private readonly IActionsRepository         _actionsRepository;
        private readonly IContentsRepository        _contentsRepository;

        public SearchService(
            IMapper mapper,
            IContentsRepository contentsRepository,
            IActionsRepository actionsRepository
            )
        {
            _mapper = mapper;
            _contentsRepository = contentsRepository;
            _actionsRepository = actionsRepository;
        }

        public async Task<Page<SearchResult>> Search(long userId, string query, int skip, int take)
        {
            var result = await _contentsRepository.Search(query, skip, take);
            await _actionsRepository.LogSearch(userId, query);

            return new Page<SearchResult>()
            {
                Items = _mapper.Map<SearchResult[]>(result.Items),
                Total = result.Total,
                Offset = result.Offset
            };
        }
    }
}
