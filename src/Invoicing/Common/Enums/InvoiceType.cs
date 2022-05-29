using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums;

public enum InvoiceType
{

    [EnumValue("I")][XmlEnum("I")] Ingreso,

    [EnumValue("E")][XmlEnum("E")] Egreso,

    [EnumValue("T")][XmlEnum("T")] Traslado,

    [EnumValue("N")][XmlEnum("N")] Nomina,

    [EnumValue("P")][XmlEnum("P")] Pago
}