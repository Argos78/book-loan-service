namespace BookLoanService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        // NOTE: Repositories registered as Singleton intentionally.
        // They simulate a persistent in-memory database for the technical test purpose :
        // - requests state share
        // - simulating database
        // - avoid to reinitialize data between requests
        // - need to persist added loans according to README.md file indications, etc.
        // In a real application, repositories would be Scoped (e.g., EF Core DbContext).
        return services.AddSingleton<IBookRepository, BookRepository>()
                       .AddSingleton<ICustomerRepository, CustomerRepository>();

    }
}
