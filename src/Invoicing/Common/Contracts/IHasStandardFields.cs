namespace Invoicing.Common.Contracts;

public interface IHasStandardFields
{
    /// <summary>
    /// Configures the invoice's standard fields, based on the complement their incorporated (complement to be added to invoice). 
    /// When a complement is incorporated into an invoice, it is very common that standard concepts are added to the invoice and
    /// several fields are omitted from the invoice header. This is a strategy to audit almost anything through the concept of
    /// "invoice complement", which is basically an invoice with a concept that has certain configuration such as identifier or
    /// special descriptions, as well as set some invoice fields to null as part of complement configuration. 
    /// </summary>
    void ConfigureStandardFields();
}