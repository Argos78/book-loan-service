namespace BookLoanService.WebApi.Responses;

public record BorrowBookResponse(
    IEnumerable<BorrowedBookDto> BorrowedBooks,
    IEnumerable<RejectedBookDto> RejectedBooks
);

public record BorrowedBookDto(
    int BookId,
    DateTime DueAt
);

public record RejectedBookDto(
    int BookId,
    string ReasonCode,
    string ReasonLabel
);
