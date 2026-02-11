namespace wildcat_one_windows.Exceptions;

public class ValidationException : Exception
{
    public string? Field { get; }

    public ValidationException(string message, string? field = null)
        : base(message)
    {
        Field = field;
    }
}
