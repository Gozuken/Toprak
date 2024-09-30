using Microsoft.EntityFrameworkCore;
using Verim.Api.Data;
using Verim.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<VerimContext>(options =>
    options.UseNpgsql(connString));

var app = builder.Build();

app.MapAuthenticationEndpoints();
app.MapMainEndpoints();
app.MapDataEndpoints();

app.MigrateDB();

app.Run();
