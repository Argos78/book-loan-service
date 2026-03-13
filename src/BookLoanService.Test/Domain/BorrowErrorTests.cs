namespace BookLoanService.Test.Domain;

public class BorrowErrorTests
{
    [Fact]
    public void BookNotFoundErrorShouldExposeNotFoundBorrowRejectionCode()
    {
        var error = new BookNotFoundError();

        error.Code.Should().Be(BorrowRejectionCode.NOT_FOUND);
    }

    [Fact]
    public void BookUnavailableErrorShouldExposeNotAvailableBorrowRejectionCode()
    {
        var error = new BookUnavailableError();

        error.Code.Should().Be(BorrowRejectionCode.NOT_AVAILABLE);
    }

    [Fact]
    public void LimitReachedErrorShouldExposeLimitReachedBorrowRejectionCode()
    {
        var error = new LimitReachedError();

        error.Code.Should().Be(BorrowRejectionCode.LIMIT_REACHED);
    }

    [Fact]
    public void LoanOverdueErrorShouldExposeLoanOverdueBorrowRejectionCode()
    {
        var error = new LoanOverdueError();

        error.Code.Should().Be(BorrowRejectionCode.LOAN_OVERDUE);
    }
}
