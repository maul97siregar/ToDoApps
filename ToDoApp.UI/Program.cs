using ToDoApp.UI;
using ToDoApp.UI.Components;
using ToDoApp.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();

builder.Services.AddServerSideBlazor()
        .AddCircuitOptions(options => { options.DetailedErrors = true; });



var apiUrl = Environment.GetEnvironmentVariable("TODO_API_URL");

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

if (string.IsNullOrEmpty(apiUrl))
{
    apiUrl = builder.Configuration["API_URL"];
}
if (string.IsNullOrEmpty(apiUrl))
{
    apiUrl = "http://localhost:8080";
}

builder.Services.AddSingleton<ApiEndpoints>(prov => new ApiEndpoints($"{apiUrl}/api"));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ToDoService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
