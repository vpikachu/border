namespace border.api.Endpoints
{
    public static class Stickers
    {
        public static void MapStikers(WebApplication app)
        {
            app.MapGet("/boards/{boardId:int}/stickers", (int boardId) => boardId);
        }
    }
}