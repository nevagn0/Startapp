using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.Common.Services;
using Jilo.App.Application.Services;
using Jilo.App.Extensions;
using Jilo.App.Infrastructure.Persistence;
using Jilo.App.Infrastructure.Persistence.Seeders;
using Jilo.App.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceContext(builder.Configuration);

builder.Services.AddRepositories();

builder.Services.AddSecurity();

builder.Services.AddJwtTokens(builder.Configuration);

builder.Services.AddMediatR();

builder.Services.AddValidators();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IUserGameService, UserGameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IUserGameRepository, UserGameRepository>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
