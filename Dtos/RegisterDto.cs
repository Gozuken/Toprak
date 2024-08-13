using System.ComponentModel.DataAnnotations;

namespace Verim.Api.Dtos;

public record class RegisterDto(
    [Required][StringLength(50)] string Username,
    [Required][StringLength(50)] string Password
    );


