using AutoMapper;
using Unidash.Auth.Application.DataModels;
using Unidash.Auth.Application.Features.Users.Requests;
using Unidash.Auth.Application.Features.Users.Requests.Responses;
using Unidash.Events.Auth;

namespace Unidash.Auth.Application.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserRequest, User>(MemberList.Destination)
                .ForMember(dm => dm.UserName, opt =>
                    opt.MapFrom(x => x.Email))
                .ForMember(dm => dm.Id, opt =>
                    opt.UseDestinationValue());

            CreateMap<User, UserRegisteredEvent>(MemberList.Destination)
                .ForMember(dm => dm.DisplayName, mo =>
                    mo.MapFrom(x => $"{x.FirstName} {x.LastName}"));

            CreateMap<User, UserResponse>(MemberList.Destination)
                .ForMember(dm => dm.DisplayName, opt => 
                    opt.MapFrom(x => $"{x.FirstName} {x.LastName}"));
        }
    }
}