using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.Common
{
    public static class InvoiceConstants
    {
        public const string NationalTin = "XAXX010101000";
        public const string ForeignTin = "XEXX010101000";
        public const string SatItemId = "01010101";
        public const string UnitOfMeasureId = "H87";
        public const string NamespaceV40 = "http://www.sat.gob.mx/cfd/4";
        public const string TaxObjectId = "02"; //Sí objeto de impuesto;
    }
}