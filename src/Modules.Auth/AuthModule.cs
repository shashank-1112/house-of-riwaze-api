using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Modules.Auth;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AdminJwtService>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var adminJwtService = new AdminJwtService(configuration);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = adminJwtService.GetSigningKey(),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };

            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AdminAuthorization.Policy, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(AdminAuthorization.Role);
            });
        });

        return services;
    }

    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/auth/login", (
            AdminLoginRequest request,
            AdminJwtService adminJwtService) =>
        {
            if (!adminJwtService.ValidateCredentials(request))
            {
                return Results.Unauthorized();
            }

            return Results.Ok(adminJwtService.CreateToken(request.Username));
        })
        .AllowAnonymous()
        .WithTags("Admin Auth");

        return app;
    }
}
