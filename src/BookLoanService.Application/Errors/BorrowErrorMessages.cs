namespace BookLoanService.Application.Errors;

internal static class BorrowErrorMessages
{
    internal const string NotFound = "Le livre est introuvable";
    internal const string NotAvailable = "Le livre n'est pas disponible actuellement";
    internal const string LoanOverdue = "La date d'échéance d'un prêt est dépassée";
    internal const string UnknownError = "Erreur inconnue";

    internal static readonly string LimitReached =
        $"Le client a atteint la limite de {BorrowingPolicies.MaxSimultaneousLoans} emprunts simultanés";

    internal static string GetMessage(BorrowError error) =>
        error.Code switch
        {
            BorrowRejectionCode.NOT_FOUND => NotFound,
            BorrowRejectionCode.NOT_AVAILABLE => NotAvailable,
            BorrowRejectionCode.LIMIT_REACHED => LimitReached,
            BorrowRejectionCode.LOAN_OVERDUE => LoanOverdue,
            _ => UnknownError
        };
}
