namespace AiPrReviewer.Models
{
    public class CommitDto
    {
        public string Sha { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
    }
}
