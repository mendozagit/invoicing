using Invoicing.Common.Attributes;

namespace Invoicing.Common.Extensions
{
    public static class InvoiceExtensions
    {
        private const string SatFormat = "yyyy-MM-ddTHH:mm:ss";

        public static string ToSatFormat(this DateTime dateTime)
        {
            return dateTime.ToString(SatFormat);
        }


        public static string ToValue(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes =
                (EnumValueAttribute[]) fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Value : enumValue.ToString();
        }
    }
}