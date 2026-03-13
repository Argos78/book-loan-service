var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.ConfigureValidators()
                .ConfigureSwagger()
                .ConfigureServices()
                .ConfigureApplicationServices()
                .ConfigureRepositories();

//Solution

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
    });
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext http, ILogger<Program> logger) =>
{
    var exception = http.Features.Get<IExceptionHandlerFeature>()?.Error;

    if (exception is not null)
    {
        logger.LogError(exception, "Unhandled exception occurred");
    }

    return Results.Problem("Une erreur inattendue est survenue, veuillez réessayer ultérieurement.");
});

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
