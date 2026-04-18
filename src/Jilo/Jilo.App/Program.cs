using Jilo.App.Extensions;
using Jilo.App.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceContext(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddRepositories();

builder.Services.AddSecurity();

builder.Services.AddJwtTokens(builder.Configuration);

builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddMediatR();

builder.Services.AddValidators();

builder.Services.AddAuthorizationPolicies();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ServiceContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
