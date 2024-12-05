namespace border.api.Endpoints
{
    public static class Stickers
    {
        public static void MapStickers(WebApplication app)
        {
            app.MapGet("/boards/{boardId:int}/stickers", (int boardId) => boardId);
        }
    }
}