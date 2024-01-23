using System.Data;
using System.Data.Common;
using api.Services;
using border.api.Endpoints;
using border.api.Models;
using border.api.Repositories;
using border.api.Services;
using Dapper;

var builder = WebApplication.CreateBuilder(args);
string? connectionStr = builder.Configuration.GetConnectionString("Main") ?? throw new Exception("Connection string is not provided");
builder.Services.AddSingleton<IDatabaseHelper>(new SqliteDatabaseHelper(connectionStr));
builder.Services.AddTransient<BoardsRepository>();

var app = builder.Build();
app.DoMigrate();
app.MapBoardsEndpoints();
app.MapGet("/", () =>
{
    return Results.Ok("border REST API");
});

app.Run();
