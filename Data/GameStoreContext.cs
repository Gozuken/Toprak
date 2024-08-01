using Toprak.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Toprak.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { ID = 1, Name = "Fighting"},
            new { ID = 2, Name = "Roleplay"},
            new { ID = 3, Name = "Race"},
            new { ID = 4, Name = "Sports"},
            new { ID = 5, Name = "Family"}
        );
    }
}
