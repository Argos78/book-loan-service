namespace BookLoanService.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        return services.AddScoped<ILibraryService, LibraryService>();
    }
}
