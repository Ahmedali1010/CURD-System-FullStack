namespace Backend.DTOs;

public record UserDto(int Id, string Username, string Role);
public record UpdateUserRoleDto(int RoleId);
