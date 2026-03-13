namespace BookLoanService.Infrastructure.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers;

    public CustomerRepository()
    {
        var today = DateTime.UtcNow.Date;

        _customers = new List<Customer>
        {
            new Customer(1, "Alice Martin", new List<Loan>
            {
                new Loan(
                    id: 1,
                    customerId: 1,
                    bookId: 1,
                    borrowedAt: today.AddDays(-30),
                    dueAt: today.AddDays(-9),
                    returnedAt: null),
                new Loan(
                    id: 5,
                    customerId: 1,
                    bookId: 9,
                    borrowedAt: today.AddDays(-15),
                    dueAt: today.AddDays(6),
                    returnedAt: today.AddDays(10)),
                new Loan(
                    id: 6,
                    customerId: 1,
                    bookId: 2,
                    borrowedAt: today.AddDays(-15),
                    dueAt: today.AddDays(6),
                    returnedAt: null),
                new Loan(
                    id: 7,
                    customerId: 1,
                    bookId: 4,
                    borrowedAt: today.AddDays(-10),
                    dueAt: today.AddDays(11),
                    returnedAt: null)
            }.ToList()),

            new Customer(2, "Bob Durand", new List<Loan>
            {
                new Loan(
                    id: 3,
                    customerId: 2,
                    bookId: 3,
                    borrowedAt: today.AddDays(-18),
                    dueAt: today.AddDays(3),
                    returnedAt: today.AddDays(-10))
            }.ToList()),

            new Customer(3, "Chloé Bernard", new List<Loan>
            {
                new Loan(
                    id: 2,
                    customerId: 3,
                    bookId: 8,
                    borrowedAt: today.AddDays(-30),
                    dueAt: today.AddDays(-9),
                    returnedAt: null)
            }.ToList()),

            new Customer(4, "David Lefèvre", new List<Loan>
            {
                new Loan(
                    id: 4,
                    customerId: 4,
                    bookId: 7,
                    borrowedAt: today.AddDays(-15),
                    dueAt: today.AddDays(6),
                    returnedAt: null)
            }.ToList())
        };
    }

    public IReadOnlyCollection<Customer> GetAll()
    {
        return _customers.AsReadOnly();
    }

    public IReadOnlyCollection<Loan> GetAllActiveLoans()
        => _customers.SelectMany(c => c.BorrowedBooks.Where(l => l.IsActive))
                     .ToList()
                     .AsReadOnly();

    public Customer? GetById(int id)
    {
        return _customers.Find(c => c.Id == id);
    }
}
