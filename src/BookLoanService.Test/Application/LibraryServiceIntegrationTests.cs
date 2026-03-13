namespace BookLoanService.Test.Application;

public class LibraryServiceIntegrationTests
{
    private readonly AppTestFixture _appTestFixture = new();

    private const int CustomerWithNoActiveLoanId = 2;
    private const int CustomerWithActiveLoanId = 4;
    private const int CustomerWithOverdueLoanId = 3;

    private const int AvailableBook1Id = 6;
    private const int AvailableBook2Id = 3;
    private const int AvailableBook3Id = 5;
    private const int ActiveLoanBorrowedBookId = 9;
    private const int OverdueLoanBorrowedBook1Id = 1;
    private const int OverdueLoanBorrowedBook2Id = 8;
    private const int UnknownBookId = 42;

    [Fact]
    public void RepositoryStateShouldPersistAcrossServiceScopes()
    {
        //Arrange & Act
        //scope 1: add a book to the repository
        using (var scope = _appTestFixture.CreateScope())
        {
            var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();

            libraryService.AddBook("Book A", "Author A");
        }

        //Act & Assert
        //scope 2: retrieve books and verify the added book is present
        using (var scope = _appTestFixture.CreateScope())
        {
            var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
            var books = libraryService.GetAllBooks();

            books.Should().ContainSingle(b => b.Title == "Book A");
        }
    }

    [Fact]
    public void BorrowBooksShouldWorkEndToEnd()
    {
        //Arrange
        using var scope = _appTestFixture.CreateScope();
        var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var bookRepo = scope.ServiceProvider.GetRequiredService<IBookRepository>();

        var customer = customerRepository.GetById(CustomerWithNoActiveLoanId)!;
        var initialCount = bookRepo.GetAll().Count;

        //Act
        libraryService.AddBook("Book A", "Author A");

        bookRepo.GetAll().Should().HaveCount(initialCount + 1);

        var command = new BorrowBooksCommand(customer.Id, [initialCount + 1]);

        var result = libraryService.BorrowBooks(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.BorrowedBooks.Should().ContainSingle(b => b.BookId == initialCount + 1);
        customer.BorrowedBooks.Should().ContainSingle(l => l.BookId == initialCount + 1);
    }

    [Fact]
    public void BorrowBooksShouldPersistLoansAcrossServiceScopes()
    {
        //Arrange & Act
        //scope 1 : add books and borrow one book for customer 2
        using (var scope = _appTestFixture.CreateScope())
        {
            var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
            var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            var customer = customerRepository.GetById(CustomerWithNoActiveLoanId);

            customer!.ActiveLoans.Should().HaveCount(0);

            var bookAId = libraryService.AddBook("Book A", "Author A");
            libraryService.GetAllBooks().Last().Id.Should().Be(bookAId);

            var bookBId = libraryService.AddBook("Book B", "Author B");
            libraryService.GetAllBooks().Last().Id.Should().Be(bookBId);

            libraryService.BorrowBooks(new BorrowBooksCommand(CustomerWithNoActiveLoanId, [bookAId]));
            customer.ActiveLoans.Should().HaveCount(1);
        }

        //Act & Assert
        //scope 2 : borrow another book for customer 2 and verify both loans are present
        using (var scope = _appTestFixture.CreateScope())
        {
            var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
            var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            var lastBookId = libraryService.GetAllBooks().Count;

            var result = libraryService.BorrowBooks(new BorrowBooksCommand(CustomerWithNoActiveLoanId, [lastBookId]));

            result.Value.BorrowedBooks.Should().Contain(b => b.BookId == lastBookId);

            var customer = customerRepository.GetById(CustomerWithNoActiveLoanId)!;

            customer.ActiveLoans.Should().HaveCount(2);
        }
    }

    [Fact]
    public void BorrowBooksShouldRejectWhenBookAlreadyLoanedByAnotherCustomer()
    {
        //Arrange
        using var scope = _appTestFixture.CreateScope();
        var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        var bookRepo = scope.ServiceProvider.GetRequiredService<IBookRepository>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

        var customer1 = customerRepository.GetById(CustomerWithNoActiveLoanId)!;
        var customer2 = customerRepository.GetById(CustomerWithActiveLoanId)!;

        var bookAId = libraryService.AddBook("Book A", "Author A");

        //Act
        libraryService.BorrowBooks(new BorrowBooksCommand(customer1.Id, [bookAId]));

        var result = libraryService.BorrowBooks(new BorrowBooksCommand(customer2.Id, [bookAId]));

        //Assert
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.ReasonCode == BorrowRejectionCode.NOT_AVAILABLE.ToString()
        );
    }

