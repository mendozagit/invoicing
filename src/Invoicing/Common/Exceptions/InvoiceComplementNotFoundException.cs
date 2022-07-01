using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.Common.Exceptions
{
    public class InvoiceComplementNotFoundException : Exception
    {
        public InvoiceComplementNotFoundException()
        {
        }

        public InvoiceComplementNotFoundException(string message) : base(message)
        {
        }

        public InvoiceComplementNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}