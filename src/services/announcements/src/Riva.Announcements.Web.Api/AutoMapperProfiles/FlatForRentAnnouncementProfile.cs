using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.Announcements.Web.Api.Models.Requests;
using Riva.Announcements.Web.Api.Models.Responses;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.Announcements.Web.Api.AutoMapperProfiles
{
    public class FlatForRentAnnouncementProfile : Profile
    {
        public FlatForRentAnnouncementProfile()
        {
            CreateMap<GetFlatForRentAnnouncementsRequest, GetFlatForRentAnnouncementsInputQuery>()
                .ConvertUsing<GetFlatForRentAnnouncementsInputQueryTypeConverter>();

            CreateMap<CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>, CollectionResponse<FlatForRentAnnouncementResponse>>()
                .ConvertUsing<CollectionResponseTypeConverter>();

            CreateMap<FlatForRentAnnouncementOutputQuery, FlatForRentAnnouncementResponse>()
                .ConvertUsing<FlatForRentAnnouncementResponseTypeConverter>();

            CreateMap<CreateFlatForRentAnnouncementRequest, CreateFlatForRentAnnouncementCommand>()
                .ConvertUsing<CreateFlatForRentAnnouncementCommandTypeConverter>();

            CreateMap<UpdateFlatForRentAnnouncementRequest, UpdateFlatForRentAnnouncementCommand>()
                .ConvertUsing<UpdateFlatForRentAnnouncementCommandTypeConverter>();
        }

        public static NumberOfRoomsEnumeration ConvertToNumberOfRoomsEnumeration(NumberOfRooms numberOfRooms)
        {
            return EnumerationBase.GetAll<NumberOfRoomsEnumeration>()
                .SingleOrDefault(x => x.DisplayName.ToLower().Equals(numberOfRooms.ToString().ToLower()));
        }

        public static NumberOfRooms ConvertToNumberOfRoomsEnum(NumberOfRoomsEnumeration numberOfRooms)
        {
            return numberOfRooms switch
            {
                { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.One) =>
                NumberOfRooms.One,
                { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.Two) =>
                NumberOfRooms.Two,
                { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.Three) =>
                NumberOfRooms.Three,
                { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.Four) =>
                NumberOfRooms.Four,
                { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.FiveAndMore)
                => NumberOfRooms.FiveAndMore,
                { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration,
                    NumberOfRoomsEnumeration.NotSpecified) => NumberOfRooms.NotSpecified,
                _ => throw new ArgumentException(
                    $"{nameof(numberOfRooms.DisplayName)} is not supported by {nameof(NumberOfRooms)}.")
            };
        }

        private class CollectionResponseTypeConverter : ITypeConverter<CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>, CollectionResponse<FlatForRentAnnouncementResponse>>
        {
            public CollectionResponse<FlatForRentAnnouncementResponse> Convert(CollectionOutputQuery<FlatForRentAnnouncementOutputQuery> source, CollectionResponse<FlatForRentAnnouncementResponse> destination, ResolutionContext context)
            {
                return new CollectionResponse<FlatForRentAnnouncementResponse>(source.TotalCount,
                    context.Mapper.Map<IEnumerable<FlatForRentAnnouncementOutputQuery>, IEnumerable<FlatForRentAnnouncementResponse>>(source.Results));
            }
        }

        private class FlatForRentAnnouncementResponseTypeConverter : ITypeConverter<FlatForRentAnnouncementOutputQuery, FlatForRentAnnouncementResponse>
        {
            public FlatForRentAnnouncementResponse Convert(FlatForRentAnnouncementOutputQuery source, FlatForRentAnnouncementResponse destination, ResolutionContext context)
            {
                return new FlatForRentAnnouncementResponse(source.Id, source.Title, source.SourceUrl, source.CityId,
                    source.Created, source.Description, source.Price, ConvertToNumberOfRoomsEnum(source.NumberOfRooms),
                    source.CityDistricts);
            }
        }

        private class GetFlatForRentAnnouncementsInputQueryTypeConverter : ITypeConverter<GetFlatForRentAnnouncementsRequest, GetFlatForRentAnnouncementsInputQuery>
        {
            public GetFlatForRentAnnouncementsInputQuery Convert(GetFlatForRentAnnouncementsRequest source, GetFlatForRentAnnouncementsInputQuery destination,
                ResolutionContext context)
            {
                var numberOfRooms = source.NumberOfRooms.HasValue
                    ? ConvertToNumberOfRoomsEnumeration(source.NumberOfRooms.Value)
                    : null;
                return new GetFlatForRentAnnouncementsInputQuery(source.Page, source.PageSize, source.Sort,
                    source.CreatedFrom, source.CreatedTo, source.CityId, source.PriceFrom, source.PriceTo,
                    numberOfRooms, source.CityDistrict);
            }
        }

        private class CreateFlatForRentAnnouncementCommandTypeConverter : ITypeConverter<CreateFlatForRentAnnouncementRequest, CreateFlatForRentAnnouncementCommand>
        {
            public CreateFlatForRentAnnouncementCommand Convert(CreateFlatForRentAnnouncementRequest source, CreateFlatForRentAnnouncementCommand destination, ResolutionContext context)
            {
                return new CreateFlatForRentAnnouncementCommand(Guid.NewGuid(), source.Title, source.SourceUrl, source.CityId,
                    source.Description, source.Price, ConvertToNumberOfRoomsEnumeration(source.NumberOfRooms), source.CityDistricts);
            }
        }

        private class UpdateFlatForRentAnnouncementCommandTypeConverter : ITypeConverter<UpdateFlatForRentAnnouncementRequest, UpdateFlatForRentAnnouncementCommand>
        {
            public UpdateFlatForRentAnnouncementCommand Convert(UpdateFlatForRentAnnouncementRequest source, UpdateFlatForRentAnnouncementCommand destination, ResolutionContext context)
            {
                return new UpdateFlatForRentAnnouncementCommand(source.Id, source.Title, source.SourceUrl, source.CityId,
                    source.Description, source.Price, ConvertToNumberOfRoomsEnumeration(source.NumberOfRooms), source.CityDistricts);
            }
        }
    }
}