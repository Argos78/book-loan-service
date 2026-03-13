namespace BookLoanService.WebApi.Mappings;

internal class ErrorToHttpMapper : IErrorToHttpMapper
{
    public IActionResult Map<T>(Result<T> result)
    {
        var error = result.Errors.First();

        return error switch
        {
            CustomerNotFoundError e =>
                new NotFoundObjectResult(new ErrorResponse(ErrorCodes.CustomerNotFound, e.Message)),

            _ => new ObjectResult(new ErrorResponse(ErrorCodes.UnexpectedError, ErrorMessages.UnexpectedError))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }

}
