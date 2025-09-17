namespace AiPrReviewer.Models
{
    public class CommentDto
    {
        public string User { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FilePath { get; set; }
    }
}
