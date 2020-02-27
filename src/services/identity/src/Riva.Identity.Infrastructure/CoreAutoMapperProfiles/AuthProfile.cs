using System;
using AutoMapper;
using Riva.Identity.Core.Models;

namespace Riva.Identity.Infrastructure.CoreAutoMapperProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<IdentityServer4.Models.AuthorizationRequest, AuthorizationRequest>()
                .ConstructUsing(x => new AuthorizationRequest(x.IdP, x.Client.ClientId, x.RedirectUri));

            CreateMap<Microsoft.AspNetCore.Authentication.AuthenticationScheme, AuthenticationScheme>();

            CreateMap<Microsoft.AspNetCore.Authentication.AuthenticateResult, AuthenticateResult>()
                .ConstructUsing(x => new AuthenticateResult(x.Succeeded, x.Failure, x.Principal, x.Properties.Items));

            CreateMap<IdentityServer4.Models.LogoutRequest, LogoutRequest>()
                .ConstructUsing((x, context) =>
                {
                    var subjectId = !string.IsNullOrWhiteSpace(x.SubjectId) ? new Guid(x.SubjectId) : new Guid?();
                    var clientId = !string.IsNullOrWhiteSpace(x.ClientId) ? new Guid(x.ClientId) : new Guid?();
                    return new LogoutRequest(x.ShowSignoutPrompt, x.PostLogoutRedirectUri, x.SignOutIFrameUrl, subjectId, clientId);
                });
        }
    }
}