namespace BookLoanService.Test.Application;

public class BorrowErrorMessagesTests
{
    [Fact]
    public void GetMessageShouldReturnNotFoundErrorMessageWhenNotFoundErrorIsEncountered()
    {
        //Arrange
        var error = new FakeNotFoundError();

        //Act
        var message = BorrowErrorMessages.GetMessage(error);

        //Assert
        message.Should().Be(BorrowErrorMessages.NotFound);
    }

    [Fact]
    public void GetMessageShouldReturnNotAvailableErrorMessageWhenNotAvailableErrorIsEncountered()
    {
        //Arrange
        var error = new FakeNotAvailableError();

        //Act
        var message = BorrowErrorMessages.GetMessage(error);

        //Asssert
        message.Should().Be(BorrowErrorMessages.NotAvailable);
    }

    [Fact]
    public void GetMessageShouldReturnLimitReachedMessageWhenLimitReachedErrorIsEncountered()
    {
        //Arrange
        var error = new FakeLimitReachedError();

        //Act
        var message = BorrowErrorMessages.GetMessage(error);

        //Assert
        message.Should().Be(BorrowErrorMessages.LimitReached);
    }

    [Fact]
    public void GetMessageShouldReturnLoanOverdueMessageWhenLoanOverdueErrorIsEncountered()
    {
        //Act
        var error = new FakeLoanOverdueError();

        //Arrange
        var message = BorrowErrorMessages.GetMessage(error);

        //Assert
        message.Should().Be(BorrowErrorMessages.LoanOverdue);
    }

    [Fact]
    public void GetMessageShouldReturnUnknownErrorMessageWhenWhenErrorEncounteredIsUnknown()
    {
        //Act
        var error = new FakeUnknownError();

        //Arrange
        var message = BorrowErrorMessages.GetMessage(error);

        //Assert
        message.Should().Be(BorrowErrorMessages.UnknownError);
    }

    private class FakeNotFoundError : BorrowError
    {
        public FakeNotFoundError() : base(BorrowRejectionCode.NOT_FOUND) { }
    }

    private class FakeNotAvailableError : BorrowError
    {
        public FakeNotAvailableError() : base(BorrowRejectionCode.NOT_AVAILABLE) { }
    }

    private class FakeLoanOverdueError : BorrowError
    {
        public FakeLoanOverdueError() : base(BorrowRejectionCode.LOAN_OVERDUE) { }
    }

    private class FakeLimitReachedError : BorrowError
    {
        public FakeLimitReachedError() : base(BorrowRejectionCode.LIMIT_REACHED) { }
    }
    private class FakeUnknownError : BorrowError
    {
        public FakeUnknownError() : base((BorrowRejectionCode)999) { }
    }
}
