namespace BookLoanService.WebApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services.AddScoped<IErrorToHttpMapper, ErrorToHttpMapper>();
    }

    public static IServiceCollection ConfigureValidators(this IServiceCollection services)
    {
        return services.AddFluentValidationAutoValidation()
                       .AddFluentValidationClientsideAdapters()
                       .AddValidatorsFromAssemblyContaining<BorrowBooksRequestValidator>();
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        return services.AddEndpointsApiExplorer()
                       .AddSwaggerGen();
    }
}
