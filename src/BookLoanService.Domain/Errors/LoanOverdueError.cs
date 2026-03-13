namespace BookLoanService.Domain.Errors;

public class LoanOverdueError() : BorrowError(BorrowRejectionCode.LOAN_OVERDUE) { }
