using AiPrReviewer.Helpers;
using AiPrReviewer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
// Bind GitHub options
builder.Services.Configure<GitHubOptions>(options =>
{
    builder.Configuration.GetSection("GitHub").Bind(options);
});
// Load tokens (from User Secrets or env variables)
var githubToken = builder.Configuration["GitHub:Token"];
var openAiKey = builder.Configuration["OpenAI:ApiKey"];

builder.Services.AddSingleton(new GitHubService(githubToken));
builder.Services.AddSingleton(new AiService(openAiKey));
builder.Services.AddSingleton<AuditService>();

var app = builder.Build();
// Enable serving static files (css, js, images, bootstrap, etc.)
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
