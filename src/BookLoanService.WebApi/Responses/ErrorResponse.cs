namespace BookLoanService.WebApi.Responses;

public class ErrorResponse
{
    public string Code { get; init; }
    public string Message { get; init; }

    public ErrorResponse(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
