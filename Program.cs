using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using StunPages.Components;
using StunPages.Data;
using System.Text.Json;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapPost("/api/stun/{group}", (string group,  StunAppInfo  request,  IConfiguration config) => {
    try
    {
        if (group == "{group}")
        {
            return new {
                Message = "invalid group",
                Success = false
            };
        }

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config["Redis"]);

        var instanceName = config["InstanceName"];
        var db = redis.GetDatabase();

        db.HashSet($"{instanceName}_{group}_{request.Key}", new HashEntry[] {
            new HashEntry("data", JsonSerializer.Serialize(request)) }
            );
        return new
        {
            Message = "",
            Success = true
        };
    }
    catch (Exception ex)
    {
        return new {
            Message = ex.Message,
            Success = false
        };
    }
});

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
