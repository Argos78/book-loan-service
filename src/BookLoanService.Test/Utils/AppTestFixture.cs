namespace BookLoanService.Test.Utils;

public class AppTestFixture
{
    private readonly IServiceProvider _serviceProvider;

    public AppTestFixture()
    {
        // NOTE: Singleton because repositories simulate a persistent in-memory DB
        _serviceProvider = new ServiceCollection()
            .AddSingleton<ICustomerRepository, CustomerRepository>()
            .AddSingleton<IBookRepository, BookRepository>()
            .AddScoped<ILibraryService, LibraryService>()
            .AddLogging()
            .BuildServiceProvider();
    }

    public IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }
}
