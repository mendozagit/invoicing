using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoicing.Base;
using Invoicing.Contracts;

namespace Invoicing.Servicies
{
    public class InvoiceService
    {
        private readonly IInvoice _invoice;

        public InvoiceService(IInvoice invoice)
        {
            _invoice = invoice;
        }


      
    }
}