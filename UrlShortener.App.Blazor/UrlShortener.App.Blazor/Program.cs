using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tailwind;
using Microsoft.AspNetCore.Components.Authorization;
using UrlShortener.App.Blazor.Client.Api;
using UrlShortener.App.Blazor.Client.Business;

namespace UrlShortener.App.Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Add blazorise ui services
        builder.Services
            .AddBlazorise(options =>
            {
                options.Immediate = true;
            })
            .AddTailwindProviders()
            .AddFontAwesomeIcons();

        // Add cascading auth state
        builder.Services.AddCascadingAuthenticationState();

        // Register apis
        builder.Services.AddTransient<IAuthApi, AuthApi>();

        // Register services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();
        builder.Services.AddScoped<IMappingsService, MappingsService>();
        builder.Services.AddScoped<TimeProvider, BrowserTimeProvider>();

        // Add http clients
        var backendSection = builder.Configuration.GetSection("Backend");
        var backendUrl = backendSection.GetSection("BaseUrl").Value ?? throw new InvalidOperationException("The backend base URL is not configured. Please set the 'Backend:BaseUrl' in the configuration.");

        builder.Services.AddHttpClient<IAuthApi, AuthApi>().ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(backendUrl);
        });
        builder.Services.AddHttpClient<IMappingsService, MappingsService>().ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(backendUrl);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        app.Run();
    }
}
