using AiPrReviewer.Models;
using Octokit;
using ProductHeaderValue = Octokit.ProductHeaderValue;


namespace AiPrReviewer.Services
{


    public class GitHubService
    {
        private readonly GitHubClient _client;

        public GitHubService(string token)
        {
            _client = new GitHubClient(new ProductHeaderValue("AiPrReviewerMVC"))
            {
                Credentials = new Credentials(token)
            };
        }

        public async Task<IReadOnlyList<PullRequest>> GetPullRequests(string owner, string repo)
        {
            return await _client.PullRequest.GetAllForRepository(owner, repo);
        }
        public async Task<IReadOnlyList<Repository>> GetRepositories(string owner)
        {
            var repos = await _client.Repository.GetAllForUser(owner);

            return repos;
        }
        public async Task<PullRequestDetailsDto> GetPullRequestDetails(string owner, string repo, int prNumber)
        {
            var pr = await _client.PullRequest.Get(owner, repo, prNumber);

            // Get issue comments
            var issueComments = await _client.Issue.Comment.GetAllForIssue(owner, repo, prNumber);

            // Get review summaries (Approve / Request Changes / Comment)
            var reviews = await _client.PullRequest.Review.GetAll(owner, repo, prNumber);

            var reviewComments = reviews
                .Where(r => !string.IsNullOrEmpty(r.Body))
                .Select(r => new CommentDto
                {
                    User = r.User.Login,
                    Body = r.Body,
                    CreatedAt = r.SubmittedAt.UtcDateTime,
                    FilePath = null
                })
                .ToList();

            //  Merge all comments
            var allComments = issueComments.Select(c => new CommentDto
            {
                User = c.User.Login,
                Body = c.Body,
                CreatedAt = c.CreatedAt.DateTime,
                FilePath = null
            }).Concat(reviewComments).ToList();

            // Get commits
            var commits = await _client.PullRequest.Commits(owner, repo, prNumber);

            return new PullRequestDetailsDto
            {
                Number = pr.Number,
                Title = pr.Title,
                Body = pr.Body,
                State = pr.State.StringValue,
                Author = pr.User.Login,
                CreatedAt = pr.CreatedAt.DateTime,
                Comments = allComments ?? new List<CommentDto>(),      
                ReviewComments = reviewComments ?? new List<CommentDto>(), 
                Commits = commits.Select(c => new CommitDto
                {
                    Sha = c.Sha,
                    Message = c.Commit.Message,
                    Author = c.Commit.Author.Name,
                    Date = c.Commit.Author.Date.DateTime
                }).ToList()
            };
        }



        public async Task<string> GetFileContent(string owner, string repo, string path, int prNumber)
        {
            var file = await _client.Repository.Content.GetAllContentsByRef(owner, repo, path, $"refs/pull/{prNumber}/head");
            return file[0].Content;
        }

        public async Task<string> CommitFix(string owner, string repo, int prNumber, string path, string fixedCode, string commitMessage)
        {
            // Get PR head branch
            var pr = await _client.PullRequest.Get(owner, repo, prNumber);
            var branch = pr.Head.Ref;

            // Get latest commit
            var reference = await _client.Git.Reference.Get(owner, repo, $"heads/{branch}");
            var latestCommit = await _client.Git.Commit.Get(owner, repo, reference.Object.Sha);

            // Blob for new content
            var blob = new NewBlob { Encoding = EncodingType.Utf8, Content = fixedCode };
            var blobRef = await _client.Git.Blob.Create(owner, repo, blob);

            // Tree
            var tree = new NewTree { BaseTree = latestCommit.Tree.Sha };
            tree.Tree.Add(new NewTreeItem
            {
                Path = path,
                Mode = "100644",
                Type = TreeType.Blob,
                Sha = blobRef.Sha
            });
            var createdTree = await _client.Git.Tree.Create(owner, repo, tree);

            // Commit
            var newCommit = new NewCommit(commitMessage, createdTree.Sha, reference.Object.Sha);
            var commit = await _client.Git.Commit.Create(owner, repo, newCommit);

            // Update branch
            await _client.Git.Reference.Update(owner, repo, $"heads/{branch}", new ReferenceUpdate(commit.Sha));

            return commit.Sha;
        }
    }

}
