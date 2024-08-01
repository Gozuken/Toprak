namespace Toprak.Api.Dtos;

public record class ToprakDto(
    int ID,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
    );
