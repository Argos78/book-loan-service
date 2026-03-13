namespace BookLoanService.Domain.Entities;

public class Book(int id, string title, string author)
{
    public int Id { get; } = id;

    public string Title { get; } = title;

    public string Author { get; } = author;

    public static Book Create(int id, string title, string author)
    {
        return new Book(id, title, author);
    }
}
