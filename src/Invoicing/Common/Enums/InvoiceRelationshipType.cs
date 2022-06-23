using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums
{
    public enum InvoiceRelationshipType
    {
        // Nota de crédito de los documentos relacionados
        [EnumValue("01")] [XmlEnum("01")] CreditNoteOfRelatedDocuments,

        //Nota de débito de los documentos relacionados
        [EnumValue("02")] [XmlEnum("02")] DebitNoteOfRelatedDocuments,

        //Devolución de mercancía sobre facturas o traslados previos
        [EnumValue("03")] [XmlEnum("03")] ReturnOfMerchandiseOnPreviousInvoicesOrTransfers,

        //Sustitución de los CFDI previos
        [EnumValue("04")] [XmlEnum("04")] SubstitutionOfPreviousCfdi,

        //Traslados de mercancías facturados previamente
        [EnumValue("05")] [XmlEnum("05")] TransfersOfGoodsPreviouslyInvoiced,

        //Factura generada por los traslados previos
        [EnumValue("06")] [XmlEnum("06")] InvoiceGeneratedByThePreviousTransfereds,

        //CFDI por aplicación de anticipo
        [EnumValue("07")] [XmlEnum("07")] CfdiForApplicationOfPrepayments,
    }
}