using AiPrReviewer.Models;

namespace AiPrReviewer.Data
{
    public static class DataSeeder
    {
        public static void SeedAuditLogs(AppDbContext db)
        {
                var fakeLogs = new List<AuditLogEntry>();

                var sampleComments = new[]
                {
                "Rename method to follow PascalCase convention.",
                "Replace magic number with a constant.",
                "Use async/await instead of blocking .Result call.",
                "Add null-check for input parameter.",
                "Refactor to use dependency injection instead of new().",
                "Extract this logic into a private method for readability.",
                "Add XML documentation for public method.",
                "Consider using StringBuilder for concatenation in loop.",
                "Remove unused using statements.",
                "Mark this method as async and return Task instead of void.",
                "Use nameof() operator instead of hard-coded string.",
                "Validate model state before saving to database.",
                "Use ILogger instead of Console.WriteLine.",
                "Apply Single Responsibility Principle (SRP) here.",
                "Avoid catching generic Exception, be more specific.",
                "Consider using LINQ Any() instead of Count() > 0.",
                "Use var keyword for implicit typing.",
                "Make this method static since it doesn’t depend on instance members.",
                "Dispose DbContext properly with using statement.",
                "Add unit test for this edge case."
            };

                var sampleCodeSnippets = new[]
                {
                "public void calculatevalue(int x) { return x*2; }",
                "int total = 0; for(int i=0;i<100;i++){ total += i; }",
                "var result = httpClient.GetAsync(url).Result;",
                "public string GetName(User user) { return user.Name; }",
                "var service = new EmailService(); service.Send();",
                "if(order != null && order.Items.Count > 0) { Save(order); }",
                "Console.WriteLine(\"Error occurred\");",
                "string data = a + b + c;",
                "if(obj != null) return obj.Id; else return 0;",
                "public void Process(){ Thread.Sleep(5000); }",
                "logger.Log(\"User logged in\");",
                "if(count > 0) return true; else return false;",
                "public class myclass { }",
                "catch(Exception ex){ throw ex; }",
                "if(user.Role == \"Admin\") { /* logic */ }",
                "var x = new List<int>(); if(x.Count() > 0){}",
                "if(isValid == true) DoWork();",
                "public void Save(){ dbContext.SaveChanges(); }",
                "File.WriteAllText(\"data.txt\", content);",
                "public void Start(){ Task.Run(()=> DoWork()); }"
            };

                for (int i = 1; i <= 30; i++)
                {
                    fakeLogs.Add(new AuditLogEntry
                    {
                        PrNumber = i,
                        PrTitle = $"Refactor PR #{i}",
                        FilePath = $"Controllers/Controller{i}.cs",
                        CommitSha = Guid.NewGuid().ToString("N").Substring(0, 7),
                        Committer = (i % 2 == 0) ? "EduartHajko" : "MarcoDev",
                        CodeSnippet = sampleCodeSnippets[i % sampleCodeSnippets.Length],
                        CreatedAt = DateTime.UtcNow.AddMinutes(-i * 15)
                    });
                }

                db.AuditLogs.AddRange(fakeLogs);
                db.SaveChanges();
            
        }
    }

}
