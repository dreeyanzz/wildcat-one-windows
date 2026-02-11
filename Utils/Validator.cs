using System.Text.RegularExpressions;
using wildcat_one_windows.Exceptions;

namespace wildcat_one_windows.Utils;

public static partial class Validator
{
    public static void ValidateRequired(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} is required", fieldName);
    }

    public static void ValidateStudentId(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException("Student ID is required", "studentId");

        if (!StudentIdRegex().IsMatch(value.Trim()))
            throw new ValidationException("Invalid Student ID format. Expected format: XX-XXXX-XXX", "studentId");
    }

    [GeneratedRegex(@"^\d{2}-\d{4}-\d{3}$")]
    private static partial Regex StudentIdRegex();
}
