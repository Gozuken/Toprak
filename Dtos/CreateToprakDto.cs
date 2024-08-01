using System.ComponentModel.DataAnnotations;

namespace Toprak.Api.Dtos;

public record class CreateToprakDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(50)] string Genre,
    [Range(1, 10)] decimal Price,
    DateOnly ReleaseDate
    );