using Microsoft.AspNetCore.Components;
using StackExchange.Redis;
using StunPages.Data;
using System.Text.Json;

namespace StunPages.Components.Pages
{
    public partial class Pages
    {
        [Inject]
        public IConfiguration Configuration { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string? Group { get; set; }

        public List<StunAppInfo> Apps { get; set; } = new List<StunAppInfo>();

        protected override async Task OnInitializedAsync()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration["Redis"]);

            var instanceName = Configuration["InstanceName"];

            var sr = redis.GetServers().FirstOrDefault();
            var keys = sr.Keys(pattern: $"{instanceName}_{Group}_*").ToList();

            var db = redis.GetDatabase();

            keys.ForEach(p =>
                {
                    var jsonData = db.HashGet(p, "data");
                    Apps.Add(JsonSerializer.Deserialize<StunAppInfo>(jsonData.ToString()));
                }
            );

            await base.OnInitializedAsync();
        }

        public void GoTo(StunAppInfo appInfo)
        {
            if(appInfo.Protocol?.ToLower() == "https")
            {
                NavigationManager.NavigateTo($"https://{appInfo.Ip}:{appInfo.Port}");
            }
            else
                NavigationManager.NavigateTo($"http://{appInfo.Ip}:{appInfo.Port}");
        }
    }
}
