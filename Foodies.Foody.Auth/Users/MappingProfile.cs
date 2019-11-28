using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Foodies.Foody.Auth.Domain.UserAggregate;

namespace Foodies.Foody.Auth.Commands
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
