using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrlShortener.App.Backend.Business;

namespace UrlShortener.App.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = CreateWebApplication(args);
            app.Run();
        }

        private static WebApplication CreateWebApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Db connection
            string connectionString = builder.Configuration.GetConnectionString("MsSql")!;
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));

            // Add jwt token generator
            builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            // Add url shortener service
            builder.Services.AddScoped<IMappingsService, MappingsService>();

            // Add redirect log service
            builder.Services.AddScoped<IRedirectLogService, RedirectLogService>();

            // Add user agent service
            builder.Services.AddHttpClient<IUserAgentService, UserAgentService>();

            // Add JWT authentication
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            // Allow all origins for development, change when in production
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    options.AddPolicy("AllowAnyOrigin", policy =>
                    {
                        policy.AllowAnyOrigin() // Allows requests from any origin
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });
            });

            // Add controllers
            builder.Services.AddControllers();

            // Add endpoints
            builder.Services.AddEndpointsApiExplorer();

            // Add openapi documentation
            builder.Services.AddOpenApi();

            var app = builder.Build();

            var serviceScopyFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopyFactory?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<AppDbContext>();
                var databaseProvider = context?.Database.ProviderName;

                if (databaseProvider != "Microsoft.EntityFrameworkCore.InMemory")
                    context?.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseDeveloperExceptionPage();
            }

            // Allow any origin
            app.UseCors("AllowAnyOrigin");

            app.UseHttpsRedirection();

            // Add authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            return app;
        }
    }
}
