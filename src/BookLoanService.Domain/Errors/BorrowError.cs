namespace BookLoanService.Domain.Errors;

public abstract class BorrowError(BorrowRejectionCode code) : Error(code.ToString())
{
    public BorrowRejectionCode Code { get; } = code;
}
