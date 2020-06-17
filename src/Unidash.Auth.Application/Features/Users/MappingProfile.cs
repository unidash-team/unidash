using AutoMapper;
using Unidash.Auth.Application.DataModels;
using Unidash.Auth.Application.Features.Users.Requests;

namespace Unidash.Auth.Application.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserRequest, User>(MemberList.Destination)
                .ForMember(dm => dm.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
