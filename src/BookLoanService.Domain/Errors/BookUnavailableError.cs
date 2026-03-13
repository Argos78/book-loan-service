namespace BookLoanService.Domain.Errors;

public class BookUnavailableError() : BorrowError(BorrowRejectionCode.NOT_AVAILABLE) { }
