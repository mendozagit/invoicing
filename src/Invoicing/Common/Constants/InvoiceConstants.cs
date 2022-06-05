namespace Invoicing.Common.Constants
{
    public static class InvoiceConstants
    {
        public const string SatInvoiceNationalTin = "XAXX010101000";
        public const string SatInvoiceForeignTin = "XEXX010101000";
        public const string SatInvoiceItemId = "01010101";
        public const string SatInvoiceUnitOfMeasureId = "H87";
        public const string SatInvoiceObjectId = "02"; //Sí objeto de impuesto;

        public const string SatPaymentItemId = "84111506";
        public const string SatPaymentUnitOfMeasureId = "ACT";
        public const string SatPaymentItemDescriptionId = "Pago";
        public const string SatPaymentObjectId = "01"; //No objeto de impuesto).;


        public const string CurrentInvoiceNamespace = "http://www.sat.gob.mx/cfd/4";
        public const string CurrentPaymentNamespace = "http://www.sat.gob.mx/cfd/4";

        public const string DeprecatedInvoiceNamespace = "http://www.sat.gob.mx/cfd/3";
        public const string DeprecatedPaymentNamespace = "http://www.sat.gob.mx/cfd/3";


        #region Namespaces and SchemaLocations

        public const string SatInvoiceXsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        public const string SatInvoice30Namespace = "http://www.sat.gob.mx/cfd/3";
        public const string SatInvoice40Namespace = "http://www.sat.gob.mx/cfd/4";
        public const string SatPayment10Namespace = "http://www.sat.gob.mx/Pagos";
        public const string SatPayment20Namespace = "http://www.sat.gob.mx/Pagos20";

        public const string SatInvoice30Schema = "http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd";
        public const string SatInvoice40Schema = "http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";
        public const string SatPayment10Schema = "http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd";
        public const string SatPayment20Schema = "http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd";

        #endregion
    }
}