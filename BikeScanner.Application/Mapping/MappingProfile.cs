using AutoMapper;
using BikeScanner.Application.Models;
using BikeScanner.Application.Models.Search;
using BikeScanner.Application.Models.Subs;
using BikeScanner.Application.Models.Users;
using BikeScanner.Domain.Models;

namespace BikeScanner.Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<ContentEntity, SearchResult>();
			CreateMap<SubscriptionEntity, Subscription>();
			CreateMap<UserEntity, User>();
			CreateMap<AdItem, ContentEntity>();
		}
	}
}

