using Microsoft.EntityFrameworkCore;

namespace Verim.Api.Data;

public static class DataExtensions
{
    public static void MigrateDB(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VerimContext>();

        Console.WriteLine("Migrating database...");
        try
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Database migration successful.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error migrating database:");
            Console.WriteLine($"Error message: {ex.Message}");
            Console.WriteLine($"Error stack trace: {ex.StackTrace}");
            Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
        }
    }

}
