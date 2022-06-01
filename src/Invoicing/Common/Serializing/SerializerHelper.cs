using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Invoicing.Common.Serializing
{
    /// <summary>
    /// XML serializer helper class. Serializes and deserializes objects from/to XML
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize/deserialize.
    /// Must have a parameterless constructor and implement <see cref="Serializable"/></typeparam>
    public class SerializerHelper
    {
        public static XmlWriterSettings DefaultXmlWriterSettings
        {
            get { return GetDefaultXmlWriterSettings(); }
        }

        public static XmlReaderSettings DefaultXmlReaderSettings
        {
            get { return GetDefaultXmlReaderSettings(); }
        }

        public static XmlSerializerNamespaces NamespacesIE40
        {
            get { return GetNamespacesIE40(); }
        }

        public static string SchemaLocationIE40
        {
            get { return GetSchemaLocationIE40(); }
        }

        public static string SchemaLocationP20
        {
            get { return GetSchemaLocationP20(); }
        }

        #region Private methods

        private static XmlWriterSettings GetDefaultXmlWriterSettings()
        {
            var xmlWriterSettings = new XmlWriterSettings
            {
                Async = false,
                CheckCharacters = true,
                CloseOutput = false,
                ConformanceLevel = ConformanceLevel.Document,
                DoNotEscapeUriAttributes = false,
                Encoding = Encoding.UTF8,
                Indent = false,
                IndentChars = " ",
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                NewLineChars = Environment.NewLine,
                NewLineHandling = NewLineHandling.Replace,
                NewLineOnAttributes = false,
                OmitXmlDeclaration = false,
                WriteEndDocumentOnClose = true
            };


            return xmlWriterSettings;
        }

        private static XmlReaderSettings GetDefaultXmlReaderSettings()
        {
            var settings = new XmlReaderSettings
            {
                Async = false,
                CheckCharacters = true,
                CloseInput = false,
                ConformanceLevel = ConformanceLevel.Document,
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = false,
                IgnoreProcessingInstructions = false,
                IgnoreWhitespace = false,
                LineNumberOffset = 0,
                LinePositionOffset = 0,
                MaxCharactersFromEntities = 0,
                MaxCharactersInDocument = 0,
                NameTable = null,
                ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings,
                ValidationType = ValidationType.None,
                XmlResolver = null
            };


            return settings;
        }


        private static XmlSerializerNamespaces GetNamespacesIE40()
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("cfdi", "http://www.sat.gob.mx/cfd/4");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            return ns;
        }


        private static string GetSchemaLocationIE40()
        {
            return "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";
        }


        private static string GetSchemaLocationP20()
        {
            return "http://www.sat.gob.mx/cfd/4 " +
                   "http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd " +
                   "http://www.sat.gob.mx/Pagos20 " +
                   "http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd";
        }

        #endregion
    }
}