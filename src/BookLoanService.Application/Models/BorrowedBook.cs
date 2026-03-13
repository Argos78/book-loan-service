namespace BookLoanService.Application.Models;

public record BorrowedBook(
    int BookId,
    DateTime DueAt
);
