using AutoMapper;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Auth.Users.Commands;
using Foodies.Foody.Auth.Users.DataTransfer;

namespace Foodies.Foody.Auth.Users
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
