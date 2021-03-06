using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums;

public enum PaymentVersion
{
    [EnumValue("1.0")] [XmlEnum("1.0")] V10,
    [EnumValue("2.0")] [XmlEnum("2.0")] V20
}