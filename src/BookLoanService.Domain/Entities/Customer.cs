namespace BookLoanService.Domain.Entities;

public class Customer(int id, string name, IReadOnlyCollection<Loan> borrowedBooks)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public IReadOnlyCollection<Loan> ActiveLoans => [..BorrowedBooks.Where(l => l.IsActive)];

    private readonly List<Loan> _borrowedBooks = [..borrowedBooks];

    public IReadOnlyCollection<Loan> BorrowedBooks => _borrowedBooks.AsReadOnly();

    //simulates loan persistance here since there is no dedicated repository
    //on purpose as per requirements in the README.md file
    public void BorrowBook(int bookId, DateTime dueAt)
    {
        var loanId = NextLoanId();
        var loan = Loan.Create(
            id: loanId,
            customerId: Id,
            bookId: bookId,
            borrowedAt: DateTime.UtcNow,
            dueAt: dueAt);

        AddLoan(loan);
    }

    private void AddLoan(Loan loan)
    {
        _borrowedBooks.Add(loan);
    }

    private int NextLoanId()
    {
        return BorrowedBooks.Count == 0
            ? 1
            : BorrowedBooks.Max(l => l.Id) + 1;
    }
}
