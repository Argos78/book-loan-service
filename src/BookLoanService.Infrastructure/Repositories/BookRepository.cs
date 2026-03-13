namespace BookLoanService.Infrastructure.Repositories;

internal class BookRepository : IBookRepository
{
    private readonly List<Book> _books;

    public BookRepository()
    {
        _books = new List<Book>
        {
            new Book(1, "Da Vinci Code", "Dan Brown"),
            new Book(2, "Dune", "Frank Herbert"),
            new Book(3, "Le Petit Prince", "Antoine de Saint-Exupéry"),
            new Book(4, "Les Misérables", "Victor Hugo"),
            new Book(5, "Le Seigneur des Anneaux", "J.R.R. Tolkien"),
            new Book(6, "L'Etranger", "Albert Camus"),
            new Book(7, "1984", "George Orwell"),
            new Book(8, "Gatsby le Magnifique", "F. Scott Fitzgerald"),
            new Book(9, "Harry Potter à l'école des sorciers", "J.K. Rowling")
        };
    }

    public IReadOnlyCollection<Book> GetAll()
    {
        return _books.AsReadOnly();
    }

    public int Add(Book book)
    {
        _books.Add(book);

        return _books.Count;
    }

    public bool Exists(int bookId)
    {
        return _books.Any(b => b.Id == bookId);
    }

    public int NextId()
    {
        return _books.Count == 0
            ? 1
            : _books.Max(b => b.Id) + 1;
    }
}
