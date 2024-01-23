namespace border.api.Models
{
    public class UserBoard
    {
        public string UserId { get; set; }
        public long BoardId { get; set; }
        public UserBoard()
        {
            UserId = "";
        }

    }
}