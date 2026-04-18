using FluentValidation;
using Jilo.App.Api.Authorization;
using Jilo.App.Api.Authorization.Handlers;
using Jilo.App.Api.Authorization.Requirements;
using Jilo.App.Applicatoin.Behaviors;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Applicatoin.Common.Services;
using Jilo.App.Infrastructure.Persistence;
using Jilo.App.Infrastructure.Persistence.Repositories;
using Jilo.App.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Jilo.App.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ServiceContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgresDatabase"));
        });

        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

        return services;
    }

    public static IServiceCollection AddJwtTokens(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        services.AddScoped<ITokenHasher, SHA256TokenHasher>();

        return services;
    }

    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JwtOptions:Issuer"],
                    ValidAudience = configuration["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
                cfg.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("access_token", out var token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

            cfg.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();

        return services;
    }

    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(cfg =>
        {
            cfg.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.ProfileOwner, policy => policy.AddRequirements(new ProfileOwnerRequirement()));

        services.AddTransient<IAuthorizationHandler, ProfileOwnerRequirementHandler>();

        return services;
    }
}
