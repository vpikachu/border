namespace border.api.Models;

public class GoogleAuthSettings
{
    public string ClientSecret { get; set; } = String.Empty;
    public string ClientId { get; set; } = String.Empty;
}
public class AuthSettings
{
    public GoogleAuthSettings Google { get; set; } = new GoogleAuthSettings();
}