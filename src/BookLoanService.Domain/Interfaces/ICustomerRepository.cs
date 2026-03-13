namespace BookLoanService.Domain.Interfaces;

public interface ICustomerRepository
{
    IReadOnlyCollection<Customer> GetAll();

    IReadOnlyCollection<Loan> GetAllActiveLoans();

    Customer? GetById(int id);
}
