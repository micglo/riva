using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.Users.Core.Commands;
using Riva.Users.Core.Enums;
using Riva.Users.Core.Extensions;
using Riva.Users.Core.Models;
using Riva.Users.Core.Queries;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Web.Api.Models.Requests;
using Riva.Users.Web.Api.Models.Responses;

namespace Riva.Users.Web.Api.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserOutputQuery, UserResponse>()
                .ForMember(x => x.FlatForRentAnnouncementPreferences, opt => opt.Ignore())
                .ForMember(x => x.RoomForRentAnnouncementPreferences, opt => opt.Ignore())
                .ConstructUsing((output, context) => new UserResponse(output.Id, output.Email, output.Picture,
                    output.ServiceActive,
                    output.AnnouncementPreferenceLimit,
                    context.Mapper.Map<AnnouncementSendingFrequencyEnumeration, AnnouncementSendingFrequency>(
                        output.AnnouncementSendingFrequency),
                    context.Mapper
                        .Map<IReadOnlyCollection<RoomForRentAnnouncementPreferenceOutputQuery>,
                            IEnumerable<RoomForRentAnnouncementPreferenceResponse>>(output
                            .RoomForRentAnnouncementPreferences),
                    context.Mapper
                        .Map<IReadOnlyCollection<FlatForRentAnnouncementPreferenceOutputQuery>,
                            IEnumerable<FlatForRentAnnouncementPreferenceResponse>>(output
                            .FlatForRentAnnouncementPreferences)));

            CreateMap<GetUsersRequest, GetUsersInputQuery>();

            CreateMap<CollectionOutputQuery<UserOutputQuery>, CollectionResponse<UserResponse>>();

            CreateMap<CreateUserRequest, CreateUserCommand>()
                .ConvertUsing(request => new CreateUserCommand(request.Id, request.Email, request.ServiceActive,
                    request.AnnouncementPreferenceLimit, request.AnnouncementSendingFrequency.ConvertToEnumeration()));

            CreateMap<UpdateUserRequest, UpdateUserCommand>()
                .ConvertUsing(request => new UpdateUserCommand(request.Id, request.ServiceActive,
                    request.AnnouncementPreferenceLimit, request.AnnouncementSendingFrequency.ConvertToEnumeration(),
                    ExtractPicture(request.Picture)));
        }

        private static PictureDto ExtractPicture(IFormFile file)
        {
            if (file == null)
                return null;

            using var stream = file.OpenReadStream();
            return new PictureDto(ReadFully(stream), file.ContentType);
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using var ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }
}