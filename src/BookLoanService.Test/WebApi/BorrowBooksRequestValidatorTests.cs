namespace BookLoanService.Test.WebApi;

public class BorrowBooksRequestValidatorTests
{
    private readonly BorrowBooksRequestValidator _validator = new();

    [Fact]
    public void ShouldFailWhenCustomerIdIsZero()
    {
        // Arrange
        var request = new BorrowBooksRequest(CustomerId: 0, BookIds: [1]);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CustomerId");
    }

    [Fact]
    public void ShouldFailWhenCustomerIdIsNegative()
    {
        var request = new BorrowBooksRequest(CustomerId: -5, [1]);

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldFailWhenBookIdsIsEmpty()
    {
        // Arrange
        var request = new BorrowBooksRequest(CustomerId: 1, []);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "BookIds");
    }

    [Fact]
    public void ShouldFailWhenBookIdsContainDuplicates()
    {
        // Arrange
        var request = new BorrowBooksRequest(CustomerId: 1, [1, 2, 2]);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "BookIds");
    }

    [Fact]
    public void ShouldPassWhenCommandIsValid()
    {
        // Arrange
        var request = new BorrowBooksRequest(CustomerId: 1, [10, 20, 30]);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
