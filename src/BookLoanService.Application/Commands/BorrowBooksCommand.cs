namespace BookLoanService.Application.Commands;

public record BorrowBooksCommand(int CustomerId, IEnumerable<int> BookIds);
