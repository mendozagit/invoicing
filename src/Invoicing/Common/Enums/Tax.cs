using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums
{
    public enum Tax
    {
        [EnumValue("001")] [XmlEnum("001")] Isr,

        [EnumValue("002")] [XmlEnum("002")] Iva,

        [EnumValue("003")] [XmlEnum("003")] Ieps
    }
}