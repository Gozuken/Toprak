using Toprak.Api;
using Toprak.Api.Data;
using Toprak.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

app.MapGetEndpoints();

app.MigrateDb();

app.Run();