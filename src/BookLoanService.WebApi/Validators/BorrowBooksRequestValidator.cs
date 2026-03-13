namespace BookLoanService.WebApi.Validators;

public class BorrowBooksRequestValidator : AbstractValidator<BorrowBooksRequest>
{
    public BorrowBooksRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage(ValidationMessages.CustomerIdIsMissing)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.CustomerIdIsNegative);

        RuleFor(x => x.BookIds)
            .NotNull()
            .WithMessage(ValidationMessages.BooksListIsMissing)
            .NotEmpty()
            .WithMessage(ValidationMessages.BooksListIsEmpty)
            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationMessages.BooksListContainsDuplicates);
    }
}
