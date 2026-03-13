namespace BookLoanService.Domain.Entities;

public class Loan(
    int id,
    int customerId,
    int bookId,
    DateTime borrowedAt,
    DateTime dueAt,
    DateTime? returnedAt)
{
    public int Id { get; } = id;

    public int CustomerId { get; } = customerId;

    public int BookId { get; } = bookId;

    public DateTime BorrowedAt { get; } = borrowedAt;

    public DateTime DueAt { get; } = dueAt;

    public DateTime? ReturnedAt { get; } = returnedAt;

    public bool IsActive => ReturnedAt == null;

    public bool IsOverdue => ReturnedAt == null && DueAt < DateTime.UtcNow;

    public static Loan Create(
        int id,
        int customerId,
        int bookId,
        DateTime borrowedAt,
        DateTime dueAt)
    {
        return new Loan(id, customerId, bookId, borrowedAt, dueAt, returnedAt: null);
    }
}
