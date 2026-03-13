namespace BookLoanService.Test.Utils;

internal static class RandomHelper
{
    private static readonly Random _random = new();

    public static int NextInt(int min = 1, int max = int.MaxValue)
        => _random.Next(min, max);
}
