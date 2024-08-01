namespace Toprak.Api.Entities;

public class Game
{
    public int ID {get; set;}

    public required string Name {get; set;}

    public int GenreId {get; set;}

    public Genre? Genre {get; set;}

    public decimal Price {get; set;}

    public DateOnly ReleaseDate {get; set;}
    
}
