using InterviewQGen.Web.Components;
using InterviewQGen.App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Register IQuestionGenerator for DI, providing an empty string for the constructor
builder.Services.AddSingleton<IQuestionGenerator>(sp => new LocalAIQuestionGenerator(""));

var app = builder.Build();

// Enable interactive server render mode
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.Run();
