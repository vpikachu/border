namespace border.api.Models;

public record AuthSettings
{
    public record GoogleAuthSettings
    {
        public string ClientSecret = "";
        public string ClientId = "";
    }
    public GoogleAuthSettings Google = new GoogleAuthSettings();
}