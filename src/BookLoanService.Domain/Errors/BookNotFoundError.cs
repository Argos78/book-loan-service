namespace BookLoanService.Domain.Errors;

public class BookNotFoundError() : BorrowError(BorrowRejectionCode.NOT_FOUND) { }
