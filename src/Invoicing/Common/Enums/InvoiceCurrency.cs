using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums;

public enum InvoiceCurrency
{
    [EnumValue("MXN")] [XmlEnum("MXN")] MXN,

    [EnumValue("USD")] [XmlEnum("USD")] USD,

    [EnumValue("XXX")] [XmlEnum("XXX")] XXX
}