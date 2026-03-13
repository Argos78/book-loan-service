namespace BookLoanService.Domain.Errors;

public class LimitReachedError() : BorrowError(BorrowRejectionCode.LIMIT_REACHED) { }

