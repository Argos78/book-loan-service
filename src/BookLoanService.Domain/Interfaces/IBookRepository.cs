namespace BookLoanService.Domain.Interfaces;

public interface IBookRepository
{
    IReadOnlyCollection<Book> GetAll();

    int Add(Book book);

    bool Exists(int bookId);

    int NextId();
}
