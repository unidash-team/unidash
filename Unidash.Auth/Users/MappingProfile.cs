using AutoMapper;
using Unidash.Auth.Domain.UserAggregate;
using Unidash.Auth.Users.Commands;
using Unidash.Auth.Users.Requests.DataTransfer;

namespace Unidash.Auth.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserCommand, User>(MemberList.Destination);
            CreateMap<User, UserDto>(MemberList.Destination);
        }
    }
}
