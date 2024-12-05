namespace border.api.Endpoints
{
    public static class UsersEndpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/users/{id}", (string id) => new { userId = id });

        }
    }
}