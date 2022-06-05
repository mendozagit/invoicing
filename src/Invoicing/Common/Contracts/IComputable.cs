using System.Xml.Serialization;

namespace Invoicing.Common.Contracts;

public interface IComputable
{
    public void Compute();
}