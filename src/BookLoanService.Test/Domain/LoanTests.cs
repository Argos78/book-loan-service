namespace BookLoanService.Test.Domain;

public class LoanTests
{
    [Fact]
    public void IsActiveShouldBeTrueWhenReturnedAtIsNotSet()
    {
        var loan = new Loan(
            id: 1,
            customerId: 1,
            bookId: 1,
            borrowedAt: DateTime.UtcNow.AddDays(-1),
            dueAt: DateTime.UtcNow.AddDays(5),
            returnedAt: null
        );

        loan.IsActive.Should().BeTrue();
    }

    [Fact]
    public void IsActiveShouldBeFalseWhenReturnedAtIsSet()
    {
        var loan = new Loan(
            id: 1,
            customerId: 1,
            bookId: 1,
            borrowedAt: DateTime.UtcNow.AddDays(-10),
            dueAt: DateTime.UtcNow.AddDays(-1),
            returnedAt: DateTime.UtcNow.AddDays(1)
        );

        loan.IsActive.Should().BeFalse();
    }

    [Fact]
    public void IsOverdueShouldBeTrueWhenDueAtIsPast()
    {
        var loan = new Loan(
            id: 1,
            customerId: 1,
            bookId: 1,
            borrowedAt: DateTime.UtcNow.AddDays(-10),
            dueAt: DateTime.UtcNow.AddDays(-1),
            returnedAt: null
        );

        loan.IsOverdue.Should().BeTrue();
    }

    [Fact]
    public void IsOverdueShouldBeFalseWhenDueAtIsFuture()
    {
        var loan = new Loan(
            id: 1,
            customerId: 1,
            bookId: 1,
            borrowedAt: DateTime.UtcNow,
            dueAt: DateTime.UtcNow.AddDays(3),
            returnedAt: null
        );

        loan.IsOverdue.Should().BeFalse();
    }
}
