using Microsoft.AspNetCore.Mvc;
using AiPrReviewer.Services;

namespace AiPrReviewer.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuditService _audit;
        private readonly AiService _ai;

        public AuthorController(AuditService audit, AiService ai)
        {
            _audit = audit;
            _ai = ai;
        }

        public IActionResult Index()
        {
            ViewData["ActiveTab"] = "Authors";
            // Get unique authors from audit log
            var authors = _audit.GetAll()
                                .Select(x => x.Committer)
                                .Distinct()
                                .OrderBy(x => x)
                                .ToList();

            ViewBag.Authors = authors;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Evaluate(string author)
        {
            ViewData["ActiveTab"] = "Authors";
            var entries = _audit.GetAll()
                                .Where(x => x.Committer == author)
                                .ToList();

            if (!entries.Any())
            {
                ViewBag.Author = author;
                ViewBag.Analysis = $"⚠️ No audit log data found for {author}.";
                return View("Evaluation");
            }

            var joinedComments = string.Join("\n---\n", entries.Select(x =>
                $"PR: {x.PrTitle}\nComment: {x.Comment}\nCode:\n{x.CodeSnippet}\n"));

            var prompt = $"""
                        You are a senior software reviewer and mentor.
                        Analyze the following pull request review comments and code fixes for author: {author}.

                        Data:
                        {joinedComments}

                        Task:
                        1. Detect the main programming languages and technologies used.
                        2. Identify the author’s strengths.
                        3. Identify areas for improvement.
                        4. Recommend tutorials, documentation, or courses for those specific technologies.
                        5. Provide a personalized improvement plan.
                    """;

            var analysis = await _ai.GetPerformanceAnalysis(prompt);

            ViewBag.Author = author;
            ViewBag.Analysis = analysis;

            return View("Evaluation");
        }

        [HttpPost]
        public IActionResult ExportToPdf(string author, string analysis)
        {
            using var ms = new MemoryStream();
            var doc = new iTextSharp.text.Document();
            iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
            doc.Open();
            doc.Add(new iTextSharp.text.Paragraph($"Performance Report for {author}"));
            doc.Add(new iTextSharp.text.Paragraph(DateTime.Now.ToString("f")));
            doc.Add(new iTextSharp.text.Paragraph("\nAnalysis:"));
            doc.Add(new iTextSharp.text.Paragraph(analysis));
            doc.Close();

            return File(ms.ToArray(), "application/pdf", $"PerformanceReport_{author}.pdf");
        }
    }
}
