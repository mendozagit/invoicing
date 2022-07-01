using Invoicing.Base;
using Invoicing.Common.Constants;
using Invoicing.Common.Contracts;
using Invoicing.Common.Enums;
using Invoicing.Common.Extensions;
using Invoicing.Common.Serializing;

namespace Invoicing.Servicies;



public class CreditNoteService : InvoiceService, IHasStandardFields
{
    public CreditNoteService()
    {
        ConfigureStandardFields();
    }

    /// <summary>
    /// Configures the invoice's standard fields, based on the complement their incorporated (complement to be added to invoice). 
    /// When a complement is incorporated into an invoice, it is very common that standard concepts are added to the invoice and
    /// several fields are omitted from the invoice header. This is a strategy to audit almost anything through the concept of
    /// "invoice complement", which is basically an invoice with a concept that has certain configuration such as identifier or
    /// special descriptions, as well as set some invoice fields to null as part of complement configuration. 
    /// </summary>
    public void ConfigureStandardFields()
    {
        SerializerHelper.ConfigureSettingsForInvoice();


        // Invoice item


        // Invoice header
        _invoice.InvoiceVersion = InvoiceVersion.V40;
        _invoice.InvoiceDate = DateTime.Now.ToSatFormat();
        _invoice.InvoiceTypeId = InvoiceType.Egreso;
        _invoice.ExportId = "01"; //No aplica
    }
}