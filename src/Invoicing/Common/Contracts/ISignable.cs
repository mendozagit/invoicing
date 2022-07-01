using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.Common.Contracts
{
    public interface ISignable
    {
        public string SignInvoice(bool compute = true);
    }
}