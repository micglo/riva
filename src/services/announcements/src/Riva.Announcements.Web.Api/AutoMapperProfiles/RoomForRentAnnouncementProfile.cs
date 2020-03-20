using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.Announcements.Web.Api.Models.Requests;
using Riva.Announcements.Web.Api.Models.Responses;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.Announcements.Web.Api.AutoMapperProfiles
{
    public class RoomForRentAnnouncementProfile : Profile
    {
        public RoomForRentAnnouncementProfile()
        {
            CreateMap<GetRoomForRentAnnouncementsRequest, GetRoomForRentAnnouncementsInputQuery>()
                .ConvertUsing<GetRoomForRentAnnouncementsInputQueryTypeConverter>();

            CreateMap<CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>, CollectionResponse<RoomForRentAnnouncementResponse>>()
                .ConvertUsing<CollectionResponseTypeConverter>();

            CreateMap<RoomForRentAnnouncementOutputQuery, RoomForRentAnnouncementResponse>()
                .ConvertUsing<RoomForRentAnnouncementResponseTypeConverter>();

            CreateMap<CreateRoomForRentAnnouncementRequest, CreateRoomForRentAnnouncementCommand>()
                .ConvertUsing<CreateRoomForRentAnnouncementCommandTypeConverter>();

            CreateMap<UpdateRoomForRentAnnouncementRequest, UpdateRoomForRentAnnouncementCommand>()
                .ConvertUsing<UpdateRoomForRentAnnouncementCommandTypeConverter>();
        }

        public static RoomTypeEnumeration ConvertToRoomTypeEnumeration(RoomType roomType)
        {
            return EnumerationBase.GetAll<RoomTypeEnumeration>()
                .SingleOrDefault(x => x.DisplayName.ToLower().Equals(roomType.ToString().ToLower()));
        }

        public static RoomType ConvertToRoomTypeEnum(RoomTypeEnumeration roomType)
        {
            return roomType switch
            {
                { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.Single) => RoomType.Single,
                { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.Double) => RoomType.Double,
                { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.Triple) => RoomType.Triple,
                { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.Quadruple) => RoomType
                    .Quadruple,
                { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.MultiPerson) => RoomType
                    .MultiPerson,
                _ => throw new ArgumentException(
                    $"{nameof(roomType.DisplayName)} is not supported by {nameof(RoomType)}.")
            };
        }

        private class CollectionResponseTypeConverter : ITypeConverter<CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>, CollectionResponse<RoomForRentAnnouncementResponse>>
        {
            public CollectionResponse<RoomForRentAnnouncementResponse> Convert(CollectionOutputQuery<RoomForRentAnnouncementOutputQuery> source, CollectionResponse<RoomForRentAnnouncementResponse> destination, ResolutionContext context)
            {
                return new CollectionResponse<RoomForRentAnnouncementResponse>(source.TotalCount,
                    context.Mapper.Map<IEnumerable<RoomForRentAnnouncementOutputQuery>, IEnumerable<RoomForRentAnnouncementResponse>>(source.Results));
            }
        }

        private class RoomForRentAnnouncementResponseTypeConverter : ITypeConverter<RoomForRentAnnouncementOutputQuery, RoomForRentAnnouncementResponse>
        {
            public RoomForRentAnnouncementResponse Convert(RoomForRentAnnouncementOutputQuery source, RoomForRentAnnouncementResponse destination, ResolutionContext context)
            {
                return new RoomForRentAnnouncementResponse(source.Id, source.Title, source.SourceUrl, source.CityId,
                    source.Created, source.Description, source.Price, source.RoomTypes.Select(ConvertToRoomTypeEnum),
                    source.CityDistricts);
            }
        }

        private class GetRoomForRentAnnouncementsInputQueryTypeConverter : ITypeConverter<GetRoomForRentAnnouncementsRequest, GetRoomForRentAnnouncementsInputQuery>
        {
            public GetRoomForRentAnnouncementsInputQuery Convert(GetRoomForRentAnnouncementsRequest source, GetRoomForRentAnnouncementsInputQuery destination,
                ResolutionContext context)
            {
                var roomType = source.RoomType.HasValue
                    ? EnumerationBase.GetAll<RoomTypeEnumeration>().SingleOrDefault(x =>
                        Equals(x, ConvertToRoomTypeEnumeration(source.RoomType.Value)))
                    : null;
                return new GetRoomForRentAnnouncementsInputQuery(source.Page, source.PageSize, source.Sort,
                    source.CreatedFrom, source.CreatedTo, source.CityId, source.PriceFrom, source.PriceTo,
                    roomType, source.CityDistrict);
            }
        }

        private class CreateRoomForRentAnnouncementCommandTypeConverter : ITypeConverter<CreateRoomForRentAnnouncementRequest, CreateRoomForRentAnnouncementCommand>
        {
            public CreateRoomForRentAnnouncementCommand Convert(CreateRoomForRentAnnouncementRequest source, CreateRoomForRentAnnouncementCommand destination, ResolutionContext context)
            {
                return new CreateRoomForRentAnnouncementCommand(Guid.NewGuid(), source.Title, source.SourceUrl, source.CityId,
                    source.Description, source.Price, source.RoomTypes.Select(ConvertToRoomTypeEnumeration), source.CityDistricts);
            }
        }

        private class UpdateRoomForRentAnnouncementCommandTypeConverter : ITypeConverter<UpdateRoomForRentAnnouncementRequest, UpdateRoomForRentAnnouncementCommand>
        {
            public UpdateRoomForRentAnnouncementCommand Convert(UpdateRoomForRentAnnouncementRequest source, UpdateRoomForRentAnnouncementCommand destination, ResolutionContext context)
            {
                return new UpdateRoomForRentAnnouncementCommand(source.Id, source.Title, source.SourceUrl, source.CityId,
                    source.Description, source.Price, source.RoomTypes.Select(ConvertToRoomTypeEnumeration), source.CityDistricts);
            }
        }
    }
}