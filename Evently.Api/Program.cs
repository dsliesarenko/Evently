using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Events.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplicationCommon([AssemblyReference.Assembly]);

string dbConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Cache")!;

builder.Services.AddInfrastructure(dbConnectionString, redisConnectionString);

builder.Configuration.AddModuleConfiguration(["events"]);

builder.Services.AddHealthChecks().AddNpgSql(dbConnectionString).AddRedis(redisConnectionString);

builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });

    app.ApplyMigrations();
}

app.MapEndpoints();

app.UseExceptionHandler();

await app.RunAsync();
