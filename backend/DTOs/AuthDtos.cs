using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public record RegisterDto(
    [Required][MaxLength(100)] string Username,
    [Required][MinLength(6)][MaxLength(100)] string Password
);

public record LoginDto(
    [Required] string Username,
    [Required] string Password
);
