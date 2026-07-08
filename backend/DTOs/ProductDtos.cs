using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateProductDto(
    [Required][MaxLength(500)] string Name,
    [MaxLength(2000)] string? Description,
    [Range(0, 999999999)] decimal Price
);

public record UpdateProductDto(
    [Required][MaxLength(500)] string Name,
    [MaxLength(2000)] string? Description,
    [Range(0, 999999999)] decimal Price
);