    [Fact]
    public void ShouldBorrowOnlyAvailableBooksAndRejectAlreadyOverdueLoanBooksByAnotherCustomer()
    {
        //Arrange
        using var scope = _appTestFixture.CreateScope();
        var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

        var customer = customerRepository.GetById(CustomerWithNoActiveLoanId);
        var command = new BorrowBooksCommand(customer!.Id, [OverdueLoanBorrowedBook1Id, ActiveLoanBorrowedBookId]);

        //Act
        var result = libraryService.BorrowBooks(command);

        //Assert
        result.Value.BorrowedBooks.Should().ContainSingle(b => b.BookId == ActiveLoanBorrowedBookId);
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.BookId == OverdueLoanBorrowedBook1Id &&
            r.ReasonCode == BorrowRejectionCode.NOT_AVAILABLE.ToString()
        );
    }

    [Fact]
    public void BorrowBooksResultShouldContainNotFoundCode()
    {
        //Arrange
        using var scope = _appTestFixture.CreateScope();
        var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

        var customer = customerRepository.GetById(CustomerWithNoActiveLoanId);
        var command = new BorrowBooksCommand(customer!.Id, [UnknownBookId]);

        //Act
        var result = libraryService.BorrowBooks(command);

        //Assert
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.BookId == UnknownBookId &&
            r.ReasonCode == BorrowRejectionCode.NOT_FOUND.ToString()
        );
    }

    [Fact]
    public void BorrowBooksResultShouldRejectWhenBookLoanIsOverdueByAnotherCustomer()
    {
        //Arrange
        using var scope = _appTestFixture.CreateScope();
        var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

        var customer = customerRepository.GetById(CustomerWithNoActiveLoanId);
        var command = new BorrowBooksCommand(customer!.Id, [OverdueLoanBorrowedBook2Id]);

        //Act
        var result = libraryService.BorrowBooks(command);

        //Assert
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.BookId == OverdueLoanBorrowedBook2Id &&
            r.ReasonCode == BorrowRejectionCode.NOT_AVAILABLE.ToString()
        );
    }

    [Fact]
    public void BorrowBooksShouldRejectWhenCustomerHasLoanOverdueBook()
    {
        //Arrange
        using var scope = _appTestFixture.CreateScope();
        var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

        var customer = customerRepository.GetById(CustomerWithOverdueLoanId);
        var command = new BorrowBooksCommand(customer!.Id, [AvailableBook1Id]);

        //Act
        var result = libraryService.BorrowBooks(command);

        //Assert
        result.Value.BorrowedBooks.Should().ContainSingle(b => b.BookId == OverdueLoanBorrowedBook2Id);
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.BookId == AvailableBook1Id &&
            r.ReasonCode == BorrowRejectionCode.LOAN_OVERDUE.ToString()
        );
    }

    [Fact]
    public void BorrowBooksShouldRejectWhenLoanLimitReachedByCustomer()
    {
        //Arrange
        using var scope = _appTestFixture.CreateScope();
        var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

        var customer = customerRepository.GetById(CustomerWithNoActiveLoanId);
        var command = new BorrowBooksCommand(
            customer!.Id, [AvailableBook2Id, ActiveLoanBorrowedBookId, AvailableBook3Id, AvailableBook1Id]);

        //Act
        var result = libraryService.BorrowBooks(command);

        //Assert
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.BookId == AvailableBook1Id &&
            r.ReasonCode == BorrowRejectionCode.LIMIT_REACHED.ToString()
        );
    }
}
