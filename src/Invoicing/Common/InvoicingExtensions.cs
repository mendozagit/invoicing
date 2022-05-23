using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.Common
{
    public static class InvoicingExtensions
    {
        private const string SatFormat = "yyyy-MM-ddTHH:mm:ss";

        public static string ToSatFormat(this DateTime dateTime)
        {
            return dateTime.ToString(SatFormat);
        }
    }
}