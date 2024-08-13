using Verim.Api.Data;
using Verim.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Verim");
builder.Services.AddSqlite<VerimContext>(connString);

var app = builder.Build();

app.MapAuthenticationEndpoints();
app.MapMainEndpoints();

app.MigrateDB();

app.Run();
