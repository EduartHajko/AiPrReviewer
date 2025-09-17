using Microsoft.AspNetCore.Mvc;
using AiPrReviewer.Services;

namespace AiPrReviewer.Controllers
{


    public class PRController : Controller
    {
        private readonly GitHubService _gitHub;
        private readonly AiService _ai;
        private readonly AuditService _audit;

        public PRController(GitHubService gitHub, AiService ai, AuditService audit)
        {
            _gitHub = gitHub;
            _ai = ai;
            _audit = audit;
        }

        public async Task<IActionResult> Index(string owner, string repo)
        {
            var prs = await _gitHub.GetPullRequests(owner, repo);
            ViewBag.Repo = repo;
            return View(prs);
        }
        public async Task<IActionResult> Details(int id)
        {
            var comments = await _gitHub.GetPullRequestDetails("EduartHajko", "InnovationPlatform", id);
            ViewBag.PrNumber = id;
            return View(comments);
        }

        [HttpPost]
        public async Task<IActionResult> SolveComment([FromBody] SolveRequest req)
        {
            try
            {
                var code = await _gitHub.GetFileContent("EduartHajko", "InnovationPlatform", req.FilePath, req.PrNumber);
                var fix = await _ai.SolveComment(req.Comment, code);
                return Json(new { suggestion = fix });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CommitFix([FromBody] CommitRequest req)
        {
            var sha = await _gitHub.CommitFix(
                "EduartHajko",
                "InnovationPlatform",
                req.PrNumber,
                req.FilePath,
                req.FixedCode,
                "AI: Applied reviewer fix"
            );

            _audit.Add(req.Comment, req.FilePath, req.PrNumber, sha);

            return Json(new { status = "success", commitSha = sha });
        }

        public IActionResult AuditLog()
        {
            return View(_audit.GetAll());
        }
    }

    public record SolveRequest(int PrNumber, string FilePath, string Comment);
    public record CommitRequest(int PrNumber, string FilePath, string FixedCode, string Comment);

}
