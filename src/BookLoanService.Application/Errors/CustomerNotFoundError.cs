namespace BookLoanService.Application.Errors;

public class CustomerNotFoundError : Error
{
    public const string ErrorMessage = "Le client est introuvable.";

    public CustomerNotFoundError()
        : base(ErrorMessage) { }
}
