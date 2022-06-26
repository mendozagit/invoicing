using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Invoicing.Common.Attributes;

namespace Invoicing.Common.Enums
{
    public enum PayrollVersion

    {
        [EnumValue("1.1")] [XmlEnum("1.1")] V11,

        [EnumValue("1.2")] [XmlEnum("1.2")] V12
    }
}