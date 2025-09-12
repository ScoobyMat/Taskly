using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.UserDtos;

public class UserDto : IMap
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>();
    }
}

