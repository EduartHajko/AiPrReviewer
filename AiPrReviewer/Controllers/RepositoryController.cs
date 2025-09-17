using AiPrReviewer.Helpers;
using AiPrReviewer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AiPrReviewer.Controllers
{
    public class RepositoryController : Controller
    {
        private readonly GitHubService _gitHub;
        private readonly GitHubOptions _options;

        public RepositoryController(GitHubService gitHub, IOptions<GitHubOptions> options)
        {
            _gitHub = gitHub;
            _options = options.Value;
        }

        public async Task<IActionResult> Index()
        {
            var repos = await _gitHub.GetRepositories(_options.Owner);
            return View(repos);
        }
    }
}
