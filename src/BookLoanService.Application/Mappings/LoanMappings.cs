namespace BookLoanService.Application.Mappings;

public static class LoanMappings
{
    public static BorrowedBook ToBorrowedBook(this Loan loan)
        => new BorrowedBook(loan.BookId, loan.DueAt);
}
