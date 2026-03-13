namespace BookLoanService.Test.WebApi;

public class ErrorToHttpMapperTests
{
    [Fact]
    public void ShouldMapCustomerNotFoundErrorTo404()
    {
        // Arrange
        var mapper = new ErrorToHttpMapper();
        var error = new CustomerNotFoundError();
        var result = Result.Fail<object>(error);

        // Act
        var action = mapper.Map(result);

        // Assert
        action.Should().BeOfType<NotFoundObjectResult>();

        var notFound = (NotFoundObjectResult)action;

        notFound.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFound.Value.Should().BeOfType<ErrorResponse>();

        var response = (ErrorResponse)notFound.Value;

        response.Code.Should().Be(ErrorCodes.CustomerNotFound);
        response.Message.Should().Be(error.Message);
    }

    [Fact]
    public void ShouldMapUnknownErrorTo500()
    {
        // Arrange
        var mapper = new ErrorToHttpMapper();
        var error = new FakeUnknownError();
        var result = Result.Fail<object>(error);

        // Act
        var action = mapper.Map(result);

        // Assert
        action.Should().BeOfType<ObjectResult>();

        var objectResult = (ObjectResult)action;

        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        objectResult.Value.Should().BeOfType<ErrorResponse>();

        var response = (ErrorResponse)objectResult.Value;

        response.Code.Should().Be(ErrorCodes.UnexpectedError);
        response.Message.Should().Be(ErrorMessages.UnexpectedError);
    }

    private class FakeUnknownError : BorrowError
    {
        public FakeUnknownError() : base((BorrowRejectionCode)999) { }
    }
}
