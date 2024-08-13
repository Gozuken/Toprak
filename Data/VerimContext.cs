using Verim.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Verim.Api.Data;

public class VerimContext(DbContextOptions<VerimContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Asset> Assets => Set<Asset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new {UserId = 1, Username = "admin", Password = "password", AuthToken = string.Empty},
            new {UserId = 2, Username = "Ahmet", Password = "C#asp.netdatabase123", AuthToken = string.Empty},
            new {UserId = 3, Username = "coolbob", Password = "ilovegaming321", AuthToken = string.Empty}
        );

        modelBuilder.Entity<Asset>()
        .HasOne(a => a.Owner)
        .WithMany(u => u.Assets)
        .HasForeignKey(a => a.UserId);
        
        modelBuilder.Entity<Asset>().HasData(
        new {AssetID = 1,
            UserId = 1,
            AssetType = "tarla",
            ProvinceName = "ankara",
            DistrictName = "golbasi",
            NeighborhoodName = "segmenler",
            BlockNumber = "6",
            ParcelNumber ="5"
            },

        new {AssetID = 2,
            UserId = 2,
            AssetType = "bag",
            ProvinceName = "bursa",
            DistrictName = "bozoyuk",
            NeighborhoodName = "yildirim",
            BlockNumber = "4",
            ParcelNumber = "3"
            },

        new {AssetID = 3,
            UserId = 3,
            AssetType = "bahce",
            ProvinceName = "kars",
            DistrictName = "cayyolu",
            NeighborhoodName = "demiroren",
            BlockNumber = "1",
            ParcelNumber = "2"
        });
        
        
    }

}
