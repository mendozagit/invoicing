using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums
{
    public enum TaxationObject
    {
        /// <summary>
        /// No objeto de impuesto.
        /// </summary>
        [EnumValue("01")] [XmlEnum("01")] NotSubjectToTax,

        /// <summary>
        /// Sí objeto de impuesto.
        /// </summary>
        [EnumValue("02")] [XmlEnum("02")] YesSubjectToTax,

        /// <summary>
        /// Sí objeto del impuesto y no obligado al desglose.
        /// </summary>
        [EnumValue("03")] [XmlEnum("03")] YesSubjectToTaxButNoTaxBreakdown,
    }
}