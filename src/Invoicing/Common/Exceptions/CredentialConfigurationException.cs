namespace Invoicing.Common.Exceptions;

internal class CredentialConfigurationException : Exception
{
    public CredentialConfigurationException()
    {
    }

    public CredentialConfigurationException(string message) : base(message)
    {
    }

    public CredentialConfigurationException(string message, Exception inner) : base(message, inner)
    {
    }
}