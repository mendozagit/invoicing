using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.Common.Exceptions
{
    public class LastPaymentNotFoundException: Exception
    {
        public LastPaymentNotFoundException()
        {
        }

        public LastPaymentNotFoundException(string message) : base(message)
        {
        }

        public LastPaymentNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
