using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StackExchange.Redis;

namespace StunPages.Components.Pages
{
    public partial class Home
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IConfiguration Configuration { get; set; }

        public string? Group { get; set; }

        public async Task GoTo(KeyboardEventArgs args)
        {
            if(args.Code == "Enter" || args.Code == "NumpadEnter")
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration["Redis"]);

                var instanceName = Configuration["InstanceName"];

                var sr = redis.GetServers().FirstOrDefault();
                var keys = sr.Keys(pattern: $"{instanceName}_{Group}_*").ToList();

                if (keys.Count > 0)
                {
                    NavigationManager.NavigateTo($"/p/{Group}");
                }

            }
        }

        public void OnInput(ChangeEventArgs args)
        {
            Group = args.Value.ToString();
        }
    }
}
                                        