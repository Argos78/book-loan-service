namespace BookLoanService.Test.Application;

public class LibraryServiceUnitTests
{
    [Fact]
    public void ShouldReturnCustomerNotFoundErrorWhenCustomerDoesNotExist()
    {
        //Arrange 
        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var customerId = RandomHelper.NextInt();
        var book1Id = RandomHelper.NextInt();
        var book2Id = RandomHelper.NextInt();

        customerRepository.Setup(r => r.GetById(customerId)).Returns((Customer?)null);

        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);
        var command = new BorrowBooksCommand(customerId, [book1Id, book2Id]);

        //Act
        var result = sut.BorrowBooks(command);

        //Assert
        var error = result.Errors.Should()
            .ContainSingle()
            .Which
            .Should()
            .BeOfType<CustomerNotFoundError>()
            .Subject;

        error.Message.Should().Be(CustomerNotFoundError.ErrorMessage);
    }

    [Fact]
    public void GetAllBooksShouldReturnBooksFromRepository()
    {
        //Arrange
        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);

        var books = new List<Book>
        {
            new Book(RandomHelper.NextInt(), "A", "AA"),
            new Book(RandomHelper.NextInt(), "B", "BB")
        };

        bookRepository.Setup(r => r.GetAll()).Returns(books);

        //Act
        var result = sut.GetAllBooks();

        //Assert
        result.Should().BeEquivalentTo(books);
    }

    [Fact]
    public void BorrowBooksShouldFailWhenCustomerIsNotFound()
    {
        //Arrange

        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var customerId = RandomHelper.NextInt();
        var bookId = RandomHelper.NextInt();

        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);

        customerRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns((Customer?)null);

        //Act
        var result = sut.BorrowBooks(new BorrowBooksCommand(customerId, [bookId]));

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e is CustomerNotFoundError);
    }

    [Fact]
    public void BorrowBooksShouldRejectWhenBookDoesNotExist()
    {
        //Arrange
        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var customerId = RandomHelper.NextInt();
        var bookId = RandomHelper.NextInt();

        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);

        var customer = new Customer(customerId, "John", []);
        customerRepository.Setup(r => r.GetById(customerId)).Returns(customer);

        bookRepository.Setup(r => r.Exists(bookId)).Returns(false);
        customerRepository.Setup(r => r.GetAllActiveLoans()).Returns([]);

        //Act
        var result = sut.BorrowBooks(new BorrowBooksCommand(customerId, [bookId]));

        //Assert
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.BookId == bookId &&
            r.ReasonCode == BorrowRejectionCode.NOT_FOUND.ToString()
        );
    }

    [Fact]
    public void BorrowBooksShouldRejectWhenCustomerHasOverdueLoan()
    {
        //Arrange
        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var customerId = RandomHelper.NextInt();
        var bookOverdueId = RandomHelper.NextInt();
        var bookAvailableId = RandomHelper.NextInt();
        var loanId = RandomHelper.NextInt();

        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);

        var overdue = new Loan(loanId, customerId, bookOverdueId, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-5), null);
        var customer = new Customer(customerId, "John", [overdue]);

        customerRepository.Setup(r => r.GetById(customerId)).Returns(customer);
        bookRepository.Setup(r => r.Exists(It.IsAny<int>())).Returns(true);
        customerRepository.Setup(r => r.GetAllActiveLoans()).Returns([]);

        //Act
        var result = sut.BorrowBooks(new BorrowBooksCommand(customerId, [bookAvailableId]));

        //Assert
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.ReasonCode == BorrowRejectionCode.LOAN_OVERDUE.ToString()
        );
    }

    [Fact]
    public void BorrowBooksShouldRejectWhenLoanLimitIsReached()
    {
        //Arrange
        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var customerId = RandomHelper.NextInt();

        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);

        var customer = new Customer(1, "John", []);

        for (int i = 1; i <= BorrowingPolicies.MaxSimultaneousLoans; i++)
        {
            customer.BorrowBook(i, DateTime.UtcNow.AddDays(5));
        }

        customerRepository.Setup(r => r.GetById(customerId)).Returns(customer);
        bookRepository.Setup(r => r.Exists(It.IsAny<int>())).Returns(true);
        customerRepository.Setup(r => r.GetAllActiveLoans()).Returns([]);

        //Act
        var result = sut.BorrowBooks(new BorrowBooksCommand(customerId, [99]));

        //Assert
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.ReasonCode == BorrowRejectionCode.LIMIT_REACHED.ToString()
        );
    }

    [Fact]
    public void BorrowBooksShouldRejectWhenBookIsAlreadyLoaned()
    {
        //Arrange
        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var customerId = RandomHelper.NextInt();
        var bookId = RandomHelper.NextInt();
        var loanId = RandomHelper.NextInt();

        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);

        var customer = new Customer(customerId, "John", []);
        customerRepository.Setup(r => r.GetById(customerId)).Returns(customer);

        bookRepository.Setup(r => r.Exists(bookId)).Returns(true);
        customerRepository.Setup(r => r.GetAllActiveLoans()).Returns(new List<Loan>
        {
            new Loan(loanId, customerId, bookId, DateTime.UtcNow, DateTime.UtcNow.AddDays(5), null)
        });

        //Act
        var result = sut.BorrowBooks(new BorrowBooksCommand(customerId, [bookId]));

        //Assert
        result.Value.RejectedBooks.Should().ContainSingle(r =>
            r.ReasonCode == BorrowRejectionCode.NOT_AVAILABLE.ToString()
        );
    }

    [Fact]
    public void BorrowBooksShouldBorrowWhenAllConditionsAreMet()
    {
        //Arrange
        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var customerId = RandomHelper.NextInt();
        var bookId = RandomHelper.NextInt();

        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);

        var customer = new Customer(customerId, "John", []);
        customerRepository.Setup(r => r.GetById(customerId)).Returns(customer);

        bookRepository.Setup(r => r.Exists(bookId)).Returns(true);
        customerRepository.Setup(r => r.GetAllActiveLoans()).Returns([]);

        //Act
        var result = sut.BorrowBooks(new BorrowBooksCommand(customerId, [bookId]));

        //Assert
        result.Value.BorrowedBooks.Should().ContainSingle(b => b.BookId == bookId);
        customer.BorrowedBooks.Should().ContainSingle(l => l.BookId == bookId);
    }

    [Fact]
    public void BorrowBooksShouldMixBorrowedAndRejected()
    {
        //Arrange
        var bookRepository = new Mock<IBookRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var customerId = RandomHelper.NextInt();
        var book1Id = RandomHelper.NextInt();
        var book2Id = RandomHelper.NextInt();

        var sut = new LibraryService(bookRepository.Object, customerRepository.Object);

        var customer = new Customer(customerId, "John", []);
        customerRepository.Setup(r => r.GetById(customerId)).Returns(customer);

        bookRepository.Setup(r => r.Exists(book1Id)).Returns(true);
        bookRepository.Setup(r => r.Exists(book2Id)).Returns(false);
        customerRepository.Setup(r => r.GetAllActiveLoans()).Returns([]);

        //Act
        var result = sut.BorrowBooks(new BorrowBooksCommand(customerId, [book1Id, book2Id]));

        //Assert
        result.Value.BorrowedBooks.Should().ContainSingle(b => b.BookId == book1Id);
        result.Value.RejectedBooks.Should().ContainSingle(r => r.BookId == book2Id);
    }
}
