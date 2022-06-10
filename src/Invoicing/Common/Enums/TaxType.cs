using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums
{
    public enum TaxType
    {
        [EnumValue("Tasa")] [XmlEnum("Tasa")] Tasa,


        [EnumValue("Cuota")] [XmlEnum("Cuota")]
        Cuota,

        [EnumValue("Exento")] [XmlEnum("Exento")]
        Exento
    }
}