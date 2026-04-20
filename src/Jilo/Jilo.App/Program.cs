using Jilo.App.Application.Common.Services;
using Jilo.App.Application.Services;
using Jilo.App.Applicatoin.DTO;
using Jilo.App.Extensions;
using Jilo.App.Infrastructure.Persistence;
using Jilo.App.Infrastructure.Persistence.Seeders;
using Jilo.App.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceContext(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddSecurity();

builder.Services.AddJwtTokens(builder.Configuration);

builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddMediatR();

builder.Services.AddValidators();

builder.Services.AddAuthorizationPolicies();

builder.Services.AddControllers();
builder.Services.AddScoped<IPlayerSearchService, PlayerSearchService>();
builder.Services.AddScoped<IPlayerSearchRepository, PlayerSearchRepository>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ServiceContext>();
    context.Database.Migrate();
    await GamesSeeder.InitializeAsync(context);
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
