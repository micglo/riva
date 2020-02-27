using System.Collections.Generic;
using AutoMapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Queries;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Models.Responses.Accounts;

namespace Riva.Identity.Web.Api.AutoMapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<GetAccountOutputQuery, GetAccountResponse>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.Tokens, opt => opt.Ignore())
                .ConstructUsing((x, context) =>
                {
                    var getAccountTokens =
                        context.Mapper.Map<IReadOnlyCollection<GetAccountTokenOutputQuery>, IEnumerable<AccountToken>>(x.Tokens);
                    return new GetAccountResponse(x.Id, x.Email, x.Confirmed, x.Created, x.PasswordAssigned, x.LastLogin, x.Roles, getAccountTokens);
                });

            CreateMap<GetAccountsRequest, GetAccountsInputQuery>();

            CreateMap<GetAccountsOutputQuery, GetAccountsCollectionItemResponse>();

            CreateMap<CollectionOutputQuery<GetAccountsOutputQuery>, CollectionResponse<GetAccountsCollectionItemResponse>>();

            CreateMap<CreateAccountRequest, CreateAccountCommand>()
                .ConstructUsing(x => new CreateAccountCommand(x.Email, x.Password));

            CreateMap<ConfirmAccountRequest, ConfirmAccountCommand>();

            CreateMap<RequestAccountConfirmationTokenRequest, RequestAccountConfirmationTokenCommand>();

            CreateMap<RequestPasswordResetTokenRequest, RequestPasswordResetTokenCommand>();

            CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
        }
    }
}