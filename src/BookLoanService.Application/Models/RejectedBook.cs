namespace BookLoanService.Application.Models;

public record RejectedBook(
    int BookId,
    string ReasonCode,
    string ReasonLabel
);
