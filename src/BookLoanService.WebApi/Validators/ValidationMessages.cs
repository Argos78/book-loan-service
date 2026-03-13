namespace BookLoanService.WebApi.Validation;

public static class ValidationMessages
{
    public const string CustomerIdIsMissing = "L'identifiant du client est obligatoire.";
    public const string CustomerIdIsNegative = "L'identifiant du client ne peut avoir une valeur négative.";

    public const string BooksListIsMissing = "La liste des livres est obligatoire.";
    public const string BooksListIsEmpty = "La liste des livres à emprunter ne peut pas être vide.";
    public const string BooksListContainsDuplicates = "La liste des livres à emprunter ne peut pas contenir plusieurs fois le même livre.";
}
