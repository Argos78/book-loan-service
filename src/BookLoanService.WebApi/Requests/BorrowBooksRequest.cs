namespace BookLoanService.WebApi.Models;

public record BorrowBooksRequest(int CustomerId, IEnumerable<int> BookIds);
