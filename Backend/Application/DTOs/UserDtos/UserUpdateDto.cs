using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.UserDtos;

public class UserUpdateDto : IMap
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserUpdateDto, User>();
    }
}
