namespace Balta.Domain.AccountContext.ValueObjects.Exceptions;

public class NullEmailException(string message = NullEmailException.DefaultErrorMessage) : Exception(message)
{
    private const string DefaultErrorMessage = "E-mail nulo";
}