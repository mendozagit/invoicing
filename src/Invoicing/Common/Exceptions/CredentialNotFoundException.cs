namespace Invoicing.Common.Exceptions;

internal class CredentialNotFoundException : Exception
{
    public CredentialNotFoundException()
    {
    }

    public CredentialNotFoundException(string message) : base(message)
    {
    }

    public CredentialNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}