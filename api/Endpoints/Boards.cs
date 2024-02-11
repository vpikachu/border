using border.api.Models;
using border.api.Repositories;
using border.api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace border.api.Endpoints
{
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
            });
            /*app.MapPost("/boards", async Task<Results<Ok<Board>, BadRequest>> ([FromBody] BoardPostRequest board, BoardsRepository repository) =>
            {
                return await repository.GetBoard(boardId) is Board board ? TypedResults.Ok(board) : TypedResults.NotFound();
            });*/
        }
    }
}