namespace BookLoanService.Domain.Policies;

public static class BorrowingPolicies
{
    public const int MaxSimultaneousLoans = 3;
    public const int LoanDurationDays = 21;
}
