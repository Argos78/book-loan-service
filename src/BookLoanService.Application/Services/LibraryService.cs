namespace BookLoanService.Application.Services;

internal class LibraryService : ILibraryService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICustomerRepository _customerRepository;

    public LibraryService(
        IBookRepository bookRepository,
        ICustomerRepository customerRepository)
    {
        _bookRepository = bookRepository;
        _customerRepository = customerRepository;
    }

    public IReadOnlyCollection<Book> GetAllBooks()
    {
        return _bookRepository.GetAll();
    }

    public int AddBook(string title, string author)
    {
        var id = _bookRepository.NextId();
        var book = Book.Create(id, title, author);

        _bookRepository.Add(book);

        return id;
    }

    public Result<BorrowedBooksResult> BorrowBooks(BorrowBooksCommand command)
    {
        var borrowed = new List<BorrowedBook>();
        var rejected = new List<RejectedBook>();

        var customer = _customerRepository.GetById(command.CustomerId);

        if (customer is null)
        {
            return Result.Fail(new CustomerNotFoundError());
        }

        borrowed.AddRange(customer.ActiveLoans.Select(l => l.ToBorrowedBook()));

        var allActiveLoanedBookIds = _customerRepository
            .GetAllActiveLoans()
            .Select(l => l.BookId)
            .ToHashSet();

        foreach (var bookId in command.BookIds)
        {
            var result = BorrowBook(customer, bookId, allActiveLoanedBookIds);

            if (result.IsSuccess)
            {
                borrowed.Add(result.Value);
            }
            else
            {
                var error = result.Errors.OfType<BorrowError>().First();
                var message = BorrowErrorMessages.GetMessage(error);

                rejected.Add(new RejectedBook(
                    bookId,
                    error.Code.ToString(),
                    message
                ));
            }
        }

        return Result.Ok(new BorrowedBooksResult(borrowed, rejected));
    }

    private Result<BorrowedBook> BorrowBook(Customer customer, int bookId, IReadOnlySet<int> activeLoanedBookIds)
    {
        if (!_bookRepository.Exists(bookId))
            return Result.Fail(new BookNotFoundError());

        if (activeLoanedBookIds.Contains(bookId))
            return Result.Fail(new BookUnavailableError());

        if (customer.BorrowedBooks.Any(l => l.IsOverdue))
            return Result.Fail(new LoanOverdueError());

        var activeLoansCount = customer.BorrowedBooks.Count(l => l.IsActive);

        if (activeLoansCount >= BorrowingPolicies.MaxSimultaneousLoans)
            return Result.Fail(new LimitReachedError());

        var dueAt = DateTime.UtcNow.AddDays(BorrowingPolicies.LoanDurationDays);

        //simulates loan persistance here since there is no dedicated repository
        customer.BorrowBook(bookId, dueAt);

        return Result.Ok(new BorrowedBook(bookId, dueAt));
    }
}
