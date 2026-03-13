namespace BookLoanService.Test.Infrastructure;

public class BookRepositoryTests
{
    [Fact]
    public void ShouldAddAndStoreABook()
    {
        // Arrange
        var repository = new BookRepository();
        var initialBooksCount = repository.GetAll().Count;
        var newBookToAdd = new Book(
            id: initialBooksCount + 1, title: "Guerre et paix", author: "Leon Tolstoï");

        // Act
        repository.Add(newBookToAdd);

        // Assert
        var updatedBooksCount = repository.GetAll().Count;
        var lastAddedBook = repository.GetAll().Last();

        updatedBooksCount.Should().Be(initialBooksCount + 1);
        lastAddedBook.Should().Be(newBookToAdd);
    }

    [Fact]
    public void ShouldRetrieveAllStoredBooks()
    {
        // Arrange
        var repository = new BookRepository();
        var initialBooksCount = repository.GetAll().Count;

        // Act
        var books = repository.GetAll();

        // Assert
        books.Count.Should().Be(initialBooksCount);
    }
}
