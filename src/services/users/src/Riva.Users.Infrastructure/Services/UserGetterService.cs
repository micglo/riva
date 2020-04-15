using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Infrastructure.Services
{
    public class UserGetterService : IUserGetterService
    {
        private readonly IUserRepository _userRepository;

        public UserGetterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetResult<User>> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                var errors = new List<IError>
                {
                    new Error(UserErrorCodeEnumeration.NotFound, UserErrorMessage.NotFound)
                };
                return GetResult<User>.Fail(errors);
            }
            return GetResult<User>.Ok(user);
        }
    }
}