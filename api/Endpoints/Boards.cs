using border.api.Models;
using border.api.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace border.api.Endpoints;

public class BoardPostRequest
{
    public string Name { get; set; }
    public BoardPostRequest()
    {
        Name = "";
    }
}
public static class BoardsEndpoints
{
    public static void MapBoardsEndpoints(this WebApplication app)
    {
        app.MapGet("/boards/{boardId:int}", async Task<Results<Ok<Board>, NotFound>> (int boardId, BoardsRepository repository) =>
        {
            return await repository.GetBoard(boardId) is Board board ? TypedResults.Ok(board) : TypedResults.NotFound();
        }).RequireAuthorization(p => p.RequireAuthenticatedUser());
        /*app.MapPost("/boards", async Task<Results<Ok<Board>, BadRequest>> ([FromBody] BoardPostRequest board, BoardsRepository repository) =>
        {
            return await repository.GetBoard(boardId) is Board board ? TypedResults.Ok(board) : TypedResults.NotFound();
        });*/
    }
}
