using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums;

public enum TaxType
{
    [EnumValue("Tasa")] [XmlEnum("Tasa")] Tasa,


    [EnumValue("Cuota")] [XmlEnum("Cuota")]
    Cuota,

    [EnumValue("Exento")] [XmlEnum("Exento")]
    Exento
}