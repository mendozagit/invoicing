using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums
{
    public enum InvoiceSerie
    {
        [EnumValue("F")] [XmlEnum("F")] Ingreso,

        [EnumValue("NC")] [XmlEnum("NC")] Egreso,

        [EnumValue("T")] [XmlEnum("T")] Traslado,

        [EnumValue("N")] [XmlEnum("N")] Nomina,

        [EnumValue("P")] [XmlEnum("P")] Pago
    }
}