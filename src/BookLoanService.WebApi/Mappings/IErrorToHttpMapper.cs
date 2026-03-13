namespace BookLoanService.WebApi.Mappings;

public interface IErrorToHttpMapper
{
    IActionResult Map<T>(Result<T> result);
}
