namespace wildcat_one_windows.Exceptions;

public class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message) { }
}
