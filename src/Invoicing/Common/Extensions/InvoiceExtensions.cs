using System.Text.RegularExpressions;
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

        public static decimal ToSatRounding(this decimal value, int decimalPlaces,
            MidpointRounding roundingStrategy = MidpointRounding.AwayFromZero)
        {
            return Math.Round(value, decimalPlaces, roundingStrategy);
        }


        public static string ToValue(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes =
                (EnumValueAttribute[]) fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Value : enumValue.ToString();
        }

        /// <summary>
        /// Remove horizontal spaces at beginning, carriage return (CR), Line Feed (LF) and xml declaration on its own line.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>cleaned str</returns>
        public static string Clean(this string str)
        {
            #region Comments

            /*
             * Coincidencias Basicas
               .       - Cualquier Caracter, excepto nueva linea
               \d      - Cualquier Digitos (0-9)
               \D      - No es un Digito (0-9)
               \w      - Caracter de Palabra (a-z, A-Z, 0-9, _)
               \W      - No es un Caracter de Palabra.
               \s      - Espacios de cualquier tipo. (espacio, tab, nueva linea)
               \S      - No es un Espacio, Tab o nueva linea.
    
               Limites
               \b      - Limite de Palabra
               \B      - No es un Limite de Palabra
               ^       - Inicio de una cadena de texto
               $       - Final de una cadena de texto
    
               Cuantificadores:
               *       - 0 o Más
               +       - 1 o Más
               ?       - 0 o Uno
               {3}     - Numero Exacto
               {3,4}   - Rango de Numeros (Minimo, Maximo)
    
               Conjuntos de Caracteres
               []      - Caracteres dentro de los brackets
               [^ ]    - Caracteres que NO ESTAN dentro de los brackets
    
               Grupos
               ( )     - Grupo
               |       - Uno u otro
             */

            #endregion

            // A: remove horizontal spaces at beginning
            str = Regex.Replace(str, @"^\s*", string.Empty, RegexOptions.Multiline).TrimStart();


            // B: remove horizontal spaces + optional CR + LF
            str = Regex.Replace(str, @"\s*\r?\n", string.Empty, RegexOptions.Multiline).TrimEnd();

            // C: xml declaration on its own line
            str = str.Replace(@"?><", @$"?>{Environment.NewLine}<");


            return string.IsNullOrEmpty(str) ? string.Empty : str;
        }
    }
}