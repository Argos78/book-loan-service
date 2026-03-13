namespace BookLoanService.Application.Results;

public record BorrowedBooksResult(
    IReadOnlyList<BorrowedBook> BorrowedBooks,
    IReadOnlyList<RejectedBook> RejectedBooks
);
