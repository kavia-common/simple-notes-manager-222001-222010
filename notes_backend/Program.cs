using NotesBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on port 3001 for HTTP
builder.WebHost.ConfigureKestrel(options =>
{
    // Only set if not already set via environment; this ensures dev preview works
    // In many hosted environments ASPNETCORE_URLS may be used; this does not override it.
    options.ListenAnyIP(3001);
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// NSwag OpenAPI with "Ocean Professional" theme metadata
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Simple Notes Manager API";
    settings.Version = "v1";
    settings.Description = "Modern REST API for managing notes. Blue & amber accents (Ocean Professional).";
    settings.DocumentName = "v1";
    // Note: NSwag's AspNetCoreOpenApiDocumentGeneratorSettings does not support arbitrary AdditionalSettings.
    // Styling hints are captured in description/title only.
});

// Add CORS (permissive for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Dependency injection for repository
builder.Services.AddSingleton<INotesRepository, InMemoryNotesRepository>();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
    config.DocumentTitle = "Simple Notes Manager API Docs";
    config.DocExpansion = "list";
});

// Map controllers and a health endpoint
app.MapControllers();

// Health check endpoint
app.MapGet("/", () => Results.Ok(new { message = "Healthy" }))
   .WithName("HealthCheck")
   .WithTags("System");

app.Run();