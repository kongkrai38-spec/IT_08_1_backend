namespace IT_08_1_backend.Models
{
    public class Comment
    {
        public string? UserName { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}