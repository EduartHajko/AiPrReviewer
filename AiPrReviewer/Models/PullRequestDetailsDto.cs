namespace AiPrReviewer.Models
{
    public class PullRequestDetailsDto
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string State { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<CommentDto> Comments { get; set; } = new();         
        public List<CommentDto> ReviewComments { get; set; } = new();   
        public List<CommitDto> Commits { get; set; } = new();           
    }

}
