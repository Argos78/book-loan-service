namespace BookLoanService.Application.Interfaces;

public interface ILibraryService
{
    int AddBook(string Title, string Author);

    IReadOnlyCollection<Book> GetAllBooks();

    Result<BorrowedBooksResult> BorrowBooks(BorrowBooksCommand command);
}
