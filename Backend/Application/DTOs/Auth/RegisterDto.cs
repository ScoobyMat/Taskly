using System.ComponentModel.DataAnnotations;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.Auth;

public class RegisterDto : IMap
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RegisterDto, User>();
    }
}
