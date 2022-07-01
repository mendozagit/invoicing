using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.Common.Exceptions
{
    public class LastPaymentInvoiceNotFoundException : Exception
    {
        public LastPaymentInvoiceNotFoundException()
        {
        }

        public LastPaymentInvoiceNotFoundException(string message) : base(message)
        {
        }

        public LastPaymentInvoiceNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}