using MiniVSA.CatalogService;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddLibraries();

builder.Services
       .AddHealthCheck(builder.Configuration);

builder.Services
       .AddBackingServices(builder.Configuration);

builder.Services
       .AddProjectSettings();

builder.Services
       .AddMappingProfiles();

var app = builder.Build();

app.ConfigureLibraries();

app.ConfigureHealthCheck();

app.ConfigureProjectSettings();

app.Run();
