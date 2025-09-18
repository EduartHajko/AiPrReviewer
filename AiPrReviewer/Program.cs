using AiPrReviewer.Data;
using AiPrReviewer.Helpers;
using AiPrReviewer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
// Bind GitHub options
builder.Services.Configure<GitHubOptions>(options =>
{
    builder.Configuration.GetSection("GitHub").Bind(options);
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Load tokens (from User Secrets or env variables)
var githubToken = builder.Configuration["GitHub:Token"];
var openAiKey = builder.Configuration["OpenAI:ApiKey"];

builder.Services.AddSingleton(new GitHubService(githubToken));
builder.Services.AddSingleton(new AiService(openAiKey));
builder.Services.AddScoped<AuditService>();

var app = builder.Build();
// Enable serving static files (css, js, images, bootstrap, etc.)
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
