using System.Xml;
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
		public static XmlSerializerNamespaces EmptyNamespaces
		{
			get 
			{
				return GetDefaultNamespaces();
			}
		}

		public static XmlWriterSettings IndentedSettings
		{
			get
			{
				return GetIndentedSettings();
			}
		}

		public static XmlWriterSettings NoXmlDeclarationSettings
		{
			get
			{
				return GetNoXmlDeclarationSettings();
			}
		}

		public static XmlReaderSettings XmlFragmentSettings
		{
			get
			{
				return GetReaderSettings();
			}
		}
		
		#region Private methods


		private static XmlSerializerNamespaces GetDefaultNamespaces()
		{
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", ""); // this removes the namespaces
			return ns;
		}

		private static XmlWriterSettings GetIndentedSettings()
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.IndentChars = "\t";

			return xmlWriterSettings;
		}

		private static XmlReaderSettings GetReaderSettings()
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			//settings.CheckCharacters = true;
			//settings.CloseInput = true;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			//settings.IgnoreComments = true;
			//settings.IgnoreProcessingInstructions = true;
			//settings.IgnoreWhitespace = true;
			//settings.Schemas = new System.Xml.Schema.XmlSchemaSet();
			//settings.

			return settings;
		}

		private static XmlWriterSettings GetNoXmlDeclarationSettings()
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			//xmlWriterSettings.CheckCharacters = true;
			//xmlWriterSettings.CloseOutput = true;
			//xmlWriterSettings.ConformanceLevel = ConformanceLevel.Auto;
			//xmlWriterSettings.Encoding = Encoding.UTF8;
			//xmlWriterSettings.Indent = true;
			//xmlWriterSettings.NewLineChars = "\n";
			//xmlWriterSettings.NewLineHandling = NewLineHandling.None;
			//xmlWriterSettings.NewLineOnAttributes = false;
			//xmlWriterSettings.OmitXmlDeclaration = false;
			//xmlWriterSettings.OutputMethod = XmlOutputMethod.AutoDetect;

			xmlWriterSettings.OmitXmlDeclaration = true;

			return xmlWriterSettings;
		}

		#endregion
	}
}
