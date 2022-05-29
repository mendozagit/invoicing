using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums
{
    public enum InvoiceVersion
    {
        [EnumValue("4.0")] [XmlEnum("4.0")] V40,
        [EnumValue("3.3")] [XmlEnum("3.3")] V33
    }
}