using System.Xml;

namespace Invoicing.Common.Contracts
{
    public interface IHasComplement : IHasStandardFields
    {
        void AddInvoiceComplement(bool compute = true);
    }
}