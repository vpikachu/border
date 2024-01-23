namespace border.api.Models
{
    public class Board
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedTime { get; set; }

        public Board()
        {
            Name = "";
            CreatedBy = "";
        }
    }
}