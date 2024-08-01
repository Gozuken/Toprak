namespace Toprak.Api.Endpoints;

using Toprak.Api.Dtos;

public static class ToprakEndpoints
{
    private static readonly List<ToprakDto> games = [
    new (
        1,
        "Street Fighter",
        "Fight",
        19.99M,
        new DateOnly(1992, 7, 15)),
    
    new (
        2,
        "Final Fantasy",
        "Roleplay",
        29.99M,
        new DateOnly(1997, 5, 21)),

    new ( 
        3,
        "Minecraft",
        "Open World",
        12.99M,
        new DateOnly(2009, 9, 9))   
    ]; 
    
    public static WebApplication MapGetEndpoints(this WebApplication app) 
    {
        const string GetGameEndpointName = "GetGame";
        
        // GET /games
        app.MapGet("games", () => games);


        // GET /games/x
        app.MapGet("games/{id}", (int id) => 
        {

            ToprakDto? game = games.Find(game => game.ID == id);

            return game is null ? Results.NotFound() : Results.Ok(game);

        }).WithName(GetGameEndpointName);


        // POST /games
        app.MapPost("games", (CreateToprakDto newGame) => 
        {
            ToprakDto game = new(
                games.Count() + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.ID}, game);
        }).WithParameterValidation();


        // PUT /games/x
        app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.ID == id);

            if(index == -1) 
            {
                return Results.NotFound();  
            }

            games[index] = new ToprakDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        } 
        );


        // DELETE /games/x
        app.MapDelete("games/{id}", (int id) => 
        {
            games.RemoveAll(game => game.ID == id);

            return Results.NoContent();
        });
        
        return app;

    }


}
