using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Commands.Handlers;

namespace Riva.Identity.Web.Api.DependencyInjection
{
    public static class CommandHandlersRegistrar
    {
        public static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<ICommandHandler<CreateRoleCommand>, CreateRoleCommandHandler>()
                .AddScoped<ICommandHandler<UpdateRoleCommand>, UpdateRoleCommandHandler>()
                .AddScoped<ICommandHandler<DeleteRoleCommand>, DeleteRoleCommandHandler>()
                .AddScoped<ICommandHandler<CreateAccountCommand>, CreateAccountCommandHandler>()
                .AddScoped<ICommandHandler<UpdateAccountRolesCommand>, UpdateAccountRolesCommandHandler>()
                .AddScoped<ICommandHandler<DeleteAccountCommand>, DeleteAccountCommandHandler>()
                .AddScoped<ICommandHandler<RequestAccountConfirmationTokenCommand>, RequestAccountConfirmationTokenCommandHandler>()
                .AddScoped<ICommandHandler<ConfirmAccountCommand>, ConfirmAccountCommandHandler>()
                .AddScoped<ICommandHandler<RequestPasswordResetTokenCommand>, RequestPasswordResetTokenCommandHandler>()
                .AddScoped<ICommandHandler<ResetPasswordCommand>, ResetPasswordCommandHandler>()
                .AddScoped<ICommandHandler<ChangePasswordCommand>, ChangePasswordCommandHandler>()
                .AddScoped<ICommandHandler<AssignPasswordCommand>, AssignPasswordCommandHandler>();
        }
    }
}