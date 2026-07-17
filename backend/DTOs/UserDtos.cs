// using System.ComponentModel.DataAnnotations;

// namespace Backend.DTOs;

// /// <summary>
// /// DTO for user registration.
// /// </summary>
// public record RegisterDto(
//     [StringLength(50, MinimumLength = 3)] string Username,
    
//     [EmailAddress] string Email,
//     [MinLength(8)] string Password
// );

// /// <summary>
// /// DTO for user login.
// /// </summary>
// public record LoginDto(
//     [StringLength(50)] string Username,
//     string Password
// );

// /// <summary>
// /// DTO for returning user information in API responses.
// /// </summary>
// public record UserDto(
//     int Id,
//     string Username,
//     string Email,
//     string Role,
//     bool IsActive,
//     DateTime CreatedAt,
//     DateTime? LastLoginAt
// );

// /// <summary>
// /// DTO for creating a user (Admin operation).
// /// </summary>
// public record CreateUserDto(
//     [StringLength(50, MinimumLength = 3)] string Username,
//     [EmailAddress] string Email,
//     [MinLength(8)] string Password,
//     int RoleId
// );

// /// <summary>
// /// DTO for updating user information.
// /// </summary>
// public record UpdateUserDto(
//     [StringLength(50, MinimumLength = 3)] string? Username,
//     [EmailAddress] string? Email
// );

// /// <summary>
// /// DTO for updating user role (Admin operation).
// /// </summary>
// public record UpdateUserRoleDto(int RoleId);

// /// <summary>
// /// DTO for changing user password.
// /// </summary>
// public record ChangePasswordDto(
//     string CurrentPassword,
//     [MinLength(8)] string NewPassword
// );

// /// <summary>
// /// DTO for authentication response containing JWT token.
// /// </summary>
// public record AuthResponseDto(
//     string Token,
//     string Username,
//     string Email,
//     int ExpiresIn
// );



using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

/// <summary>
/// DTO for user registration.
/// </summary>
public record RegisterDto(
    [property: Required]
    [property: StringLength(50, MinimumLength = 3)]
    string Username,

    [property: Required]
    [property: EmailAddress]
    string Email,

    [property: Required]
    [property: MinLength(8)]
    [property: DataType(DataType.Password)]
    string Password
);

/// <summary>
/// DTO for user login.
/// </summary>
public record LoginDto(
    [property: Required]
    [property: StringLength(50, MinimumLength = 3)]
    string Username,

    [property: Required]
    [property: DataType(DataType.Password)]
    string Password
);

/// <summary>
/// DTO for returning user information.
/// </summary>
public record UserDto(
    int Id,
    string Username,
    string Email,
    string Role,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);

/// <summary>
/// DTO for creating a user (Admin).
/// </summary>
public record CreateUserDto(
    [property: Required]
    [property: StringLength(50, MinimumLength = 3)]
    string Username,

    [property: Required]
    [property: EmailAddress]
    string Email,

    [property: Required]
    [property: MinLength(8)]
    [property: DataType(DataType.Password)]
    string Password,

    [property: Range(1, int.MaxValue)]
    int RoleId
);

/// <summary>
/// DTO for updating user.
/// </summary>
public record UpdateUserDto(
    [property: StringLength(50, MinimumLength = 3)]
    string? Username,

    [property: EmailAddress]
    string? Email
);

/// <summary>
/// DTO for updating user role.
/// </summary>
public record UpdateUserRoleDto(
    [property: Range(1, int.MaxValue)]
    int RoleId
);

/// <summary>
/// DTO for changing password.
/// </summary>
public record ChangePasswordDto(
    [property: Required]
    [property: DataType(DataType.Password)]
    string CurrentPassword,

    [property: Required]
    [property: MinLength(8)]
    [property: DataType(DataType.Password)]
    string NewPassword
);

/// <summary>
/// Authentication response.
/// </summary>
public record AuthResponseDto(
    string Token,
    string Username,
    string Email,
    int ExpiresIn
);
