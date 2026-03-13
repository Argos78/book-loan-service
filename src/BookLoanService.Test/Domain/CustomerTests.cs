namespace BookLoanService.Test.Domain;

public class CustomerTests
{
    [Fact]
    public void BorrowBookShouldAddLoan()
    {
        // Arrange
        var customerId = RandomHelper.NextInt();
        var bookId = RandomHelper.NextInt();
        var customer = new Customer(customerId, "John", []);
        var dueAt = DateTime.UtcNow.AddDays(7);

        // Act
        customer.BorrowBook(bookId, dueAt);

        // Assert
        customer.BorrowedBooks.Should().ContainSingle(l =>
            l.BookId == bookId &&
            l.DueAt == dueAt
        );
    }

    [Fact]
    public void ActiveLoansShouldReturnOnlyActiveLoans()
    {
        // Arrange
        var customerId = RandomHelper.NextInt();

        var activeLoanBookId = RandomHelper.NextInt();

        var pastLoan = LoanTestHelper.CreateReturnedLoan(customerId);
        var overdueLoan = LoanTestHelper.CreateOverdueLoan(customerId);
        var activeLoan = LoanTestHelper.CreateActiveLoan(customerId, activeLoanBookId);

        var customer = new Customer(customerId, "John", [pastLoan, activeLoan, overdueLoan]);

        // Act
        customer.BorrowBook(RandomHelper.NextInt(), DateTime.UtcNow.AddDays(5));   // active
        customer.BorrowBook(RandomHelper.NextInt(), DateTime.UtcNow.AddDays(-1));  // overdue

        // Assert
        customer.ActiveLoans.Should().ContainSingle(l => l.BookId == activeLoanBookId);
    }

    [Fact]
    public void HasOverdueLoansShouldBeTrueWhenAnyLoanIsOverdue()
    {
        // Arrange
        var customerId = RandomHelper.NextInt();

        var pastLoan = LoanTestHelper.CreateReturnedLoan(customerId);
        var overdueLoan = LoanTestHelper.CreateOverdueLoan(customerId);
        var activeLoan = LoanTestHelper.CreateActiveLoan(customerId);

        var customer = new Customer(customerId, "John", [pastLoan, activeLoan, overdueLoan]);

        // Act
        customer.BorrowBook(RandomHelper.NextInt(), DateTime.UtcNow.AddDays(-3)); // overdue

        // Assert
        customer.BorrowedBooks.Any(l => l.IsOverdue).Should().BeTrue();
    }
}
