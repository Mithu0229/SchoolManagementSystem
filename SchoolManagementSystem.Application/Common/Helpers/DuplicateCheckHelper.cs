namespace SchoolManagementSystem.Application.Common.Helpers;
public static class DuplicateCheckHelper
{
    public static string NormalizeString(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        return input
            .ToLowerInvariant()
            .Replace(" ", "")
            .Replace(".", "")
            .Replace("-", "")
            .Trim();
    }

    public static string NormalizeEmail(string email)
    {
        return email?.Trim().ToLowerInvariant() ?? string.Empty;
    }
}
