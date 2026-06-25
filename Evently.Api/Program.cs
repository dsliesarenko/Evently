using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Modules.Events.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplicationCommon([
    Evently.Modules.Events.Application.AssemblyReference.Assembly,
]);

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

EventsModule.MapEndpoints(app);

app.UseExceptionHandler();

await app.RunAsync();
