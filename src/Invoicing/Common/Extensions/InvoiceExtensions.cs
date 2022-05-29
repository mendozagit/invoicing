namespace Invoicing.Common.Extensions
{
    public static class InvoiceExtensions
    {
        private const string SatFormat = "yyyy-MM-ddTHH:mm:ss";

        public static string ToSatFormat(this DateTime dateTime)
        {
            return dateTime.ToString(SatFormat);
        }
    }
}