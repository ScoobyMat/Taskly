using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.Auth;

public class AuthResponseDto : IMap
{
    public string Token { get; set; } = string.Empty;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, AuthResponseDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore());
    }
}
