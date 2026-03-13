namespace BookLoanService.Test.Utils;

internal static class LoanTestHelper
{
    private static readonly Random _random = new();

    public static Loan CreateRandomLoan(
        int? customerId = null,
        int? bookId = null,
        bool? isReturned = null,
        bool? isOverdue = null)
    {
        var borrowedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 30));
        var dueAt = borrowedAt.AddDays(_random.Next(1, 20));

        DateTime? returnedAt = null;

        if (isReturned == true)
        {
            returnedAt = dueAt.AddDays(-_random.Next(1, 5));
        }
        else if (isOverdue == true)
        {
            dueAt = DateTime.UtcNow.AddDays(-_random.Next(1, 5));
        }

        return new Loan(
            id: RandomHelper.NextInt(),
            customerId: customerId ?? RandomHelper.NextInt(),
            bookId: bookId ?? RandomHelper.NextInt(),
            borrowedAt: borrowedAt,
            dueAt: dueAt,
            returnedAt: returnedAt
        );
    }

    public static Loan CreateActiveLoan(int? customerId = null, int? bookId = null)
        => CreateRandomLoan(customerId, bookId, isReturned: false, isOverdue: false);

    public static Loan CreateOverdueLoan(int? customerId = null, int? bookId = null)
        => CreateRandomLoan(customerId, bookId, isReturned: false, isOverdue: true);

    public static Loan CreateReturnedLoan(int? customerId = null, int? bookId = null)
        => CreateRandomLoan(customerId, bookId, isReturned: true, isOverdue: false);
}
