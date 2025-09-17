using AiPrReviewer.Helpers;
using AiPrReviewer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.Configure<GitHubOptions>(builder.Configuration.GetSection("GitHub"));
builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));


builder.Services.AddScoped<GitHubService>(sp =>
{
    var options = sp.GetRequiredService<
        Microsoft.Extensions.Options.IOptions<GitHubOptions>>().Value;
    return new GitHubService(options.Token); 
});

builder.Services.AddScoped<AiService>(sp =>
{
    var options = sp.GetRequiredService<
        Microsoft.Extensions.Options.IOptions<OpenAIOptions>>().Value;
    return new AiService(options.ApiKey);
});
builder.Services.AddScoped<AuditService>();

var app = builder.Build();

app.MapDefaultControllerRoute();
app.Run();
